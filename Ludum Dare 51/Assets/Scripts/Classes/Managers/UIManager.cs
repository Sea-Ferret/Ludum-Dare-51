using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Murgn
{
    public class UIManager : MonoBehaviour
    {
        [Header("Timer Panel")]
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private Slider timerSlider;

        [Header("Bomb Panel")] 
        [SerializeField] private Image bombTextPanel;
        [SerializeField] private Image bombImage;

        private Manager manager;
        private PlayerController playerController;
        private void Start()
        {
            manager = Manager.instance;
            playerController = manager.playerController;
        }

        private void Update()
        {
            Timer();
            BombPanel();
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
    }   
}