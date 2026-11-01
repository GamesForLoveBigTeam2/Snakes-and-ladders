using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPanel;
    
    public void Start()
    {
        playerPanel.SetActive(false);
    }
      public void OpenPlayer1()
    {
        SceneManager.LoadScene("1 Player");
    }
     public void OpenPlayer2()
    {
        SceneManager.LoadScene("2 Player");
    }
     public void OpenPlayer3()
    {
        SceneManager.LoadScene("3 Player");
    }
     public void OpenPlayer4()
    {
        SceneManager.LoadScene("4 Player");
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
