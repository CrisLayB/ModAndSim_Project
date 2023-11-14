using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spoon : MonoBehaviour
{
    [SerializeField] private GameObject guacamole, cabbage;
    private int contents = 0;

    public int Contents
    {
        get {return contents;}
    }

    private void OnTriggerEnter(Collider other)
    {
        //si la cuchara esta vacia (llenar con guacamole o cabbage)
        if ((other.gameObject.tag.Equals("guacamole") && contents == 0))
        {
            guacamole.SetActive(true); //lenar cuchara con guacamole
            contents = 1;
        }
        if ((other.gameObject.tag.Equals("cabbage") && contents == 0))
        {
            cabbage.SetActive(true); //lenar cuchara con guacamole
            contents = 2;
        }        
    }

    private void OnCollisionEnter(Collision other) 
    {
        // Para indicar que ya se hecho el elemento en el pan
        if ((other.gameObject.tag.Equals("bread") && contents > 0))
        {
            guacamole.SetActive(false);
            cabbage.SetActive(false);
            contents = 0;
        }
    }
}
