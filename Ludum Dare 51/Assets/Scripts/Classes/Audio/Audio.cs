using UnityEngine;
using UnityEngine.Audio;

namespace Murgn.Audio
{
    [System.Serializable]
    public class Audio
    {
        [Header("Audio Settings")]
        public string name;
        public AudioClip clip;

        [Range(0, 100)]
        public float volume = 100;
        [Range(-3, 3)]
        public float pitch = 1;
        [Range(0, 1)]
        public float spatialBlend = 0;

        public bool loop;
        public bool playOnAwake;

        [HideInInspector]
        public AudioSource source;
    }   
}
