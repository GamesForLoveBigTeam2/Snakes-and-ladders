using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPanel;

    private void Start()
    {
        playerPanel.SetActive(false);

        AudioManager.Instance?.PlayMusic(SoundType.MenuMusic);
    }

    public void OpenGame()
    {
        SceneManager.LoadScene("Game");

        AudioManager.Instance?.StopMusic();
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
