using UnityEngine;

namespace Character
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        private CharacterManager _characterManager;
        private float _vertical;
        private float _horizontal;
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");

        protected virtual void Awake() => _characterManager = GetComponent<CharacterManager>();
        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement)
        {
            const float dampTime = 0.075f;
            _characterManager.animator.SetFloat(Horizontal, horizontalMovement, dampTime, Time.deltaTime);
            _characterManager.animator.SetFloat(Vertical, verticalMovement,dampTime, Time.deltaTime);
        }

        public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction, 
            bool applyRootMotion = false, bool canRotate = false, bool canMove = false)
        {
            _characterManager.applyRootMotion = applyRootMotion;
            _characterManager.animator.CrossFade(targetAnimation, 0.2f);
            
            // Can be used to prevent the character from performing other actions while performing an action
            _characterManager.isPerformingAction = isPerformingAction;
            _characterManager.canRotate = canRotate;
            _characterManager.canMove = canMove;
        }
    }
}
