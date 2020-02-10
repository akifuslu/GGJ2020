using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game
{
    public class SceneLoader : Singleton<SceneLoader>
    {
        public void LoadNextScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void LoadLevel(int i)
        {
            SceneManager.LoadScene(i);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
