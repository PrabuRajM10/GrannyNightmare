using System;
using Helper;
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
            Utils.OnQuitButtonPressed(quitButton,true);
        }

        private void OnClickResumeButton()
        {
            Utils.ButtonOnClick(resumeButton , () =>
            {
                 uiManager.SwitchScreen(GameScreens.PauseScreen,GameScreens.GamePlayScreen);
            } , true);
        }
    }
}