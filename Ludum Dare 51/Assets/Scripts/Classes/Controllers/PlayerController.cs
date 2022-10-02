using System;
using UnityEngine;

namespace Murgn
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveTimer = 0.1f;
        private float moveCounter;
        
        [HideInInspector] public Vector2Int offsetedPosition;
        private PlayerFace playerFace;
        
        public bool hasBomb;
        
        private Manager manager;
        private LevelManager levelManager;
        private PlayerInput input;
        private Animator animator;
        
        private void Start()
        {
            manager = Manager.instance;
            levelManager = LevelManager.instance;
            input = manager.input;
            animator = GetComponent<Animator>();

            manager.playerController = this;
            PlayerMove();
        }

        private void Update()
        {
            if(moveCounter <= Time.time)
                Movement();
            
            Animation();
        }

        private void Movement()
        {
            if (input.Player.Up.WasPerformedThisFrame())
            {
                Vector2Int newPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)) + Vector2Int.up;
                if (!levelManager.IsBomb(newPos))
                {
                    transform.position += (Vector3)Vector2.up;
                    PlayerMove();
                
                    moveCounter = moveTimer + Time.time;
                }
                playerFace = PlayerFace.Back;
            }
            else if (input.Player.Right.WasPerformedThisFrame())
            {
                Vector2Int newPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)) + Vector2Int.right;
                if (!levelManager.IsBomb(newPos))
                {
                    transform.position += (Vector3)Vector2.right;
                    PlayerMove();
                
                    moveCounter = moveTimer + Time.time;
                }
                playerFace = PlayerFace.Right;
            }
            else if (input.Player.Down.WasPerformedThisFrame())
            {
                Vector2Int newPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)) + Vector2Int.down;
                if (!levelManager.IsBomb(newPos))
                {
                    transform.position += (Vector3)Vector2.down;
                    PlayerMove();

                    moveCounter = moveTimer + Time.time;
                }
                playerFace = PlayerFace.Front;
            }
            else if (input.Player.Left.WasPerformedThisFrame())
            {
                Vector2Int newPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)) + Vector2Int.left;
                if (!levelManager.IsBomb(newPos))
                {
                    transform.position += (Vector3)Vector2.left;
                    PlayerMove();

                    moveCounter = moveTimer + Time.time;
                }
                playerFace = PlayerFace.Left;
            }

        }

        private void PlayerMove()
        {
            offsetedPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)) + levelManager.mapOffset;
            transform.position = new Vector3(Mathf.Clamp(offsetedPosition.x, 0, LevelManager.mapWidth - 1),
                Mathf.Clamp(offsetedPosition.y, 0, LevelManager.mapHeight - 1)) - new Vector3(levelManager.mapOffset.x, levelManager.mapOffset.y);

            EventManager.PlayerMove?.Invoke(new Vector2Int(Mathf.RoundToInt(transform.position.x),
                Mathf.RoundToInt(transform.position.y)) + levelManager.mapOffset);
        }

        private void Animation()
        => animator.CrossFade(hasBomb ? playerFace.ToString() + "Bomb" : playerFace.ToString(), 0.0f);
    }   
}