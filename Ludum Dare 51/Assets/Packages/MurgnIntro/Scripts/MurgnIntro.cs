using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.UI;

namespace Murgn.Intro
{
    public class MurgnIntro : MonoBehaviour
    {
        [SerializeField] private int sceneToLoad = 1;
        [SerializeField] private bool rumble;

        [SerializeField] private int blinkingCount;
        [SerializeField] private float blinkTimer;
        [SerializeField] private float murgnTimer;
        [SerializeField] private float loadSceneTimer;
        [SerializeField] private AudioClip[] murgnClips;
        [SerializeField] private string murgnText;
        [SerializeField] private Camera introCamera;

        private Queue<char> blinkers = new Queue<char>();
        private Queue<char> murgnChars = new Queue<char>();

        private float timerCounter;
        private float loadSceneTimerCounter;
        private AudioSource audioSource;
        private Image background;
        private Image border;
        private TextMeshProUGUI text;
        private string currentText;
        private bool blink = true;
        private bool audioPlayed;
        private bool loadedScene;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            background = transform.GetChild(0).GetComponent<Image>();
            border = transform.GetChild(1).GetComponent<Image>();
            text = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

            audioSource.clip = murgnClips[Random.Range(0, murgnClips.Length)];

            for (int i = 0; i < blinkingCount; i++)
            {
                blinkers.Enqueue('_');
                blinkers.Enqueue(' ');
            }
            
            char[] textArray = murgnText.ToCharArray();

            for (int i = 0; i < textArray.Length; i++)
            {
                murgnChars.Enqueue(textArray[i]);
            }

            loadSceneTimerCounter = loadSceneTimer + Time.time;

            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(introCamera.gameObject);
        }


        private void Update()
        {
            if ((Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) || (Gamepad.current != null && Gamepad.current.children.Any(x => x.IsPressed())))
            {
                if(!loadedScene)
                {
                    LoadScene(0.0f);
                }
            }
            
            if (timerCounter <= Time.time && !loadedScene)
            {
                if (blinkers.Count != 0)
                {
                    text.text = blinkers.Dequeue().ToString();
                    timerCounter = blinkTimer + Time.time;
                }
                else if (murgnChars.Count != 0)
                {
                    if(rumble) StartCoroutine(PlayRumble(0.5f, 0.0f, murgnTimer / 2));

                    if (!audioPlayed)
                    {
                        audioSource.Play();
                        audioPlayed = true;
                    }
                    currentText += murgnChars.Dequeue().ToString();
                    
                    text.text = currentText + (murgnChars.Count != 0 ? blink ? "" : "_" : "").ToString();
                    blink = !blink;

                    timerCounter = murgnTimer + Time.time;
                }

                if(loadSceneTimerCounter <= Time.time && !loadedScene)
                {
                    LoadScene(1.0f);
                }                
            }            
        }

        private void OnDisable()
        {
            if (Gamepad.current != null)
                Gamepad.current.ResetHaptics();
        }

        private void LoadScene(float delay)
        {
            loadedScene = true;
            SceneManager.LoadScene(sceneToLoad);
            StartCoroutine(FadeOut(delay, 2.0f));
        }

        public IEnumerator PlayRumble(float lowfrequency, float highfrequency, float length)
        {
            if (Gamepad.current == null) yield break;
            Gamepad.current.SetMotorSpeeds(lowfrequency, highfrequency);
            yield return new WaitForSeconds(length);
            Gamepad.current.ResetHaptics();
        }

        private IEnumerator FadeOut(float delay, float duration)
        {
            yield return new WaitForSeconds(delay);
            Destroy(introCamera.gameObject);
            float time = 0;

            while (time < duration)
            {
                background.color = new Color(background.color.r, background.color.g, background.color.b, Mathf.Lerp(text.color.a, 0.0f, time / duration));
                border.color = new Color(border.color.r, border.color.g, border.color.b, Mathf.Lerp(text.color.a, 0.0f, time / duration));
                text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(text.color.a, 0.0f, time / duration));
                time += Time.deltaTime;
                yield return null;
            }
            background.color = new Color(background.color.r, background.color.g, background.color.b, 0.0f);
            border.color = new Color(border.color.r, border.color.g, border.color.b, 0.0f);
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0.0f);
            Destroy(gameObject);
        }
    }   
}
