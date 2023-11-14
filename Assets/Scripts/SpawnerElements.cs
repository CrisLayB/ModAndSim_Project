using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerElements : MonoBehaviour
{
    // ==> Objetos
    [SerializeField] private GameObject element;
    [SerializeField] private string tagElement;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Quaternion rotationElement;    

    // ==> Atributos
    private List<GameObject> elementsInsideTrigger = new List<GameObject>();
    
    
    void Start()
    {
        // Inicializnado propiedades
        if(spawnPoint == null) spawnPoint = GetComponent<Transform>();

        // Revisar si los objetos y atributos son nulos o no        
        if(tagElement == null) Debug.LogError("SpawnElement Script: Tag of Element is NULL.");
        StartCoroutine("SpawnElementWithTime");
    }

    private void InstantiateElement()
    {
        if(element == null) 
        {
            Debug.LogError("SpawnElement Script: Element is NULL.");
            return;
        }

        AudioManager.Play(AudioClipName.pop1);
        Instantiate(element, spawnPoint.position, rotationElement);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag.Equals(tagElement))
        {
            if (!elementsInsideTrigger.Contains(other.gameObject))
            {
                elementsInsideTrigger.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        elementsInsideTrigger.Remove(other.gameObject);

        if(elementsInsideTrigger.Count == 0)
        {
            StartCoroutine("SpawnElementWithTime");
        }
    }

    IEnumerator SpawnElementWithTime()
    {
        yield return new WaitForSeconds(1f);
        if(elementsInsideTrigger.Count == 0) InstantiateElement();
    }
}
