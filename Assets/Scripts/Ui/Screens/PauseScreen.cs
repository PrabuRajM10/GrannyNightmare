using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Screens
{
    public class PauseScreen : MonoBehaviour
    {
        [SerializeField] UiManager uiManager;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button quitButton;
        
        private void OnEnable()
        {
            resumeButton.onClick.AddListener(OnClickResumeButton);
            quitButton.onClick.AddListener(OnClickQuitButton);
        }

        private void OnDisable()
        {
            resumeButton.onClick.RemoveListener(OnClickResumeButton);
            quitButton.onClick.RemoveListener(OnClickQuitButton);
        }

        private void OnClickQuitButton()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif  
        }

        private void OnClickResumeButton()
        {
            uiManager.SwitchScreen(GameScreens.PauseScreen,GameScreens.GamePlayScreen);
        }
    }
}