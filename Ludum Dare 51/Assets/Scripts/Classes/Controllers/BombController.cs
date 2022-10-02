using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Murgn
{
    public class BombController : MonoBehaviour
    {
        public bool isHeld;
        public Vector2Int position;
        [SerializeField] private float bombSpeed;
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private float fadeInSpeed = 0.5f;
        
        private int currentSprite;
        private float bombTimer;
        
        private SpriteRenderer spriteRenderer;
        private void Start()
        {
            spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            EventManager.SpriteSetPrimary?.Invoke(spriteRenderer);
            spriteRenderer.flipX = Utilities.RandomChance();

            StartCoroutine(Fade());
                
            bombTimer = bombSpeed + Time.time + Random.Range(0, 0.5f);
            
        }

        private void Update()
        {
            if (isHeld) return;
            
            if (bombTimer <= Time.time)
            {
                currentSprite++;
                bombTimer = bombSpeed + Time.time + Random.Range(0, 0.5f);
            }
            
            if(currentSprite >= sprites.Length)
            {
                EventManager.DestroyTile?.Invoke(position);
                Destroy(gameObject);
            }
            else spriteRenderer.sprite = sprites[currentSprite];
        }
        
        public IEnumerator Fade(bool fadeIn = true)
        {
            float time = 0;
            Color color = spriteRenderer.color;
            while (time < fadeInSpeed)
            {
                spriteRenderer.color = new Color(color.r,color.g,color.b,Mathf.Lerp(fadeIn ? 0 : 1, fadeIn ? 1 : 0, time / fadeInSpeed).RoundToNearest(fadeInSpeed));
                time += Time.deltaTime;
                yield return null;
            }
            spriteRenderer.color = new Color(color.r, color.g, color.b, fadeIn ? 1 : 0);
        }
    }   
}