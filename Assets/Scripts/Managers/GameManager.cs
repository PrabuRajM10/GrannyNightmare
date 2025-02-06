using System;
using Gameplay;
using ObjectPooling;
using UnityEngine;
using AudioType = Gameplay.AudioType;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PoolManager poolManager;
        [SerializeField] GameAudios gameAudios;

        private void Awake()
        {
            SoundManager.Init(gameAudios , poolManager);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            SoundManager.PlaySound(AudioType.Bg ,default , true);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
