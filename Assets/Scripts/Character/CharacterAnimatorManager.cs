using Unity.Netcode;
using UnityEngine;

namespace Character
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        private CharacterManager _characterManager;
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");

        protected virtual void Awake() => _characterManager = GetComponent<CharacterManager>();

       

        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
        {
            var horizontalAmount = horizontalMovement;
            var verticalAmount = verticalMovement;
            
            if (isSprinting) verticalAmount = 2;
            
            const float dampTime = 0.075f;
            _characterManager.animator.SetFloat(Horizontal, horizontalAmount, dampTime, Time.deltaTime);
            _characterManager.animator.SetFloat(Vertical, verticalAmount,dampTime, Time.deltaTime);

        }

        public virtual void PlayTargetActionAnimation(
            string targetAnimation, 
            bool isPerformingAction, 
            bool applyRootMotion = true, 
            bool canRotate = false, 
            bool canMove = false)
        {
            _characterManager.applyRootMotion = applyRootMotion;
            _characterManager.animator.CrossFade(targetAnimation, 0.2f);
            
            // Can be used to prevent the character from performing other actions while performing an action
            _characterManager.isPerformingAction = isPerformingAction;
            _characterManager.canRotate = canRotate;
            _characterManager.canMove = canMove;
            
            // Tell the sever/host we played an animation, and to play it on the clients
            _characterManager.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(
                NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }
    }
}
