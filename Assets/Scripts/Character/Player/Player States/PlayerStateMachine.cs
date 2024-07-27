using System;

namespace Character.Player.Player_States
{
    public class PlayerStateMachine : CharacterStateMachine
    {
        private void Start() => SwitchState(new PlayerIdleState(this));
    }
}