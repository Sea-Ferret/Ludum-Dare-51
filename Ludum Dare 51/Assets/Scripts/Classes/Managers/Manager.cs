using System;
using Mono.Cecil.Cil;
using UnityEngine;

namespace Murgn
{
    public class Manager : Singleton<Manager>
    {
        [HideInInspector] public Camera mainCamera;
        [HideInInspector] public PlayerInput input;
        [HideInInspector] public PlayerController playerController;
        [HideInInspector] public float gameTimer;
        private float timer;
        [HideInInspector] public int timerProgress;
        [HideInInspector] public int bombAmount = 3; 

        public override void Awake()
        {
            base.Awake();
            mainCamera = Camera.main;
            input = new PlayerInput();
        }

        private void Start() => timer = 1 + Time.time;

        private void OnEnable()
        {
            input.Enable();
        }

        private void OnDisable()
        {
            input.Disable();
        }

        private void Update()
        {
            gameTimer += Time.deltaTime;

            if (timer <= Time.time)
            {
                timerProgress++;
                if(timerProgress == 10) EventManager.TimerMax?.Invoke();
                if (timerProgress > 10) timerProgress = 1;
                
                timer = 1 + Time.time;
            }
        }
    }
}