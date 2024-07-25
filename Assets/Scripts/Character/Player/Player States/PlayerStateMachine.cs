using State_Machines;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character.Player.Player_States
{
    public class PlayerStateMachine : StateMachine
    {
        public CharacterController characterController;
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            characterController = GetComponent<CharacterController>();
        }
        
        
    }
}