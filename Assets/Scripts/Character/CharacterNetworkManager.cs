using UnityEngine;
using Unity.Netcode;

namespace Character
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
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
    }
}