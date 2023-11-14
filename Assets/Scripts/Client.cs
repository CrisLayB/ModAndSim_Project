using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour
{
    [SerializeField] private double _arrivalTime = 0.0f;
    [SerializeField] private double _attendedTime;
    [SerializeField] private bool _attended = false;
    [SerializeField] private float _xlimit;
    [SerializeField] private float _speed;

    void Update()
    {
        if(_attended) QuitFromStore();
        if(!_attended) MoveToShucosStore();
    }

    private void MoveToShucosStore()
    {
        bool inPair = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out RaycastHit hitinfo, 3.5f);
        if(inPair) return;

        if(transform.position.x < 2.00) transform.Translate(new Vector3(1, 0, 0) * _speed * Time.deltaTime);
    }

    private void QuitFromStore()
    {
        // ! Lo dejare arriba solo por molestar JAJAJAJJAJAJA
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }
    
    public double ArrivalTime
    {
        get { return _arrivalTime; }
    }

    public double AttendedTime
    {
        get {return _arrivalTime; }
    }

    public bool IsAttended
    {
        get {return _attended;}
    }

    public void Initialize(double pArrivalTime)
    {
        _arrivalTime = pArrivalTime;
    }

    public void Attended(double pAttendedTime)
    {
        _attendedTime = pAttendedTime;
    }

    public double diferenceTime()
    {
        return  _attendedTime - _arrivalTime;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("shuco"))
        {
            Destroy(other.gameObject);
            _attended = true;
        }
    }
}
