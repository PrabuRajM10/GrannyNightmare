using System;
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
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif  
        }

        private void OnClickPlayButton()
        {
            uiManager.SwitchScreen(GameScreens.MenuScreen , GameScreens.GamePlayScreen);
        }
    }
}