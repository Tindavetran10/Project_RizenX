using Unity.Netcode;
using UnityEngine;

namespace Character.Player.Player_UI
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager Instance { get; private set; }
        [Header("NETWORK JOIN")] 
        [SerializeField] private bool startGameAsClient;

        [HideInInspector] public PlayerUIHudManager playerUIHudManager;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else Destroy(gameObject);
            
            playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
        }

        private void Start() => DontDestroyOnLoad(gameObject);

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
