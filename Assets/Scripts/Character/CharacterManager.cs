using UnityEngine;
using Unity.Netcode;

namespace Character
{
    public class CharacterManager : NetworkBehaviour
    {
        public CharacterController characterController;
        
        private CharacterNetworkManager _characterNetworkManager;
        
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);
            
            characterController = GetComponent<CharacterController>();
            _characterNetworkManager = GetComponent<CharacterNetworkManager>();
        }
        
        protected virtual void Update()
        {
            // If this is the owner of the character, update the network position
            if (IsOwner)
            {
                _characterNetworkManager.networkPosition.Value = transform.position;
                _characterNetworkManager.networkRotation.Value = transform.rotation;
            }
            // If this is not the owner of the character, update the position of the character
            else
            {
                // Smoothly move the character to the network position
                transform.position = Vector3.SmoothDamp(transform.position, 
                    _characterNetworkManager.networkPosition.Value, 
                    ref _characterNetworkManager.networkPositionVelocity, 
                    _characterNetworkManager.networkPositionSmoothTime);
                
                // Smoothly rotate the character to the network rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, 
                    _characterNetworkManager.networkRotation.Value, 
                    _characterNetworkManager.networkRotationSmoothTime);
            }
        }

        protected virtual void LateUpdate() {}
    }
}
