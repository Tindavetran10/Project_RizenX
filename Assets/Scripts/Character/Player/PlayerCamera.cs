using System;
using UnityEngine;

namespace Character.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance { get; private set; }

        public Camera cameraObject;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else Destroy(gameObject);
        }
        
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}