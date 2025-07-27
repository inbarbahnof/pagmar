using Audio.FMOD;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    public void ShowPauseMenu(bool show)
    {
        if (show)
            AudioManager.Instance.PauseMenuSnapshotStart();
        else
            AudioManager.Instance.ResumePauseMenu();


        pauseMenu.SetActive(show);
    }

    public void OnResume()
    {
        GameManager.instance.OnMenuButton();
    }

    public void OnReset()
    {
        GameManager.instance.RestartGame();
    }

    public void OnCredits()
    {
        AudioManager.Instance.StopAllSnapshots();
        AudioManager.Instance.StopMusic();
        ShowCredits();
    }

    public void ShowCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}