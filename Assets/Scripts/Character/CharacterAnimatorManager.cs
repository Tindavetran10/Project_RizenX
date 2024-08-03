using System;
using UnityEngine;

namespace Character
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        private CharacterManager characterManager;
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");

        protected virtual void Awake()
        {
           characterManager = GetComponent<CharacterManager>(); 
        }

        public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue)
        {
            // Optional: Cache the character manager
            characterManager.animator.SetFloat(Horizontal, horizontalValue, 0.1f, Time.deltaTime);
            characterManager.animator.SetFloat(Vertical, verticalValue,0.1f, Time.deltaTime);
        }
    }
}
