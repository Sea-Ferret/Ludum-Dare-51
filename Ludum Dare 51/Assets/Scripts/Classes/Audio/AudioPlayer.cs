using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Murgn.Audio
{
    [System.Serializable]
    public class AudioPlayer : MonoBehaviour
    {
        public string audioCategory = "AudioPlayer";
        public AudioMixerGroup audioMixerGroup;
        [NonReorderable] public Audio[] audioClips;

        private AudioManager am;
        private void Awake()
        {
            am = AudioManager.instance;
            am.audioPlayers.Add(this);
        }
    }
}

