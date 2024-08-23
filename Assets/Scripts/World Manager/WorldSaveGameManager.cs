using System.Collections;
using Character.Player;
using Game_Saving;
using Menu_Scene;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace World_Manager
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager Instance { get; private set; }

        public PlayerManager playerManager;
        
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
        public CharacterSaveData characterSlot01;
        public CharacterSaveData characterSlot02;
        public CharacterSaveData characterSlot03;
        public CharacterSaveData characterSlot04;
        public CharacterSaveData characterSlot05;
        public CharacterSaveData characterSlot06;
        public CharacterSaveData characterSlot07;
        public CharacterSaveData characterSlot08;
        public CharacterSaveData characterSlot09;
        public CharacterSaveData characterSlot10;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            LoadAllCharacterProfiles();
        }

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

        public string DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot)
        {
            string fileName = "";
            switch (characterSlot)
            {
                case CharacterSlot.CharacterSlot_01:
                    fileName = "characterSlot_01";
                    break;
                case CharacterSlot.CharacterSlot_02:
                    fileName = "characterSlot_02";
                    break;
                case CharacterSlot.CharacterSlot_03:
                    fileName = "characterSlot_03";
                    break;
                case CharacterSlot.CharacterSlot_04:
                    fileName = "characterSlot_04";
                    break;
                case CharacterSlot.CharacterSlot_05:
                    fileName = "characterSlot_05";
                    break;
                case CharacterSlot.CharacterSlot_06:
                    fileName = "characterSlot_06";
                    break;
                case CharacterSlot.CharacterSlot_07:
                    fileName = "characterSlot_07";
                    break;
                case CharacterSlot.CharacterSlot_08:
                    fileName = "characterSlot_08";
                    break;
                case CharacterSlot.CharacterSlot_09:
                    fileName = "characterSlot_09";
                    break;
                case CharacterSlot.CharacterSlot_10:
                    fileName = "characterSlot_10";
                    break;
            }

            return fileName;
        }

        public void AttemptToCreateNewGame()
        {
            _saveFileDataWriter = new SaveFileDataWriter
            {
                SaveDataDirectoryPath = Application.persistentDataPath,
                // Check to see if we can create a new save file (check for other existing files first)
                SaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01)
            };


            if(!_saveFileDataWriter.CheckToSeeIfFileExists())
            {
                // If this profile slot is not taken, make a new file using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_01;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            
            // Check to see if we can create a new save file (check for other existing files first)
            _saveFileDataWriter.SaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
            
            if(!_saveFileDataWriter.CheckToSeeIfFileExists())
            {
                // If this profile slot is not taken, make a new file using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_02;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            
            // Check to see if we can create a new save file (check for other existing files first)
            _saveFileDataWriter.SaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
            
            if(!_saveFileDataWriter.CheckToSeeIfFileExists())
            {
                // If this profile slot is not taken, make a new file using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_03;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            
            // Check to see if we can create a new save file (check for other existing files first)
            _saveFileDataWriter.SaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
            
            if(!_saveFileDataWriter.CheckToSeeIfFileExists())
            {
                // If this profile slot is not taken, make a new file using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_04;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            
            // Check to see if we can create a new save file (check for other existing files first)
            _saveFileDataWriter.SaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
            
            if(!_saveFileDataWriter.CheckToSeeIfFileExists())
            {
                // If this profile slot is not taken, make a new file using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_05;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            
            // Check to see if we can create a new save file (check for other existing files first)
            _saveFileDataWriter.SaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
            
            if(!_saveFileDataWriter.CheckToSeeIfFileExists())
            {
                // If this profile slot is not taken, make a new file using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_06;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            
            // Check to see if we can create a new save file (check for other existing files first)
            _saveFileDataWriter.SaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
            
            if(!_saveFileDataWriter.CheckToSeeIfFileExists())
            {
                // If this profile slot is not taken, make a new file using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_07;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            
            // Check to see if we can create a new save file (check for other existing files first)
            _saveFileDataWriter.SaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
            
            if(!_saveFileDataWriter.CheckToSeeIfFileExists())
            {
                // If this profile slot is not taken, make a new file using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_08;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            
            // Check to see if we can create a new save file (check for other existing files first)
            _saveFileDataWriter.SaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
            
            if(!_saveFileDataWriter.CheckToSeeIfFileExists())
            {
                // If this profile slot is not taken, make a new file using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_09;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            
            // Check to see if we can create a new save file (check for other existing files first)
            _saveFileDataWriter.SaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
            
            if(!_saveFileDataWriter.CheckToSeeIfFileExists())
            {
                // If this profile slot is not taken, make a new file using this slot
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_10;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            
            // If there is no free slot, notify the player
            TitleScreenManager.Instance.DisplayNoFreeCharacterSlotsPopUp();
        }

        private void NewGame()
        {
            // Saves the newly created character stats, and items (when creation screen is added)
            SaveGame();
            StartCoroutine(LoadWorldScene());
        }
        
        public void LoadGame()
        {
            // Load a previous file, with a file name depending on which slot we are using
            _saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);
            
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
            _saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);
            
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
        
        public void DeleteGame(CharacterSlot characterSlot)
        {
            // Choose the file based on name
            _saveFileDataWriter = new SaveFileDataWriter();
            // Generally works on multiple machine types
            _saveFileDataWriter.SaveDataDirectoryPath = Application.persistentDataPath;
            _saveFileDataWriter.SaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
            
            _saveFileDataWriter.DeleteSaveFile();
        }
        
        // Load all character profiles on the device when starting the game
        private void LoadAllCharacterProfiles()
        {
            _saveFileDataWriter = new SaveFileDataWriter
            {
                // Generally works on multiple machine types
                SaveDataDirectoryPath = Application.persistentDataPath,
                SaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01)
            };

            characterSlot01 = _saveFileDataWriter.LoadSaveFile();
            
            _saveFileDataWriter.SaveFileName =
                DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
            characterSlot02 = _saveFileDataWriter.LoadSaveFile();
            
            _saveFileDataWriter.SaveFileName =
                DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
            characterSlot03 = _saveFileDataWriter.LoadSaveFile();
            
            _saveFileDataWriter.SaveFileName =
                DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
            characterSlot04 = _saveFileDataWriter.LoadSaveFile();
            
            _saveFileDataWriter.SaveFileName =
                DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
            characterSlot05 = _saveFileDataWriter.LoadSaveFile();
            
            _saveFileDataWriter.SaveFileName =
                DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
            characterSlot06 = _saveFileDataWriter.LoadSaveFile();
            
            _saveFileDataWriter.SaveFileName =
                DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
            characterSlot07 = _saveFileDataWriter.LoadSaveFile();
            
            _saveFileDataWriter.SaveFileName =
                DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
            characterSlot08 = _saveFileDataWriter.LoadSaveFile();
            
            _saveFileDataWriter.SaveFileName =
                DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
            characterSlot09 = _saveFileDataWriter.LoadSaveFile();
            
            _saveFileDataWriter.SaveFileName =
                DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
            characterSlot10 = _saveFileDataWriter.LoadSaveFile();
        }

        private IEnumerator LoadWorldScene()
        {
            // If you want to use the same scene for all levels in your project, use this
            var loadOperation = SceneManager.LoadSceneAsync(worldScreenIndex);
            
            // If you want to use different scenes for different levels in your project, use this
            //var loadOperation = SceneManager.LoadSceneAsync(currentCharacterData.sceneIndex);
            
            playerManager.LoadGameDataFromCurrentCharacterData(ref currentCharacterData);
            yield return null;
        }
    
        public int GetWorldSceneIndex() => worldScreenIndex;
    }
}
