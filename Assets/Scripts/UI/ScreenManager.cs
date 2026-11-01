using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPanel;
    
    public void Start()
    {
        playerPanel.SetActive(false);
    }
  
     public void OpenGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void OpenPlayerPanel()
    {
        playerPanel.SetActive(true);
    }
    public void OpenPausePanel()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }

}
