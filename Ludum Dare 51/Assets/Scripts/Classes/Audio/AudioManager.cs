using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Murgn.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        public List<AudioPlayer> audioPlayers;

        private void Start()
        {
            for (int i = 0; i < audioPlayers.Count; i++)
            {
                GameObject audioOrigin = new GameObject();
                audioOrigin.name = string.Format("{0}'s AudioPlayer", audioPlayers[i].audioCategory);
                audioOrigin.transform.parent = transform;

                for (int j = 0; j < audioPlayers[i].audioClips.Length; j++)
                {
                    audioPlayers[i].audioClips[j].source = audioOrigin.AddComponent<AudioSource>();
                    audioPlayers[i].audioClips[j].source.clip = audioPlayers[i].audioClips[j].clip;
                    audioPlayers[i].audioClips[j].source.volume = audioPlayers[i].audioClips[j].volume / 100;
                    audioPlayers[i].audioClips[j].source.pitch = audioPlayers[i].audioClips[j].pitch;
                    audioPlayers[i].audioClips[j].source.loop = audioPlayers[i].audioClips[j].loop;
                    audioPlayers[i].audioClips[j].source.playOnAwake = audioPlayers[i].audioClips[j].playOnAwake;
                    audioPlayers[i].audioClips[j].source.outputAudioMixerGroup = audioPlayers[i].audioMixerGroup;
                    audioPlayers[i].audioClips[j].spatialBlend = audioPlayers[i].audioClips[j].spatialBlend;
                }
            }
        }

        private Audio ClipFinder(string name)
        {
            string[] seperatedString = name.Split('/');

            AudioPlayer audioPlayer = audioPlayers.Find(audioPlayer => audioPlayer.audioCategory == seperatedString[0]);
            if (audioPlayer == null) { Debug.LogWarningFormat("Audio Category: {0} not found!", seperatedString[0]); return null; }

            Audio audio = Array.Find(audioPlayer.audioClips, audio => audio.name == seperatedString[1]);
            if (audio == null) { Debug.LogWarningFormat("Sound: {0} not found!", seperatedString[1]); return null; }

            return audio;
        }

        public void Play(string name = "Category/audioClip")
        {
            Audio audio = ClipFinder(name);

            audio.source.Play();
        }

        public void Play(string name = "Category/audioClip", float minPitch = 0.95f, float maxPitch = 1.05f)
        {
            Audio audio = ClipFinder(name);

            audio.source.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
            audio.source.Play();
        }

        public void Play(string name = "Category/audioClip", float pitchOffset = 0.1f)
        {
            Audio audio = ClipFinder(name);
            audio.source.pitch = UnityEngine.Random.Range(audio.pitch - pitchOffset, audio.pitch + pitchOffset);
            audio.source.Play();
        }

        public void Stop(string name = "Category/audioClip")
        {
            Audio audio = ClipFinder(name);

            audio.source.Stop();
        }

        public void Mute(string name = "Category/audioClip", bool mute = true)
        {
            Audio audio = ClipFinder(name);

            audio.source.mute = mute;
        }

        public void MuteAll(bool mute)
        {
            for (int i = 0; i < audioPlayers.Count; i++)
            {
                for (int j = 0; j < audioPlayers[j].audioClips.Length; j++)
                {
                    audioPlayers[i].audioClips[j].source.mute = mute;
                }
            }
        }
    }   
}
