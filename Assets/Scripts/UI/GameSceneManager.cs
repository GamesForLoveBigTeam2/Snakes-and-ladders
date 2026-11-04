using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    
    public void Start()
    {
        pausePanel.SetActive(false);
    }
    
     public void OpenGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void OpenPausePanel()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }
    public void ClosePausePanel()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
