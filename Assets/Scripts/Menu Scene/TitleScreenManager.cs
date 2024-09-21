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
        [SerializeField] private Button deleteCharacterPopUpConfirmButton;
        
        [Header("Pop Ups")]
        [SerializeField] private GameObject noCharacterSlotsPopUp;
        [SerializeField] private Button noCharacterSlotsOkayButton;
        [SerializeField] public GameObject deleteCharacterSlotPopUp;
        
        [Header("Character Slots")]
        public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;
        
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
            // Close the pop-up
            noCharacterSlotsPopUp.SetActive(false);
            // Select the return button
            mainMenuNewGameButton.Select();
        }
        
        // Character Slots
        public void SelectCharacterSlot(CharacterSlot characterSlot) => currentSelectedSlot = characterSlot;

        public void SelectNoSlot() => currentSelectedSlot = CharacterSlot.NO_SLOT;
        
        public void AttemptToDeleteCharacterSlot()
        {
            if (currentSelectedSlot != CharacterSlot.NO_SLOT)
            {
                deleteCharacterSlotPopUp.SetActive(true);
                deleteCharacterPopUpConfirmButton.Select();
            }
        }
        
        public void DeleteCharacterSlot()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            WorldSaveGameManager.Instance.DeleteGame(currentSelectedSlot);
            
            // we disable and then enable the load menu to refresh the character slots
            // (The deleted slots will now become inactive)
            titleScreenLoadMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);
            
            loadMenuReturnButton.Select();
        }
        
        public void CloseDeleteCharacterSlotPopUp()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            loadMenuReturnButton.Select();
        }
    }
}
