using Menu;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Pong.Scripts {
    public class PacmanManager : MonoBehaviour {
        
       
        [SerializeField] private GameObject uiPanel;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button quitButton;

        private void OnEnable() {
            continueButton.onClick.AddListener(OnClickContinueButton);
            quitButton.onClick.AddListener(OnClickQuitButton);
        }

        private void OnDisable() {
            continueButton.onClick.RemoveListener(OnClickContinueButton);
            quitButton.onClick.RemoveListener(OnClickQuitButton);
        }

        private void Update() {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            OpenPauseMenu();
        }


        private void OpenPauseMenu() {
            uiPanel.SetActive(true);
            Time.timeScale = 0f;
        }

        private void OnClickContinueButton() {
            uiPanel.SetActive(false);
            Time.timeScale = 1f;
        }

        private void OnClickQuitButton() {
            Time.timeScale = 1;
            SceneManager.LoadScene((int)Scenes.FinalMenu);
        }
        


    }
}