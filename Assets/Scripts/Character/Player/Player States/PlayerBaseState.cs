using State_Machines;
using UnityEngine;

namespace Character.Player.Player_States
{
    public abstract class PlayerBaseState : State
    {
        protected readonly PlayerStateMachine StateMachine;
        protected PlayerBaseState(PlayerStateMachine stateMachine) => StateMachine = stateMachine;
    }
}