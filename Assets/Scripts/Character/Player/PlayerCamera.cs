using Character.Player.Player_States;
using UnityEngine;

namespace Character.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance { get; private set; }
        public PlayerStateMachine PlayerStateMachine { get; set; }
        public Camera cameraObject;

        [Header("Camera Settings")] 
        private Vector3 _cameraVelocity;

        // The bigger the value, the slower the camera will follow the player
        private const float CameraSmoothSpeed = 0.1f;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            else Destroy(gameObject);
        }
        
        private void Start() => DontDestroyOnLoad(gameObject);

        public void HandleAllCameraActions()
        {
            // Follow the player
            // Rotate the camera around the player
            // Collision detection
            
            if(PlayerStateMachine != null)
            {
                FollowTarget();
            }
        }

        private void FollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(
                transform.position, 
                PlayerStateMachine.transform.position, 
                ref _cameraVelocity, 
                CameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }
    }
}