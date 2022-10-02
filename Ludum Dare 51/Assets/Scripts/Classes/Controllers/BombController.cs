using UnityEngine;

namespace Murgn
{
    public class BombController : MonoBehaviour
	{
        private SpriteRenderer spriteRenderer;
        private void Start()
        {
            spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            EventManager.SpriteSetPrimary?.Invoke(spriteRenderer);
            spriteRenderer.flipX = Utilities.RandomChance();
        }
    }   
}