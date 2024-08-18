using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using World_Manager;

namespace Menu_Scene
{
    public class TitleScreenManager : MonoBehaviour
    {
        [Header("Menus")]
        [SerializeField] private GameObject titleScreenMainMenu;
        [SerializeField] private GameObject titleScreenLoadMenu;
        
        [Header("Buttons")]
        [SerializeField] private Button loadMenuReturnButton;
        [SerializeField] private Button mainMenuLoadGameButton;
        
        
        public void StartNetworkAsHost() => NetworkManager.Singleton.StartHost();

        public void StartNewGame()
        {
            WorldSaveGameManager.Instance.CreateNewGame();
            StartCoroutine(WorldSaveGameManager.Instance.LoadWorldScene());
        }

        public void OpenLoadGameMenu()
        {
            // Close the main menu
            titleScreenMainMenu.SetActive(false);
            // Open load menu
            titleScreenLoadMenu.SetActive(true);
            
            // Select the return button first
            loadMenuReturnButton.Select();
        }
        
        public void CloseLoadGameMenu()
        {
            // Open the main menu
            titleScreenMainMenu.SetActive(true);
            // Close the load menu
            titleScreenLoadMenu.SetActive(false);
            
            mainMenuLoadGameButton.Select();
        }
    }
}
