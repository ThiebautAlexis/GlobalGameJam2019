using System;
using UnityEngine;
using UnityEngine.Audio;


/*public class AudioManagerPersonal : MonoBehaviour
{
    #region F/P
    public static AudioManagerPersonal Instance;
    public Sound[] sounds;
    #endregion

    #region Methods
    public void PlaySound(string _soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == _soundName);
        s.source.Play();
    }

    #endregion

    #region UnityMethods
    private void Awake()
    {
        if (!Instance) Instance = this;
        foreach(Sound s in sounds)
        {
            s.source =  gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.Loop;
        }
    }

    private void Start()
    {
        PlaySound("Theme");
    }
    #endregion*/
//}