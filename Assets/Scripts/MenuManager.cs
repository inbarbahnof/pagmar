using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject endCredits;
        
        public void ShowPauseMenu(bool show)
        {
                pauseMenu.SetActive(show);
        }
        
        public void OnResume()
        {
                GameManager.instance.OnMenuButton();
        }

        public void OnReset()
        {
                SceneManager.LoadScene(0);
        }

        public void OnCredits()
        {
                ShowCredits();
        }

        public void ShowCredits()
        {
                endCredits.SetActive(true);
        }
}