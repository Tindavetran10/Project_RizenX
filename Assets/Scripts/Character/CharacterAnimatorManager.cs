using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Character
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        private CharacterManager _characterManager;
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");

        [Header("Damage Animation")]
        public string lastDamageAnimationPlayed;
        
        [Header("Damage Animation")] 
        [SerializeField] private string hit_Forward_Medium_01 = "Hit Forward Medium 01";
        [SerializeField] private string hit_Forward_Medium_02 = "Hit Forward Medium 02";
        
        [SerializeField] private string hit_Backward_Medium_01 = "Hit Backward Medium 01";
        [SerializeField] private string hit_Backward_Medium_02 = "Hit Backward Medium 02";
        
        [SerializeField] private string hit_Left_Medium_01 = "Hit Left Medium 01";
        [SerializeField] private string hit_Left_Medium_02 = "Hit Left Medium 02";
        
        [SerializeField] private string hit_Right_Medium_01 = "Hit Right Medium 01";
        [SerializeField] private string hit_Right_Medium_02 = "Hit Right Medium 02";

        public List<string> ForwardMediumDamage = new();
        public List<string> BackwardMediumDamage = new();
        public List<string> LeftMediumDamage = new();
        public List<string> RightMediumDamage = new();
        
        protected virtual void Awake() => _characterManager = GetComponent<CharacterManager>();

        protected virtual void Start()
        {
            ForwardMediumDamage.Add(hit_Forward_Medium_01);
            ForwardMediumDamage.Add(hit_Forward_Medium_02);
            
            BackwardMediumDamage.Add(hit_Backward_Medium_01);
            BackwardMediumDamage.Add(hit_Backward_Medium_02);
            
            LeftMediumDamage.Add(hit_Left_Medium_01);
            LeftMediumDamage.Add(hit_Left_Medium_02);
            
            RightMediumDamage.Add(hit_Right_Medium_01);
            RightMediumDamage.Add(hit_Right_Medium_02);
        }

        public string GetRandomAnimationFromList(IEnumerable<string> animationList)
        {
            var finalList = animationList.ToList();

            // Check if we have already played this damage animation, so it doesn't play the same one twice
            finalList.Remove(lastDamageAnimationPlayed);

            // Check the list for null values and remove them
            for (var i = finalList.Count - 1; i > -1; i--)
            {
                if(finalList[i] == null)
                    finalList.RemoveAt(i);
            }
            
            var randomValue = Random.Range(0, finalList.Count);
            return finalList[randomValue];
        }
        
        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
        {
            var snappedHorizontal = horizontalMovement;
            var snappedVertical = verticalMovement;

            snappedHorizontal = horizontalMovement switch
            {
                > 0 and <= 0.5f => 0.5f,
                > 0.5f and <= 1 => 1,
                < 0 and >= -0.5f => -0.5f,
                < -0.5f and >= -1 => -1,
                _ => 0
            };

            snappedVertical = verticalMovement switch
            {
                > 0 and <= 0.5f => 0.5f,
                > 0.5f and <= 1 => 1,
                < 0 and >= -0.5f => -0.5f,
                < -0.5f and >= -1 => -1,
                _ => 0
            };

            if (isSprinting) snappedVertical = 2;
            
            const float dampTime = 0.075f;
            _characterManager.animator.SetFloat(Horizontal, snappedHorizontal, dampTime, Time.deltaTime);
            _characterManager.animator.SetFloat(Vertical, snappedVertical,dampTime, Time.deltaTime);

        }

        public virtual void PlayTargetActionAnimation(
            string targetAnimation, 
            bool isPerformingAction, 
            bool applyRootMotion = true, 
            bool canRotate = false, 
            bool canMove = false)
        {
            Debug.Log("Playing animation: " + targetAnimation);
            
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

        public virtual void PlayTargetAttackActionAnimation(
            AttackType attackType,
            string targetAnimation,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false)
        {
            // Keep track the last attack performed (for combos)
            // Keep track of the current attack type (light, heavy, critical)
            // Update the animation set to the current weapons animations
            // Decide if out attack can be parried or not
            // Tell the network our "isAttacking" flag is active (for counter damage, etc.)
            
            _characterManager.characterCombatManager.currentAttackType = attackType;
            _characterManager.applyRootMotion = applyRootMotion;
            _characterManager.animator.CrossFade(targetAnimation, 0.2f);
            
            // Can be used to prevent the character from performing other actions while performing an action
            _characterManager.isPerformingAction = isPerformingAction;
            _characterManager.canRotate = canRotate;
            _characterManager.canMove = canMove;
            
            // Tell the sever/host we played an animation, and to play it on the clients
            _characterManager.characterNetworkManager.NotifyTheServerOfAttackActionAnimationServerRpc(
                NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }
    }
}
