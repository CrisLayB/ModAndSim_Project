using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour
{
    [SerializeField] private double _arrivalTime = 0.0f;
    [SerializeField] private double _attendedTime;
    
    public double ArrivalTime
    {
        get { return _arrivalTime; }
    }

    public double AttendedTime
    {
        get {return _arrivalTime; }
    }

    public void Initialize(double pArrivalTime)
    {
        _arrivalTime = pArrivalTime;
    }

    public void Attended(double pAttendedTime)
    {
        _attendedTime = pAttendedTime;
    }
}
