using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Murgn
{
    public class VolumeChanger : MonoBehaviour
	{
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Slider slider;

        private void Start()
        {
            float volume = PlayerPrefs.GetFloat("Volume");
            if (volume == 0) volume = 0.5f;

            audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20.0f);
            
            if(slider != null)
                slider.value = volume * 10000;
        }

        public void SetVolume(float volume)
        {
            audioMixer.SetFloat("Volume", Mathf.Log10(volume / 10000) * 20.0f);
            PlayerPrefs.SetFloat("Volume", volume / 10000);
        }
        
        // Volume slider should have a min value of 1, max value of 10000
        public void VolumeChange(bool increase = true)
        {
            Debug.Log("Changed volume");
            slider.value += increase ? 1250 : -1250;
        }
    }   
}