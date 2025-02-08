using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Screens
{
    public class GameResultScreen : MonoBehaviour
    {
        [SerializeField] UiManager uiManager;
        [SerializeField] private TMP_Text result;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button quitButton; 
        
        [SerializeField] string gameWonMessage = "You won!";
        [SerializeField] string gameLostMessage = "Loser HAHAHA";
        
        private void OnEnable()
        {
            retryButton.onClick.AddListener(OnClickRetryButton);
            quitButton.onClick.AddListener(OnClickQuitButton);
            result.text = uiManager.IsGameWon ? gameWonMessage : gameLostMessage;
        }

        private void OnDisable()
        {
            retryButton.onClick.RemoveListener(OnClickRetryButton);
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

        private void OnClickRetryButton()
        {
            uiManager.SwitchScreen(GameScreens.GameResult , GameScreens.GamePlayScreen);
        }
    }
}