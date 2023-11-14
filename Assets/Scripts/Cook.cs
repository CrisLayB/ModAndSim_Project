using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Cook:
 * Para calentar un elemento o alimento que posea este script
 */
public class Cook : MonoBehaviour
{
    [SerializeField] private int segundosPreparar = 1;
    [SerializeField] protected Material materialReady;
    private AudioSource cookSound;
    private bool canBeCook = true;
    private int s;

    private void Start()
    {
        GameObject gameObjectCookSound = GameObject.Find("CookFireSound");

        if(gameObjectCookSound != null)
        {
            cookSound = gameObjectCookSound.GetComponent<AudioSource>();

            if(cookSound == null)
            {
                Debug.LogError("Cook Fire Sound Audio Source is Null");
            }
        }
        else
        {
            Debug.LogError("Cook Fire Sound Game Object is Null");
        }
        
        s = segundosPreparar;        
    }

    private void OnCollisionEnter(Collision other)
    {
        StartToCook(other);
    }

    private void OnCollisionExit(Collision other)
    {                
        if (other.gameObject.tag.Equals("barbecue"))
        {
            AudioManager.PlayContinueSound(cookSound, false);
            CancelInvoke();
        }
    }

    private void updateTimer()
    {        
        s--;
        if(s == 0) ChangueMaterial();     
    }

    virtual public void ChangueMaterial()
    {
        // Agarrar el puro MeshRenderer del object que tiene este script
        AudioManager.Play(AudioClipName.point_bell);
        this.GetComponent<MeshRenderer>().material = materialReady;        
    }

    protected void ChangueCookToTrue() { canBeCook = true;  }

    protected int GetTimeCook() { return s; }

    protected void StartToCook(Collision other)
    {
        if (other.gameObject.tag.Equals("barbecue"))
        {
            if(cookSound != null) AudioManager.PlayContinueSound(cookSound, true);
            if(canBeCook == true) InvokeRepeating("updateTimer", 0f, 1f);
        }
    }
}
