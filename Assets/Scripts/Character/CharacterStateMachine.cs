using State_Machines;
using UnityEngine;

namespace Character
{
    public class CharacterStateMachine : StateMachine
    {
        public CharacterController characterController;
        public CharacterNetworkManager characterNetworkManager;
        
        [field: Header("Movement Speeds")]
        [field: SerializeField] public float WalkingSpeed { get; private set; }
        [field: SerializeField] public float RunningSpeed { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }
        
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);
            characterController = GetComponent<CharacterController>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
        }

        protected virtual void FixedUpdate()
        {
            if(IsOwner)
            {
                characterNetworkManager.networkPosition.Value = transform.position;
                characterNetworkManager.networkRotation.Value = transform.rotation;
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, 
                    characterNetworkManager.networkPosition.Value, 
                    ref characterNetworkManager.networkPositionVelocity, 
                    characterNetworkManager.networkPositionSmoothTime);
                
                transform.rotation = Quaternion.Slerp(transform.rotation, 
                    characterNetworkManager.networkRotation.Value, 
                    characterNetworkManager.networkRotationSmoothTime);
            }
        }
    }
}