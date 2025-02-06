using ObjectPooling;
using UnityEngine;
using UnityEngine.Audio;

namespace Gameplay
{
    public class PositionalAudio : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;
        PoolManager poolManager;
        private void OnValidate()
        {
            if(audioSource == null) audioSource = GetComponent<AudioSource>();
        }

        public void Init(PoolManager poolManager)
        {
            this.poolManager = poolManager;
        }

        public void SetData(AudioClip clip, AudioMixerGroup mixerGroup, Vector3 position = default, bool loop = false)
        {
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
    }
}