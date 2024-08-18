using System.Collections;
using Character.Player;
using Game_Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace World_Manager
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager Instance { get; private set; }

        [SerializeField] private PlayerManager playerManager;
        
        [Header("SAVE/LOAD")]
        [SerializeField] private bool saveGame;
        [SerializeField] private bool loadGame;
        
        [Header("World Screen Index")]
        [SerializeField] private int worldScreenIndex;

        [Header("Save Data Writer")] 
        private SaveFileDataWriter _saveFileDataWriter;
        
        [Header("Current Character Data")]
        public CharacterSlot currentCharacterSlotBeingUsed;
        public CharacterSaveData currentCharacterData;
        private string _saveFileName;
        
        [Header("Character Slots")]
        public CharacterSaveData characterSlot1;
        public CharacterSaveData characterSlot2;
        public CharacterSaveData characterSlot3;
        public CharacterSaveData characterSlot4;
        public CharacterSaveData characterSlot5;
        public CharacterSaveData characterSlot6;
        public CharacterSaveData characterSlot7;
        public CharacterSaveData characterSlot8;
        public CharacterSaveData characterSlot9;
        public CharacterSaveData characterSlot10;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else Destroy(gameObject);
        }

        private void Start() => DontDestroyOnLoad(gameObject);

        private void Update()
        {
            if (saveGame)
            {
                saveGame = false;
                SaveGame();
            }
            
            if(loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }

        private void DecideCharacterFileNameBasedOnCharacterSlotBeingUsed()
        {
            switch (currentCharacterSlotBeingUsed)
            {
                case CharacterSlot.CharacterSlot_01:
                    _saveFileName = "characterSlot_01";
                    break;
                case CharacterSlot.CharacterSlot_02:
                    _saveFileName = "characterSlot_02";
                    break;
                case CharacterSlot.CharacterSlot_03:
                    _saveFileName = "characterSlot_03";
                    break;
                case CharacterSlot.CharacterSlot_04:
                    _saveFileName = "characterSlot_04";
                    break;
                case CharacterSlot.CharacterSlot_05:
                    _saveFileName = "characterSlot_05";
                    break;
                case CharacterSlot.CharacterSlot_06:
                    _saveFileName = "characterSlot_06";
                    break;
                case CharacterSlot.CharacterSlot_07:
                    _saveFileName = "characterSlot_07";
                    break;
                case CharacterSlot.CharacterSlot_08:
                    _saveFileName = "characterSlot_08";
                    break;
                case CharacterSlot.CharacterSlot_09:
                    _saveFileName = "characterSlot_09";
                    break;
                case CharacterSlot.CharacterSlot_10:
                    _saveFileName = "characterSlot_10";
                    break;
            }
        }

        public void CreateNewGame()
        {
            // Create a new file, with a file name depending on which slot we are using
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();
            currentCharacterData = new CharacterSaveData();
        }

        private void LoadGame()
        {
            // Load a previous file, with a file name depending on which slot we are using
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();
            
            _saveFileDataWriter = new SaveFileDataWriter
            {
                // Generally works on multiple machine types
                SaveDataDirectoryPath = Application.persistentDataPath,
                SaveFileName = _saveFileName
            };

            currentCharacterData = _saveFileDataWriter.LoadSaveFile();

            StartCoroutine(LoadWorldScene());
        }

        private void SaveGame()
        {
            // Save the current file under a file name depending on which slot we are using
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();
            
            _saveFileDataWriter = new SaveFileDataWriter
            {
                // Generally works on multiple machine types
                SaveDataDirectoryPath = Application.persistentDataPath,
                SaveFileName = _saveFileName
            };

            // Pass the players info, from game to their save file
            playerManager.SaveGameDataToCurrentCharacterData(ref currentCharacterData);
            
            // Write that info onto a JSON file, save it to the machine
            _saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
        }
        
        public IEnumerator LoadWorldScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldScreenIndex);
            yield return null;
        }
    
        public int GetWorldSceneIndex() => worldScreenIndex;
    }
}
