using UnityEngine;

public class OpenScene : MonoBehaviour
{
    public void OpenLevel(string nameScene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(nameScene);
    }
    public void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
