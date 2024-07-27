using System;
using UnityEngine;

namespace Character.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        public Camera cameraObject;
        public static PlayerCamera Instance { get; private set; }
        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            else Destroy(gameObject);
        }
        
        private void Start() => DontDestroyOnLoad(gameObject);
    }
}