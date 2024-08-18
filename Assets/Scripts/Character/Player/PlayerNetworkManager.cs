using Unity.Collections;
using Unity.Netcode;

namespace Character.Player
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        public NetworkVariable<FixedString64Bytes> characterName = new("Character", 
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    }
}