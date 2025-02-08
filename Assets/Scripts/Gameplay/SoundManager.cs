using System;
using System.Collections.Generic;
using ObjectPooling;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Gameplay
{
    public static class SoundManager
    {
        private static GameAudios gameAudios;
        private static PoolManager poolManager;

        static Dictionary<AudioType , PositionalAudio> loopableAudios = new Dictionary<AudioType , PositionalAudio>();

        public static void Init(GameAudios gameAudiosSo , PoolManager poolManagerSo)  
        {
            gameAudios = gameAudiosSo;
            poolManager = poolManagerSo;
        }

        public static void PlaySound(AudioType audioType, Vector3 position = default, bool loop = false , Action callback = null)
        {
            if (loop && loopableAudios.TryGetValue(audioType, out var audio))
            {
                audio = loopableAudios[audioType];  
                audio.Resume();
                return;
            }
            audio = poolManager.GetPoolObject<PositionalAudio>();
           
            var audioData = gameAudios.GetAudioData(audioType);
            if (audioData != null)
                audio.SetData(audioData.Value.clip, audioData.Value.mixer, position, loop , callback);
            else
            {
                Debug.LogError("Audio data Empty for type " + audioType);
            }
            if(loop)loopableAudios.Add(audioType , audio);
        }

        public static void StopAudio(AudioType audioType)
        {
            if (!loopableAudios.ContainsKey(audioType))
            {
                Debug.LogError("[PauseAudio] given type doesnt have a loopable audio " + audioType);
                return;
            }
            loopableAudios[audioType].Stop();
        }
    }
}