using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

//Referencias: https://www.youtube.com/watch?v=NJJK2ySgvXk&ab_channel=GameDevTraum 
public class Timer : MonoBehaviour
{
    // --> Atributos
    [SerializeField] private int minutos;
    [SerializeField] private int segundos;
    private int m, s;
    [SerializeField] private TextMeshProUGUI textoT;
    private bool timeup = false;

    private int secondsState = 1;
    private bool state = false;

    // --> Propiedades
    public bool Timeup
    {
        get
        {
            return timeup;
        }
    }

    // --> Propiedades    
    public bool TimeUp
    {
        get
        {
            return timeup;
        }
    }

    public bool State
    {
        get
        {
            return state;
        }
    }

    // * =============================================================================
    // * === UNITY METHODS ===========================================================
    // * =============================================================================

    private void Start()
    {
        startTimer();
    }

    // * =============================================================================
    // * === OWN METHODS =============================================================
    // * =============================================================================
    
    public void startTimer()
    {
        m = minutos;
        s = segundos;
        writeTimer(m, s);
        InvokeRepeating("updateTimer", 0f, 1f);
    }

    public void stopTimer()
    {
        CancelInvoke();
    }

    private void updateTimer()
    {
        // Revisar los segundos del estado de los enemigos
        secondsState++;
        if (secondsState > 10)
        {
            state = !state;
            secondsState = 1;
        }

        // Contar el tiempo restante
        s--;
        if (s < 0)
        {
            if (m == 0) //Caso1: Termina el juego
            {
                timeup = true;
                StartCoroutine("GoToMainMenu");
            }
            else //Caso2: Se acaba el primer minuto
            {
                m--;
                s = 59;
            }
        }
        writeTimer(m, s);
    }

    private void writeTimer(int m, int s)
    {
        textoT.text = (s < 10) ? textoT.text = "TIME: " + m.ToString() + ":0" + s.ToString() : textoT.text = "TIME: " + m.ToString() + ":" + s.ToString();
        if (s <= 0) textoT.text = "TIME: " + m.ToString() + ":00";
    }

    IEnumerator GoToMainMenu()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }
}
//Referencias: https://www.youtube.com/watch?v=NJJK2ySgvXk&ab_channel=GameDevTraum 