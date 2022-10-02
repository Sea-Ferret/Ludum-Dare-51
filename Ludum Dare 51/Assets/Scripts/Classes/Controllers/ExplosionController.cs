using UnityEngine;

namespace Murgn
{
    public class ExplosionController : MonoBehaviour
	{
        [SerializeField] private float explosionSpeed;
        [SerializeField] private Sprite[] sprites;
        
        private int currentSprite;
        private float explosionTimer;
        
        private SpriteRenderer spriteRenderer;
        private void Start()
        {
            spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            EventManager.SpriteSetPrimary?.Invoke(spriteRenderer);
            spriteRenderer.flipX = Utilities.RandomChance();
            
            explosionTimer = explosionSpeed + Time.time;
        }

        private void Update()
        {
            if (explosionTimer <= Time.time)
            {
                currentSprite++;
                explosionTimer = explosionSpeed + Time.time;
            }
            
            if(currentSprite >= sprites.Length) Destroy(gameObject);
            else spriteRenderer.sprite = sprites[currentSprite];
        }
    }   
}