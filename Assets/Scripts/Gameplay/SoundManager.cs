using System;
using ObjectPooling;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Gameplay
{
    public static class SoundManager
    {
        private static GameAudios gameAudios;
        private static PoolManager poolManager;

        public static void Init(GameAudios gameAudiosSo , PoolManager poolManagerSo)  
        {
            gameAudios = gameAudiosSo;
            poolManager = poolManagerSo;
        }

        public static void PlaySound(AudioType audioType, Vector3 position = default, bool loop = false)
        {
            var audio = poolManager.GetPoolObject<PositionalAudio>();
            var audioData = gameAudios.GetAudioData(audioType);
            if (audioData != null)
                audio.SetData(audioData.Value.clip, audioData.Value.mixer, position, loop);
            else
            {
                Debug.LogError("Audio data Empty for type " + audioType);
            }
        }
    }
}