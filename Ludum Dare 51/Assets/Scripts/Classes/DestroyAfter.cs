using UnityEngine;

namespace Murgn
{
    public class DestroyAfter : MonoBehaviour
    {
        [SerializeField] private float destroyAfter;
        private float timer;
        private void Start() => timer = destroyAfter + Time.time;

        private void Update()
        {
            if(timer <= Time.time) Destroy(gameObject);
        }
    }   
}