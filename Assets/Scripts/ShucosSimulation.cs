using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShucosSimulation : MonoBehaviour
{
    [SerializeField] private int cashierAmount = 1;
    [SerializeField] private GameObject clientPrefab;
    [SerializeField] private double customerLambda = 100;

    private List<Queue<Client>> cashierQueues;
    private List<int> currentQueueLengths;
    private int customerAmount;
    private double sumOfWaitTime;
    private List<int> customersServedByCashier;

    void Start()
    {
        cashierQueues = new List<Queue<Client>>();
        currentQueueLengths = new List<int>(new int[cashierAmount]);

        for (int i = 0; i < cashierAmount; i++)
        {
            cashierQueues.Add(new Queue<Client>());
        }

        customerAmount = 0;
        sumOfWaitTime = 0;
        customersServedByCashier = new List<int>(new int[cashierAmount]);

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

        cashierQueues[cashierIndex].Enqueue(customer);

        if (cashierQueues[cashierIndex].Count == 1)
        {
            StartCoroutine(CustomerBehavior(cashierIndex));
        }
    }

    IEnumerator CustomerBehavior(int cashierIndex)
    {
        Queue<Client> cashierQueue = cashierQueues[cashierIndex];
        Client currentCustomer = cashierQueue.Peek();

        double serviceStartTime = Time.time;
        double waitTime = serviceStartTime - currentCustomer.ArrivalTime;
        sumOfWaitTime += waitTime;
        Debug.Log($"{currentCustomer.name} waits for {FloatToHoursMinutes(waitTime)}");
        
        yield return new WaitForSeconds((float)Exponential(6)); // Use un valor lambda apropiado

        if(currentCustomer.IsAttended)
        {
            double serviceEndTime = Time.time;
            Debug.Log($"{currentCustomer.name} is done and leaves at {FloatToHoursMinutes(serviceEndTime)}");

            customersServedByCashier[cashierIndex]++;
            currentQueueLengths[cashierIndex]--;

            cashierQueue.Dequeue();
            Destroy(currentCustomer.gameObject);

            if (cashierQueue.Count > 0)
            {
                StartCoroutine(CustomerBehavior(cashierIndex));
            }
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
