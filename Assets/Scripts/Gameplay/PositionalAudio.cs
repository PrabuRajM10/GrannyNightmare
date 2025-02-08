using System;
using ObjectPooling;
using UnityEngine;
using UnityEngine.Audio;

namespace Gameplay
{
    public class PositionalAudio : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;
        PoolManager poolManager;
        Action onAudioPlayed;
        private void OnValidate()
        {
            if(audioSource == null) audioSource = GetComponent<AudioSource>();
        }

        public void Init(PoolManager poolManager)
        {
            this.poolManager = poolManager;
        }

        public void SetData(AudioClip clip, AudioMixerGroup mixerGroup, Vector3 position = default, bool loop = false , Action onAudioCompleted = null)
        {
            onAudioPlayed = onAudioCompleted;
            transform.position = position;
            audioSource.outputAudioMixerGroup = mixerGroup;
            audioSource.clip = clip;
            audioSource.loop = loop;
            gameObject.SetActive(true);
            audioSource.Play();
            if(!loop)Invoke(nameof(BackToPool) , audioSource.clip.length);
        }

        void BackToPool()
        {
            onAudioPlayed?.Invoke();
            poolManager.ReturnPoolObject(this);
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        public void Reset()
        {
            transform.position = Vector3.zero;
        }

        public void Resume()
        {
            if(!audioSource.isPlaying)audioSource.Play();
        }

        public void Stop()
        {
            if(audioSource.isPlaying)audioSource.Stop();
        }
    }
}