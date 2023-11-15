using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShucosSimulation : MonoBehaviour
{
    [SerializeField] private int shuqueroAmount = 1;
    [SerializeField] private GameObject clientPrefab;
    [SerializeField] private double customerLambda = 1;
    [SerializeField] private double shuqueroLambda = 6;

    private List<Queue<Client>> shuqueroQueues;
    private List<int> currentQueueLengths;
    private int customerAmount;
    private double sumOfWaitTime;
    private List<int> customersServedByShuquero;

    void Start()
    {
        shuqueroQueues = new List<Queue<Client>>();
        currentQueueLengths = new List<int>(new int[shuqueroAmount]);

        for (int i = 0; i < shuqueroAmount; i++)
        {
            shuqueroQueues.Add(new Queue<Client>());
        }

        customerAmount = 0;
        sumOfWaitTime = 0;
        customersServedByShuquero = new List<int>(new int[shuqueroAmount]);

        StartCoroutine(RunSimulation());
    }

    IEnumerator RunSimulation()
    {
        while (true)
        {
            yield return new WaitForSeconds((float)Exponential(customerLambda));
            Debug.Log($"Seconds for spawn: {(float)Exponential(customerLambda)} con valor lambda de {customerLambda}");
            SpawnCustomer();
        }
    }

    void SpawnCustomer()
    {
        customerAmount++;
        string customerName = "Client " + customerAmount;
        double arrivalTime = Time.time;

        Debug.Log($"{customerName} arrives at {FloatToHoursMinutes(arrivalTime)}");

        int cashierIndex = ChooseCashier();
        currentQueueLengths[cashierIndex]++;

        GameObject customerObject = Instantiate(clientPrefab, transform.position, Quaternion.identity);
        Client customer = customerObject.GetComponent<Client>();
        customer.Initialize(arrivalTime);

        shuqueroQueues[cashierIndex].Enqueue(customer);

        if (shuqueroQueues[cashierIndex].Count == 1)
        {
            StartCoroutine(CustomerBehavior(cashierIndex));
        }
    }

    IEnumerator CustomerBehavior(int cashierIndex)
    {
        Queue<Client> cashierQueue = shuqueroQueues[cashierIndex];
        Client currentCustomer = cashierQueue.Peek();

        double serviceStartTime = Time.time;
        double waitTime = serviceStartTime - currentCustomer.ArrivalTime;
        sumOfWaitTime += waitTime;
        Debug.Log($"{currentCustomer.name} waits for {FloatToHoursMinutes(waitTime)}");
        
        yield return new WaitForSeconds((float)Exponential(shuqueroLambda)); // Use un valor lambda apropiado

        if(currentCustomer.IsAttended)
        {
            double serviceEndTime = Time.time;
            currentCustomer.Attended(serviceEndTime);
            Debug.Log($"{currentCustomer.name} is done and leaves at {FloatToHoursMinutes(currentCustomer.AttendedTime)} and waiting time {FloatToHoursMinutes(currentCustomer.diferenceTime())}");

            customersServedByShuquero[cashierIndex]++;
            currentQueueLengths[cashierIndex]--;

            cashierQueue.Dequeue();
            Destroy(currentCustomer.gameObject);
        }

        if (cashierQueue.Count > 0)
        {
            StartCoroutine(CustomerBehavior(cashierIndex));
        }
    }

    private int ChooseCashier()
    {
        int chosenCashier = 0;
        int minQueueLength = int.MaxValue;

        for (int i = 0; i < currentQueueLengths.Count; i++)
        {
            if (currentQueueLengths[i] < minQueueLength)
            {
                minQueueLength = currentQueueLengths[i];
                chosenCashier = i;
            }
        }

        return chosenCashier;
    }

    static string FloatToHoursMinutes(double floatVal)
    {
        int hours = (int)floatVal;
        int minutes = (int)((floatVal - hours) * 60);
        return string.Format("{0}:{1:D2}", hours, minutes);
    }

    static double Exponential(double lambda)
    {
        double u = Random.value;
        return -Mathf.Log((float)u) / lambda;
    }
}
