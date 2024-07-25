using System;
using State_Machines;
using UnityEngine;

namespace Character.Player.Player_States
{
    public class PlayerStateMachine : StateMachine
    {
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}