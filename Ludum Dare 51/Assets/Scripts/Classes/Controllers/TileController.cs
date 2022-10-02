using System;
using UnityEngine;
using UnityEngine.U2D;

namespace Murgn
{
    public class TileController : MonoBehaviour
    {
        [SerializeField] private Sprite[] sprites;
        [HideInInspector] public bool hasBomb;
        private SpriteRenderer spriteRenderer;
        private void Start()
        {
            spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            EventManager.SpriteSetPrimary?.Invoke(spriteRenderer);
            spriteRenderer.flipX = Utilities.RandomChance();
        }

        private void Update() => spriteRenderer.sprite = hasBomb ? sprites[1] : sprites[0];
    }   
}