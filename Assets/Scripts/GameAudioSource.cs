using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioSource : MonoBehaviour
{
    /// <summary>
    /// Awake es llamado cuando el script es cargado
    /// </summary>
    private void Awake()
    {
        // Tenemos que asegurarnos que solo tengas uno de estos
        // en el game object
        if(!AudioManager.Initialized)
        {
            // Inicializar el audio maneger y persistir el audio source
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            AudioManager.Initialize(audioSource);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Duplicaremos el game object, así que lo destruiremos
            Destroy(gameObject);
        }
    }
}