using Effects;
using UnityEngine;
using Unity.Netcode;
using World_Manager;

namespace Character
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        private CharacterManager _characterManager;
        
        [Header("Position")]
        [HideInInspector] public NetworkVariable<Vector3> networkPosition = new(Vector3.zero, 
            NetworkVariableReadPermission.Everyone, 
            NetworkVariableWritePermission.Owner);
        
        [HideInInspector] public NetworkVariable<Quaternion> networkRotation = new(Quaternion.identity, 
            NetworkVariableReadPermission.Everyone, 
            NetworkVariableWritePermission.Owner);
        
        [HideInInspector] public Vector3 networkPositionVelocity;
        public float networkPositionSmoothTime = 0.1f;
        public float networkRotationSmoothTime = 0.1f;
    
        [Header("Animator")]
        public NetworkVariable<float> horizontalMovement = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> verticalMovement = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> moveAmount = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Target")]
        public NetworkVariable<ulong> currentTargetNetworkObjectID = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        [Header("Flags")]
        public NetworkVariable<bool> isLockedOn = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isSprinting = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isJumping = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        [Header("Stats")]
        public NetworkVariable<int> currentHealth = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> maxHealth = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        public NetworkVariable<float> currentStamina = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> maxStamina = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        [Header("Resources")]
        public NetworkVariable<int> endurance = new(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> vitality = new(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        protected virtual void Awake() => _characterManager = GetComponent<CharacterManager>();

        public void CheckHP(int oldValue, int newValue)
        {
            if (currentHealth.Value <= 0)
                StartCoroutine(_characterManager.ProcessDeathEvent());

            // Prevent us over healing
            if (_characterManager.IsOwner)
            {
                if(currentHealth.Value > maxHealth.Value)
                    currentHealth.Value = maxHealth.Value;
            }
        }

        public void OnLockTargetIDChange(ulong oldId, ulong nextId)
        {
            if (!IsOwner)
            {
                _characterManager.characterCombatManager.currentTarget = NetworkManager.Singleton.SpawnManager.SpawnedObjects[nextId]
                    .gameObject.GetComponent<CharacterManager>();
            }
        }

        public void OnIsLockedOnChange(bool old, bool isLockedOn)
        {
            if(!isLockedOn)
                _characterManager.characterCombatManager.currentTarget = null;
        }
        
        #region Action Animation
        // A server RPC is a method that is called on the server and executed on the clients
        [ServerRpc]
        public void NotifyTheServerOfActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            // If this is the server, play the action animation for all clients
            if (IsServer) PlayActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);
        }
        
        // A client RPC is a method that is called on the client and executed on the server
        [ClientRpc]
        private void PlayActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            // We make sure to not run the function on the character who sent it (so we don't play the animation twice)
            if (clientID != NetworkManager.Singleton.LocalClientId) PerformActionAnimationFromServer(animationID, applyRootMotion);
        }
        
        private void PerformActionAnimationFromServer(string animationID, bool applyRootMotion)
        {
            // Play the action animation on the server
            _characterManager.applyRootMotion = applyRootMotion;
            _characterManager.animator.CrossFade(animationID, 0.2f);
        }
        #endregion
        
        #region Attack Animation
        [ServerRpc]
        public void NotifyTheServerOfAttackActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            // If this is the server, play the action animation for all clients
            if (IsServer) PlayAttackActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);
        }
        
        [ClientRpc]
        private void PlayAttackActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            // We make sure to not run the function on the character who sent it (so we don't play the animation twice)
            if (clientID != NetworkManager.Singleton.LocalClientId) PerformAttackActionAnimationFromServer(animationID, applyRootMotion);
        }
        
        private void PerformAttackActionAnimationFromServer(string animationID, bool applyRootMotion)
        {
            // Play the action animation on the server
            _characterManager.applyRootMotion = applyRootMotion;
            _characterManager.animator.CrossFade(animationID, 0.2f);
        }
        #endregion

        #region Damage
        [ServerRpc(RequireOwnership = false)]
        public void NotifyTheServerOfCharacterDamageServerRpc(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float lightningDamage,
            float holyDamage,
            float poisonDamage,
            float angleHitFrom,
            float contactPointX,
            float contactPointY,
            float contactPointZ)
        {
            if (IsServer)
            {
                NotifyTheServerOfCharacterDamageClientRpc(damagedCharacterID, characterCausingDamageID, 
                    physicalDamage, magicDamage, fireDamage, lightningDamage, holyDamage, poisonDamage, 
                    angleHitFrom, contactPointX, contactPointY, contactPointZ);
            }
        }
        
        [ClientRpc]
        private void NotifyTheServerOfCharacterDamageClientRpc(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float lightningDamage,
            float holyDamage,
            float poisonDamage,
            float angleHitFrom,
            float contactPointX,
            float contactPointY,
            float contactPointZ)
        {
            ProcessCharacterDamageFromServer(damagedCharacterID, characterCausingDamageID, 
                physicalDamage, magicDamage, fireDamage, lightningDamage, holyDamage, poisonDamage, 
                angleHitFrom, contactPointX, contactPointY, contactPointZ);
        }
        
        private void ProcessCharacterDamageFromServer(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float lightningDamage,
            float holyDamage,
            float poisonDamage,
            float angleHitFrom,
            float contactPointX,
            float contactPointY,
            float contactPointZ)
        {
            var damagedCharacterManager = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damagedCharacterID]
                .gameObject.GetComponent<CharacterManager>();
            
            var characterCausingDamage = NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterCausingDamageID]
                .gameObject.GetComponent<CharacterManager>();
            
            var damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicalDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poisonDamage;
            damageEffect.angleHitFrom = angleHitFrom;
            damageEffect.contactPoint = new Vector3(contactPointX, contactPointY, contactPointZ);
            damageEffect.characterCausingDamage = characterCausingDamage;
            
            damagedCharacterManager.characterEffectsManager.ProcessInstantEffect(damageEffect); 
        }

        #endregion
    }
}