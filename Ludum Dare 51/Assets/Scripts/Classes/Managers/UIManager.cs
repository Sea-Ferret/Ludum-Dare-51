using System;
using System.Collections;
using Mono.Cecil.Cil;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Murgn
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private Image fadeOutPanel;
        
        [Header("Timer Panel")]
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private Slider timerSlider;

        [Header("Bomb Panel")] 
        [SerializeField] private Image bombTextPanel;
        [SerializeField] private Image bombImage;

        [Header("Game Over Panel")] 
        [SerializeField] private Image gameOverPanel;
        [SerializeField] private TextMeshProUGUI gameOverTimer;
        private float endingTimer;
        
        private float startingTimer;
        private bool fadedOut;
        private Manager manager;
        private PlayerController playerController;
        
        private void Start()
        {
            manager = Manager.instance;
            playerController = manager.playerController;
            startingTimer = 1 + Time.time;
        }

        private void Update()
        {
            if (startingTimer <= Time.time && !fadedOut)
            {
                StartCoroutine(Fade(false));
                fadedOut = true;
            }
            
            Timer();
            BombPanel();
            GameOverPanel();
        }

        private void Timer()
        {
            TimeSpan time = TimeSpan.FromSeconds(manager.gameTimer);
            timerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", 
                time.Hours, 
                time.Minutes, 
                time.Seconds);
            timerSlider.value = manager.timerProgress;
        }

        private void BombPanel()
        {
            bombTextPanel.gameObject.SetActive(playerController.hasBomb);
            bombImage.gameObject.SetActive(playerController.hasBomb);
        }
        
        public IEnumerator Fade(bool fadeIn = true)
        {
            float time = 0;
            Color color = fadeOutPanel.color;
            while (time < 1)
            {
                fadeOutPanel.color = new Color(color.r,color.g,color.b,Mathf.Lerp(fadeIn ? 0 : 1, fadeIn ? 1 : 0, time / 1).RoundToNearest(1f/4f));
                time += Time.deltaTime;
                yield return null;
            }
            fadeOutPanel.color = new Color(color.r, color.g, color.b, fadeIn ? 1 : 0);
        }

        private void GameOverPanel()
        {
            if(!manager.ended) endingTimer = 1.1f + Time.time;
            
            if (endingTimer <= Time.time)
            {
                gameOverPanel.gameObject.SetActive(manager.gameState == GameStates.Ending);
                TimeSpan time = TimeSpan.FromSeconds(manager.gameTimer);
                gameOverTimer.text = string.Format("{0:D2}:{1:D2}:{2:D2}", 
                    time.Hours, 
                    time.Minutes, 
                    time.Seconds);
            }
        }
    }   
}