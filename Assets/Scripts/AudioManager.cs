using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static bool initialized = false;
    private static AudioSource audioSource;
    private static Dictionary<AudioClipName, AudioClip> audioClips = 
        new Dictionary<AudioClipName, AudioClip>();

    /// <summary>
    /// Verificar si ya fue inicializado o todavía no
    /// </summary>
    public static bool Initialized
    {
        get { return initialized; }
    }
    
    /// <summary>
    /// Inicializar el Audio Maneger con los audios disponibles
    /// </summary>
    /// <param name="name">nombre del audio que se ejecutará</param>
    public static void Initialize(AudioSource source)
    {
        initialized = true;
        audioSource = source;

        // Agregar los sonidos
        addSound(AudioClipName.bell, "bell");
        addSound(AudioClipName.fire, "fire");
        addSound(AudioClipName.ketchup_sound, "ketchup_sound");
        addSound(AudioClipName.knifesharpener1, "knifesharpener1");
        addSound(AudioClipName.plastic_bag_sound, "plastic_bag_sound");
        addSound(AudioClipName.point_bell, "point_bell");
        addSound(AudioClipName.analog_alarm_clock, "analog_alarm_clock");
        addSound(AudioClipName.pop1, "pop1");
        addSound(AudioClipName.pop2, "pop2");
        addSound(AudioClipName.pop3, "pop3");
        addSound(AudioClipName.cabague_sound, "cabague_sound");
        addSound(AudioClipName.guacamole_sound, "guacamole_sound");
    }

    /// <summary>
    /// Se almacenará el audio ingresado en el diccionario de Audio Clips
    /// </summary>
    /// <param name="clipName">nombre del audio que se ejecutará</param>
    /// <param name="audioClip">nombre del audio que se ejecutará</param>
    private static void addSound(AudioClipName clipName, string audioClip)
    {
        audioClips.Add(clipName, Resources.Load<AudioClip>(audioClip));
    }

    /// <summary>
    /// Ejecuta el audio con el clip y nombre incluido
    /// </summary>
    /// <param name="name">nombre del audio que se ejecutará</param>
    public static void Play(AudioClipName name)
    {
        audioSource.PlayOneShot(audioClips[name]);
    }    

    public static void PlayContinueSound(AudioSource audio, bool activedSound)
    {
        audio.loop = activedSound;
        audio.enabled = activedSound;
    }
}