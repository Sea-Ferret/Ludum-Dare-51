using System;
using System.Collections.Generic;
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
        public Vector2Int mapOffset;
        private Vector2 currentPos;

        [Header("Player")] 
        private Vector2Int oldPos = new (-1, -1);

        private Manager manager;

        private void OnEnable()
        {
            EventManager.PlayerMove += OnPlayerMove;
            EventManager.TimerMax += OnTimerMax;
        }

        private void OnDisable()
        {
            EventManager.PlayerMove -= OnPlayerMove;
            EventManager.TimerMax -= OnTimerMax;
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

        private void Start() => manager = Manager.instance;

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
            for (int i = 0; i < manager.bombAmount; i++)
            {
                if (activeTiles.Count == 0) return;
                int rand = Random.Range(0, activeTiles.Count);
                map[activeTiles[rand].x, activeTiles[rand].y].tileController.hasBomb = true;
                bombMap[activeTiles[rand].x, activeTiles[rand].y] = Instantiate(bombPrefab, new Vector2(activeTiles[rand].x - mapOffset.x, activeTiles[rand].y - mapOffset.y), Quaternion.identity);
            }
        }

        public bool IsBomb(Vector2Int pos)
        {
            if (!IsWithinMap(pos + mapOffset)) return false;
            
            Vector2Int newPos = pos + mapOffset;
            return bombMap[newPos.x, newPos.y] != null;
        }
        
        public bool IsWithinMap(Vector2Int position) => 0 <= position.x && position.x <= mapWidth - 1 
                                                                        && 0 <= position.y && position.y <= mapHeight - 1;
    }   
}