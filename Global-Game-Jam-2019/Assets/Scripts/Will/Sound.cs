using UnityEngine;
using UnityEngine.Audio;
using System;

[Serializable]
public class Sound 
{
    #region F/P
    public string name;
    public AudioClip Clip;
    [Range(0f, 1f)]
    public float volume;
    public float pitch;
    public bool Loop = false;
    [HideInInspector]
    public AudioSource source;

    #endregion

    #region Methods

    #endregion

    #region UnityMethods

    #endregion
}
