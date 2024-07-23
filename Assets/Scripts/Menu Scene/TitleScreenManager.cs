using Unity.Netcode;
using UnityEngine;

namespace Menu_Scene
{
    public class TitleScreenManager : MonoBehaviour
    {
        public void StartNetworkAsHost() => NetworkManager.Singleton.StartHost();

        public void StartNewGame()
        {
            StartCoroutine(WorldSaveGameManager.Instance.LoadNewGame());
        }
    }
}
