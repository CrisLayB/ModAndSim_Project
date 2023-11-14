using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private Timer timer;
    private int counter = 0;
    IEnumerator myCouroutine;

    // Start is called before the first frame update
    private void Start()
    {
        myCouroutine = counterLoop(0.1f);
        StartCoroutine(myCouroutine);
    }

    public void addCount()
    {
        if(!timer.Timeup)
            counter++;
    }

    IEnumerator counterLoop(float seconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(seconds);

            string information = (!timer.Timeup) ? "Counter: " + counter : "Final: " + counter;
            counterText.text = information;
        }
    }
}
