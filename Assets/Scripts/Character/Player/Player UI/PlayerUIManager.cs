using Unity.Netcode;
using UnityEngine;

namespace Character.Player.Player_UI
{
    public class PlayerUIManager : MonoBehaviour
    {
        private static PlayerUIManager Instance { get; set; }
        [Header("NETWORK JOIN")] 
        [SerializeField] private bool startGameAsClient;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (startGameAsClient)
            {
                startGameAsClient = false;
                // We must first shut down, because we have started as a host during the title screen
                NetworkManager.Singleton.Shutdown();
                // We then restart, as a client
                NetworkManager.Singleton.StartClient();
            }
        }
    }
}
