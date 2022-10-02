using System;
using Microsoft.Win32.SafeHandles;
using Mono.Cecil.Cil;
using Murgn.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Murgn
{
    public class Manager : Singleton<Manager>
    {
        public GameStates gameState;
        [HideInInspector] public Camera mainCamera;
        [HideInInspector] public PlayerInput input;
        [HideInInspector] public PlayerController playerController;
        [HideInInspector] public float gameTimer;
        [HideInInspector] public int timerProgress;
        public int bombAmount = 1;
        [SerializeField] private GameObject deadPlayerPrefab;
        
        [SerializeField] private GameObject fadeCanvasPrefab;
        private FadeController fadeObject;
        
        private float timer;
        private float startingTimer;
        private bool started;
        private float endingTimer;
        [HideInInspector] public bool ended;

        private AudioManager audioManager;

        public override void Awake()
        {
            base.Awake();
            mainCamera = Camera.main;
            input = new PlayerInput();
            audioManager = AudioManager.instance;
        }

        private void Start()
        {
             // Add a 2 second delay to game start
             startingTimer = 2 + Time.time;
            
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


        private void OnEnable()
        {
            input.Enable();
            EventManager.PlayerDeath += OnPlayerDeath;
        }

        private void OnDisable()
        {
            input.Disable();
            EventManager.PlayerDeath -= OnPlayerDeath;
        }

        private void Update()
        {
            Cursor.visible = false;
            
            if (startingTimer <= Time.time && !started)
            {
                gameState = GameStates.Playing;
                EventManager.TimerMax?.Invoke();
                started = true;
            }

            if (gameState == GameStates.Playing)
            {
                if (timer <= Time.time)
                {
                    gameTimer++;
                    timerProgress++;
                    if(timerProgress == 10)
                    {
                        EventManager.TimerMax?.Invoke();
                        if(Utilities.RandomChance(75)) bombAmount++;
                        bombAmount = Mathf.Clamp(bombAmount, 1, 10);
                    }
                    if (timerProgress > 10) timerProgress = 1;
                
                    timer = 1 + Time.time;
                }
            }

            if (gameState == GameStates.Ending)
            {
                if (endingTimer <= Time.time && !ended)
                {
                    StartCoroutine(UIManager.instance.Fade());
                    ended = true;
                }
            }
        }

        private void OnPlayerDeath()
        {
            GameObject deadPlayer = Instantiate(deadPlayerPrefab, playerController.gameObject.transform.position, Quaternion.identity);
            EventManager.SpriteSetPrimary?.Invoke(deadPlayer.transform.GetChild(0).GetComponent<SpriteRenderer>());
            Destroy(playerController.gameObject);
            audioManager.Play("Player/Death", 0.1f);
            endingTimer = 1 + Time.time;
            gameState = GameStates.Ending;
        }
        
        public void LoadLevel(int levelID) => StartCoroutine(fadeObject.LoadSceneFadeIn(levelID));
        public void RestartLevel() => StartCoroutine(fadeObject.LoadSceneFadeIn(SceneManager.GetActiveScene().buildIndex));
        public void Quit() => Application.Quit();
    }
}