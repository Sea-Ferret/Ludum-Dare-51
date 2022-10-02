using System;
using System.Collections.Generic;
using Murgn.Audio;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Murgn
{
    public class LevelManager : Singleton<LevelManager>
    {
        [Header("Map")]
        public static int mapWidth = 9;
        public static int mapHeight = 6;
        public Tile[,] map = new Tile[mapWidth, mapHeight];
        public GameObject[,] bombMap = new GameObject[mapWidth, mapHeight];
        
        [Header("Settings")] 
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private GameObject bombPrefab;
        [SerializeField] private GameObject explosionPrefab;
        [SerializeField] private GameObject deadBombPrefab;
        public Vector2Int mapOffset;
        private Vector2 currentPos;

        [Header("Player")] 
        private Vector2Int oldPos = new (-1, -1);

        private Manager manager;
        private AudioManager audioManager;

        private void OnEnable()
        {
            EventManager.PlayerMove += OnPlayerMove;
            EventManager.TimerMax += OnTimerMax;
            EventManager.DestroyTile += OnDestroyTile;
        }

        private void OnDisable()
        {
            EventManager.PlayerMove -= OnPlayerMove;
            EventManager.TimerMax -= OnTimerMax;
            EventManager.DestroyTile -= OnDestroyTile;
        }

        private void Awake()
        {
            GameObject mapParent = new GameObject("Map");
            
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    GameObject tile = Instantiate(tilePrefab, new Vector2(x, y) - mapOffset, Quaternion.identity, mapParent.transform);
                    map[x, y] = new Tile()
                    {
                        tileController = tile.GetComponent<TileController>(),
                        spriteRenderer = tile.transform.GetChild(0).GetComponent<SpriteRenderer>(),
                        enabled = true
                    };
                }
            }
        }

        private void Start()
        {
            audioManager = AudioManager.instance;
            manager = Manager.instance;
        }

        private void OnPlayerMove(Vector2Int currentPos)
        {
            if(oldPos != new Vector2Int(-1, -1)) map[oldPos.x, oldPos.y].spriteRenderer.enabled = true;
            map[currentPos.x, currentPos.y].spriteRenderer.enabled = false;
            oldPos = currentPos;
        }

        private void OnTimerMax()
        {
            List<Vector2Int> activeTiles = new();
            
            // Add enabled tiles to list
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if(map[x, y].enabled && !map[x, y].tileController.hasBomb)
                        activeTiles.Add(new Vector2Int(x, y));
                }
            }
            
            // Randomly pick several tiles from list
            for (int i = 0; i < manager.bombAmount + (Utilities.RandomChance(50) ? 1 : 0); i++)
            {
                if (activeTiles.Count == 0) return;
                int rand = Random.Range(0, activeTiles.Count);
                map[activeTiles[rand].x, activeTiles[rand].y].tileController.hasBomb = true;
                bombMap[activeTiles[rand].x, activeTiles[rand].y] = Instantiate(bombPrefab, new Vector2(activeTiles[rand].x - mapOffset.x, activeTiles[rand].y - mapOffset.y), Quaternion.identity);
                bombMap[activeTiles[rand].x, activeTiles[rand].y].GetComponent<BombController>().position = activeTiles[rand];
            }
        }

        public bool IsBomb(Vector2Int pos)
        {
            if (!IsWithinMap(pos + mapOffset)) return false;
            
            Vector2Int newPos = pos + mapOffset;
            return bombMap[newPos.x, newPos.y] != null;
        }
        
        public bool IsTile(Vector2Int position)
        {
            if (!IsWithinMap(position + mapOffset)) return false;
            
            Vector2Int newPos = position + mapOffset;
            return map[newPos.x, newPos.y].enabled;
        }
        
        public bool IsWithinMap(Vector2Int position) => 0 <= position.x && position.x <= mapWidth - 1 
                                                                        && 0 <= position.y && position.y <= mapHeight - 1;

        private void OnDestroyTile(Vector2Int position)
        {
            if (!IsWithinMap(position)) return;
            
            map[position.x, position.y].tileController.gameObject.SetActive(false);
            map[position.x, position.y].enabled = false;
            Instantiate(explosionPrefab, new Vector2(position.x - mapOffset.x, position.y - mapOffset.y), Quaternion.identity);
            audioManager.Play("Items/Explosion", 0.1f);
        }

        public GameObject TakeBomb(Vector2Int position)
        {
            Vector2Int newPos = position + mapOffset;
            
            if (!IsWithinMap(newPos)) return null;

            GameObject bomb = bombMap[newPos.x, newPos.y].gameObject;
            bomb.SetActive(false);
            bombMap[newPos.x, newPos.y] = null;
            map[newPos.x, newPos.y].tileController.hasBomb = false;
            
            audioManager.Play("UI/Select", 0.1f);

            return bomb;
        }
        
        public void SetBomb(Vector2Int position, GameObject bomb)
        {
            Vector2Int newPos = position + mapOffset;

            if(IsTile(position))
            {
                bombMap[newPos.x, newPos.y] = bomb;
                bombMap[newPos.x, newPos.y].GetComponent<BombController>().position = newPos;
                map[newPos.x, newPos.y].tileController.hasBomb = true;
                bomb.transform.position = new Vector2(position.x, position.y);
                bomb.gameObject.SetActive(true);
            }
            else
            {
                GameObject deadItem = Instantiate(deadBombPrefab, new Vector2(position.x, position.y), Quaternion.identity);
                EventManager.SpriteSetPrimary?.Invoke(deadItem.transform.GetChild(0).GetComponent<SpriteRenderer>());
                Destroy(bomb);
            }
            
            audioManager.Play("UI/Select", 0.1f);
        }
    }   
}