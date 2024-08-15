using UnityEngine;
using Unity.Netcode;

namespace Character
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        private CharacterManager _characterManager;
        
        [Header("Position")]
        public NetworkVariable<Vector3> networkPosition = new(Vector3.zero, 
            NetworkVariableReadPermission.Everyone, 
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<Quaternion> networkRotation = new(Quaternion.identity, 
            NetworkVariableReadPermission.Everyone, 
            NetworkVariableWritePermission.Owner);
        
        public Vector3 networkPositionVelocity;
        public float networkPositionSmoothTime = 0.1f;
        public float networkRotationSmoothTime = 0.1f;
    
        [Header("Animator")]
        public NetworkVariable<float> horizontalMovement = new(0, 
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> verticalMovement = new(0, 
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> moveAmount = new(0, 
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        protected virtual void Awake() => _characterManager = GetComponent<CharacterManager>();

        // A server RPC is a method that is called on the server and executed on the clients
        [ServerRpc]
        public void NotifyTheServerOfActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            // If this is the server, play the action animation for all clients
            if (IsServer)
            {
                PlayActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);
            }
        }
        
        // A client RPC is a method that is called on the client and executed on the server
        [ClientRpc]
        public void PlayActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            // We make sure to not run the function on the character who sent it (so we don't play the animation twice)
            if (clientID != NetworkManager.Singleton.LocalClientId)
            {
                PerformActionAnimationFromServer(animationID, applyRootMotion);
            }
        }
        
        private void PerformActionAnimationFromServer(string animationID, bool applyRootMotion)
        {
            // Play the action animation on the server
            _characterManager.applyRootMotion = applyRootMotion;
            _characterManager.animator.CrossFade(animationID, 0.2f);
        }
    }
}