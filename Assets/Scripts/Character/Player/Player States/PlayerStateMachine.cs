using State_Machines;
using UnityEngine;

namespace Character.Player.Player_States
{
    public class PlayerStateMachine : StateMachine
    {
        public CharacterController characterController;
        
        [field: SerializeField] public float WalkingSpeed { get; private set; }
        [field: SerializeField] public float RunningSpeed { get; private set; }
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            characterController = GetComponent<CharacterController>();
        }
        
        private void Start() => SwitchState(new PlayerIdleState(this));
    }
}