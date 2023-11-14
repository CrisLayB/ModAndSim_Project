using System;
using SimSharp;
using System.Collections.Generic;
using System.Linq;
using static SimSharp.Distributions;

class MySimulation
{
    private static double customerLambda = 10; // Customers/hour
    private static double cashierLambda = 6; // Customers/hour
    private static Simulation env;
    private static Random random = new Random(); // Propia instancia de Random

    static void Main(string[] args)
    {
        env = new Simulation(randomSeed: 42);
        var storeSim = new StoreSimulation(env, 3);
        env.Process(storeSim.Run());
        env.Run(TimeSpan.FromHours(8));
    }

    class StoreSimulation
    {
        private Simulation env;
        private List<Resource> cashiers;
        private List<TimeSeriesMonitor> cashierQueueMonitors; // Monitores para cada cajero
        private int customerAmount;
        private double sumOfWaitTime;
        private List<int> customersServedByCashier;
        private List<int> currentQueueLengths;
        public StoreSimulation(Simulation env, int cashierAmount)
        {
            this.env = env;
            cashiers = new List<Resource>();
            cashierQueueMonitors = new List<TimeSeriesMonitor>();
            currentQueueLengths = new List<int>(new int[cashierAmount]);
            for (int i = 0; i < cashierAmount; i++)
            {
                var cashier = new Resource(env, 1);
                cashiers.Add(cashier);
                cashierQueueMonitors.Add(new TimeSeriesMonitor(env, collect: true));
            }

            customerAmount = 0;
            sumOfWaitTime = 0;
            customersServedByCashier = new List<int>(new int[cashierAmount]);
        }

        public IEnumerable<Event> Run()
        {
            while (true)
            {
                yield return env.TimeoutD(Exponential(customerLambda));
                env.Process(CustomerBehavior());
            }
        }

        private IEnumerable<Event> CustomerBehavior()
        {
            customerAmount++;
            string customerName = "Customer " + customerAmount;
            double arrivalTime = (env.Now - env.StartDate).TotalHours;
            Console.WriteLine($"{customerName} arrives at {FloatToHoursMinutes(arrivalTime)}");

            int cashierIndex = ChooseCashier();
            currentQueueLengths[cashierIndex]++; // Incrementar la longitud de la cola
            var cashier = cashiers[cashierIndex];
            using (var request = cashier.Request())
            {
                yield return request;

                double serviceStartTime = (env.Now - env.StartDate).TotalHours;
                double waitTime = serviceStartTime - arrivalTime;
                sumOfWaitTime += waitTime;
                Console.WriteLine($"{customerName} waits for {FloatToHoursMinutes(waitTime)}");

                yield return env.TimeoutD(Exponential(cashierLambda));
                double serviceEndTime = (env.Now - env.StartDate).TotalHours;
                Console.WriteLine($"{customerName} is done and leaves at {FloatToHoursMinutes(serviceEndTime)}");

                customersServedByCashier[cashierIndex]++;
            }
        }

        private int ChooseCashier()
        {
            int chosenCashier = 0;
            int minQueueLength = int.MaxValue;

            for (int i = 0; i < cashiers.Count; i++)
            {
                if (currentQueueLengths[i] < minQueueLength)
                {
                    minQueueLength = currentQueueLengths[i];
                    chosenCashier = i;
                }
            }

            return chosenCashier;
        }
    }

    static string FloatToHoursMinutes(double floatVal)
    {
        int hours = (int)floatVal;
        int minutes = (int)((floatVal - hours) * 60);
        return string.Format("{0}:{1:D2}", hours, minutes);
    }

    static double Exponential(double lambda)
    {
        double u = random.NextDouble();
        return -Math.Log(u) / lambda;
    }
}
