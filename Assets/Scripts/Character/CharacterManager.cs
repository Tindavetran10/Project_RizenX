using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Character
{
    public class CharacterManager : NetworkBehaviour
    {
        [Header("Status")]
        public NetworkVariable<bool> isDead = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        
        [HideInInspector] public CharacterNetworkManager characterNetworkManager;
        [HideInInspector] public CharacterEffectsManager characterEffectsManager;
        [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
        
        [Header("Flags")]
        public bool isPerformingAction;
        public bool isJumping;
        public bool isGrounded;
        public bool applyRootMotion;
        public bool canRotate = true;
        public bool canMove = true;
        
        private static readonly int IsGrounded = Animator.StringToHash("isGrounded");

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);
            
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        }
        
        protected virtual void Update()
        {
            animator.SetBool(IsGrounded, isGrounded);
            
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

        public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;
                
                // Reset any flags here that need to be reset
                
                // If we are not grounded, play ariel death animation

                if (!manuallySelectDeathAnimation)
                {
                    characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
                }
            }
            
            // Play some death sfx
            yield return new WaitForSeconds(1);
            
            // award players with runes
            
            // disable the character
        }
    }
}
