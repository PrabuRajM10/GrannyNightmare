using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace Gameplay
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Create GameAudios", fileName = "GameAudios", order = 0)]
    public class GameAudios : ScriptableObject
    {
        [SerializeField] List<AudioData> audioClips = new List<AudioData>();

        public (AudioClip clip, AudioMixerGroup mixer)? GetAudioData(AudioType audioType)
        {
            foreach (var audioClip in audioClips.Where(audioClip => audioClip.type == audioType))
            {
                return (audioClip.clip, audioClip.audioMixerGroup);
            }

            return null;
        }
    }

    [Serializable]
    public class AudioData
    {
        public AudioType type;
        public AudioClip clip;
        public AudioMixerGroup audioMixerGroup;
    }

    public enum AudioType
    {
        Bg,
        GunFire,
        BulletHit,
        EnemyLightAttack,
        EnemyHeavyAttack,
        EnemyJumpAttack,
        EnemyRangedAttack,
        EnemyDeath
    }
}