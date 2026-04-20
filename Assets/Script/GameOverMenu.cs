using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); 
    }
    public void ExitGame()
    {
        Debug.Log("Quit Game");
    }
}