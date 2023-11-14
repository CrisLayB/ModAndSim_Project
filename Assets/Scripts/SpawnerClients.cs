using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerClients : MonoBehaviour
{
    [SerializeField] private GameObject _clientPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        if(_clientPrefab == null) Debug.LogError("Error from SpawnerClients.cs: client prefab is null");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
