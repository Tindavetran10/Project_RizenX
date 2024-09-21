using Unity.Netcode;
using UnityEngine;

namespace Character
{
    public class CharacterCombatManager : NetworkBehaviour
    {
        private CharacterManager _characterManager;
        
        [Header("Attack Target")]
        public CharacterManager currentTarget;
        
        [Header("Attack Type")]
        public AttackType currentAttackType;
        
        [Header("Lock On Transform")]
        public Transform lockOnTransform;

        protected virtual void Awake() => _characterManager = GetComponent<CharacterManager>();

        public virtual void SetTarget(CharacterManager newTarget)
        {
            if (_characterManager.IsOwner)
            {
                if(newTarget != null)
                {
                    currentTarget = newTarget;
                    
                    // Tell the network we have a target, and tell the network the target's network id
                    _characterManager.characterNetworkManager.currentTargetNetworkObjectID.Value = 
                        newTarget.GetComponent<NetworkObject>().NetworkObjectId;
                }
                else currentTarget = null;
            }
        }
    }
}