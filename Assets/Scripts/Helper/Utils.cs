using System;
using Gameplay;
using State_Machine.EnemyStateMachine;
using State_Machine.PlayerStateMachine.PlayerStates;
using UnityEngine;
using UnityEngine.UI;
using AudioType = Gameplay.AudioType;

namespace Helper
{
    public static class Utils
    {
        public static PlayerStateMachine GetPlayerIfInOverlap(Vector3 position, float overlapRadius)
        {
            var hitColliders = Physics.OverlapSphere(position, overlapRadius);

            foreach (var hitCollider in hitColliders)
            {
                var playerObj = hitCollider.gameObject.GetComponent<PlayerStateMachine>();
                if (playerObj == null || playerObj.GetHealth() <= 0) continue;
                
                var playerObjTransform = playerObj.transform;
                var dirVect = (playerObjTransform.position - position).normalized;

                // if ((!(Vector3.Angle(stateMachine.transform.forward, dirVect) < viewAngle / 2))) continue;
                var newPos = new Vector3(position.x, position.y + 0.5f,position.z);
         
                if (!Physics.Raycast(newPos, dirVect, overlapRadius)) continue;
                // Debug.DrawLine(newPos, playerObjTransform.position, Color.yellow, 1000);
                // Debug.Log("Player got caught");
                return playerObj;
            }
            return null;
        }
        
        public static void ButtonOnClick(Button button , Action callBack, bool ignoreTimeScale = false)
        {
            button.interactable = false;
            SoundManager.PlaySound(AudioType.ButtonClick);
            var buttonTrans = button.transform;
            var initialScale = buttonTrans.localScale;
            buttonTrans.localScale = initialScale / 1.2f;
            buttonTrans.LeanScale(initialScale, 0.25f).setEaseInOutBack().setOnComplete(() =>
            {
                callBack?.Invoke();
                button.interactable = true;
            }).setIgnoreTimeScale(ignoreTimeScale);
        }


        public static void OnQuitButtonPressed(Button quitButton, bool b = false)
        {
            ButtonOnClick(quitButton , () =>
            {
                
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif  
            } , b);
        }
    }
}
