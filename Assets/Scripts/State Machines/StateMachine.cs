using Unity.Netcode;

namespace State_Machines
{
    public abstract class StateMachine : NetworkBehaviour
    {
        private State _currentState;
        
        public void SwitchState(State newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }

        private void Update() => _currentState?.Update();
    }
}