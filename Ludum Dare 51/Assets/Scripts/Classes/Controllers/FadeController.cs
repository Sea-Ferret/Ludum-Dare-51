using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Murgn
{
    public class FadeController : MonoBehaviour
	{
        [SerializeField] private Image fade;
        [SerializeField] private float fadeDuration = 1;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            GetComponent<Canvas>().worldCamera = Camera.main;
            Color secondaryColor = ColorManager.instance.chosenPalette.secondaryColor;
            fade.color = new Color(secondaryColor.r, secondaryColor.g, secondaryColor.b, 0);
        }

        private void OnEnable() => SceneManager.sceneLoaded += OnLoad;
        
        private void OnDisable() => SceneManager.sceneLoaded -= OnLoad;

        private void OnLoad(Scene scene, LoadSceneMode loadSceneMode)
        {
            Debug.Log($"Loaded Scene: {scene.name}");
            GetComponent<Canvas>().worldCamera = Camera.main;
            fade.color = ColorManager.instance.chosenPalette.secondaryColor;
            StartCoroutine(LoadSceneFadeOut());
        }

        public IEnumerator LoadSceneFadeIn(int sceneId)
        {
            Debug.Log("LoadSceneFadeIn");
            float time = 0;
            float startValue = fade.color.a;
            while (time < fadeDuration)
            {
                fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, Mathf.Lerp(startValue, 1, time / fadeDuration).RoundToNearest(1f/8f));
                time += Time.deltaTime;
                yield return null;
            }
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1);

            SceneManager.LoadScene(sceneId);
        }

        private IEnumerator LoadSceneFadeOut()
        {
            Debug.Log("LoadSceneFadeOut");
            float time = 0;
            float startValue = fade.color.a;
            while (time < fadeDuration)
            {
                fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, Mathf.Lerp(startValue, 0, time / fadeDuration).RoundToNearest(1f/8f));
                time += Time.deltaTime;
                yield return null;
            }
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 0);
        }
    }   
}