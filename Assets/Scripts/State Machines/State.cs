using UnityEngine;

namespace State_Machines
{
    public abstract class State
    {
        public abstract void Awake();
        public abstract void Enter();
        public abstract void Tick(float deltaTime);
        public abstract void Exit();
    }
}