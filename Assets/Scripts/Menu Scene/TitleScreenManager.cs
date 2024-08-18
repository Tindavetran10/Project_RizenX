using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using World_Manager;

namespace Menu_Scene
{
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager Instance { get; private set; }
        
        [Header("Menus")]
        [SerializeField] private GameObject titleScreenMainMenu;
        [SerializeField] private GameObject titleScreenLoadMenu;
        
        [Header("Buttons")]
        [SerializeField] private Button loadMenuReturnButton;
        [SerializeField] private Button mainMenuLoadGameButton;
        [SerializeField] private Button mainMenuNewGameButton;

        [Header("Pop Ups")]
        [SerializeField] private GameObject noCharacterSlotsPopUp;
        [SerializeField] private Button noCharacterSlotsOkayButton;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else Destroy(gameObject);
        }

        public void StartNetworkAsHost() => NetworkManager.Singleton.StartHost();

        public void StartNewGame() => WorldSaveGameManager.Instance.AttemptToCreateNewGame();

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
        
        public void DisplayNoFreeCharacterSlotsPopUp()
        {
            // Display a message that there are no free slots
            noCharacterSlotsPopUp.SetActive(true);
            // Select the okay button
            noCharacterSlotsOkayButton.Select();
        }
        
        public void CloseNoFreeCharacterSlotsPopUp()
        {
            // Close the pop up
            noCharacterSlotsPopUp.SetActive(false);
            // Select the return button
            mainMenuNewGameButton.Select();
        }
    }
}
