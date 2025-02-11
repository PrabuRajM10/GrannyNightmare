using System;
using Helper;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ui.Screens
{
    public class MenuScreen : MonoBehaviour
    {
        [SerializeField] UiManager uiManager;
        [SerializeField] private Button playButton;
        [SerializeField] private Button quitButton;
        

        private void OnEnable()
        {
            playButton.onClick.AddListener(OnClickPlayButton);
            quitButton.onClick.AddListener(OnClickQuitButton);
        }

        private void OnDisable()
        {
            playButton.onClick.RemoveListener(OnClickPlayButton);
            quitButton.onClick.RemoveListener(OnClickQuitButton);
        }

        private void OnClickQuitButton()
        {
            Utils.OnQuitButtonPressed(quitButton);
        }

        private void OnClickPlayButton()
        {
            Utils.ButtonOnClick(playButton , () =>
            {
                uiManager.SwitchScreen(GameScreens.MenuScreen , GameScreens.GamePlayScreen);
            });
        }
    }
}