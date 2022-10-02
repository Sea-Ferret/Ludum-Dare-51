using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Murgn
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject fadeCanvasPrefab;
        private FadeController fadeObject;
        
        private void Start()
        {
            GameObject fadeCanvas = GameObject.Find("Fade Canvas");
            if (fadeCanvas == null)
            {
                fadeCanvas = Instantiate(fadeCanvasPrefab);
                fadeCanvas.name = "Fade Canvas";
                fadeObject = fadeCanvas.GetComponent<FadeController>();
            }
            else
                fadeObject = fadeCanvas.GetComponent<FadeController>();
        }

        private void Update()
        {
            Cursor.visible = false;
        }

        public void LoadLevel(int levelId)
            => StartCoroutine(fadeObject.LoadSceneFadeIn(levelId));

        public void LoadNextLevel()
            => StartCoroutine(fadeObject.LoadSceneFadeIn(SceneManager.GetActiveScene().buildIndex + 1));

        public void Quit()
            => Application.Quit();
    }   
}