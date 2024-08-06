using Character.Player.Player_States;
using UnityEngine;

namespace Character
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        private PlayerStateMachine _stateMachine;
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");

        protected virtual void Awake() => _stateMachine = GetComponent<PlayerStateMachine>();

        public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue)
        {
            _stateMachine.animator.SetFloat(Horizontal, horizontalValue, 0.1f, Time.deltaTime);
            _stateMachine.animator.SetFloat(Vertical, verticalValue, 0.1f, Time.deltaTime);
        }
    }
}