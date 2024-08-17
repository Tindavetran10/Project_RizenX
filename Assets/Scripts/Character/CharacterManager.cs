using Unity.Netcode;
using UnityEngine;

namespace Character
{
    public class CharacterManager : NetworkBehaviour
    {
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        
        [HideInInspector] public CharacterNetworkManager characterNetworkManager;
        
        [Header("Flags")]
        public bool isPerformingAction;
        public bool applyRootMotion;
        public bool canRotate = true;
        public bool canMove = true;
        
        
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);
            
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
        }
        
        protected virtual void Update()
        {
            // If this is the owner of the character, update the network position
            if (IsOwner)
            {
                characterNetworkManager.networkPosition.Value = transform.position;
                characterNetworkManager.networkRotation.Value = transform.rotation;
            }
            // If this is not the owner of the character, update the position of the character
            else
            {
                // Smoothly move the character to the network position
                transform.position = Vector3.SmoothDamp(transform.position, 
                    characterNetworkManager.networkPosition.Value, 
                    ref characterNetworkManager.networkPositionVelocity, 
                    characterNetworkManager.networkPositionSmoothTime);
                
                // Smoothly rotate the character to the network rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, 
                    characterNetworkManager.networkRotation.Value, 
                    characterNetworkManager.networkRotationSmoothTime);
            }
        }

        protected virtual void LateUpdate() {}
        
        
    }
}
