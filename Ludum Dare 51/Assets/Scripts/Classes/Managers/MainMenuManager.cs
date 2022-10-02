using UnityEngine;
using UnityEngine.SceneManagement;

namespace Murgn
{
    public class MainMenuManager : MonoBehaviour
    {
        public void LoadLevel(int levelId)
            => SceneManager.LoadScene(levelId);

        public void LoadNextLevel()
            => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        public void Quit()
            => Application.Quit();
    }   
}