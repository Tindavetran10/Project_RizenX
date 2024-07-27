using State_Machines;
using UnityEngine;

namespace Character
{
    public class CharacterStateMachine : StateMachine
    {
        public CharacterController characterController;
        
        [field: Header("Movement Speeds")]
        [field: SerializeField] public float WalkingSpeed { get; private set; }
        [field: SerializeField] public float RunningSpeed { get; private set; }
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            characterController = GetComponent<CharacterController>();
        }
    }
}