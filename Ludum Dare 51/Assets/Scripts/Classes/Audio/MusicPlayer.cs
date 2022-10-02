using UnityEngine;

namespace Murgn
{
    public class MusicPlayer : MonoBehaviour
	{
        private void Start() => DontDestroyOnLoad(gameObject);
    }   
}