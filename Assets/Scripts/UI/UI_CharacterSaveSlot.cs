using System;
using Game_Saving;
using Menu_Scene;
using TMPro;
using UnityEngine;
using World_Manager;

namespace UI
{
    public class UI_CharacterSaveSlot : MonoBehaviour
    {
        private SaveFileDataWriter _saveFileDataWriter;
        
        [Header("Game Slot")]
        public CharacterSlot characterSlot;

        [Header("Character Info")] 
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI timedPlayed;

        private void OnEnable() => LoadSaveSlots();

        private void LoadSaveSlots()
        {
            _saveFileDataWriter = new SaveFileDataWriter
            {
                SaveDataDirectoryPath = Application.persistentDataPath
            };

            switch (characterSlot)
            {
                // save slot 01
                case CharacterSlot.CharacterSlot_01:
                {
                    _saveFileDataWriter.SaveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    // If the file exists, get information from it
                    if (_saveFileDataWriter.CheckToSeeIfFileExists())
                        characterName.text = WorldSaveGameManager.Instance.characterSlot01.characterName;
                    // If the file does not exist, disable the game object
                    else gameObject.SetActive(false);
                    break;
                }
                // save slot 02
                case CharacterSlot.CharacterSlot_02:
                {
                    _saveFileDataWriter.SaveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    // If the file exists, get information from it
                    if (_saveFileDataWriter.CheckToSeeIfFileExists())
                        characterName.text = WorldSaveGameManager.Instance.characterSlot02.characterName;
                    // If the file does not exist, disable the game object
                    else gameObject.SetActive(false);
                    break;
                }
                // save slot 03
                case CharacterSlot.CharacterSlot_03:
                {
                    _saveFileDataWriter.SaveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    // If the file exists, get information from it
                    if (_saveFileDataWriter.CheckToSeeIfFileExists())
                        characterName.text = WorldSaveGameManager.Instance.characterSlot03.characterName;
                    // If the file does not exist, disable the game object
                    else gameObject.SetActive(false);
                    break;
                }
                // save slot 04
                case CharacterSlot.CharacterSlot_04:
                {
                    _saveFileDataWriter.SaveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    // If the file exists, get information from it
                    if (_saveFileDataWriter.CheckToSeeIfFileExists())
                        characterName.text = WorldSaveGameManager.Instance.characterSlot04.characterName;
                    // If the file does not exist, disable the game object
                    else gameObject.SetActive(false);
                    break;
                }
                // save slot 05
                case CharacterSlot.CharacterSlot_05:
                {
                    _saveFileDataWriter.SaveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    // If the file exists, get information from it
                    if (_saveFileDataWriter.CheckToSeeIfFileExists())
                        characterName.text = WorldSaveGameManager.Instance.characterSlot05.characterName;
                    // If the file does not exist, disable the game object
                    else gameObject.SetActive(false);
                    break;
                }
                // save slot 06
                case CharacterSlot.CharacterSlot_06:
                {
                    _saveFileDataWriter.SaveFileName =
                        WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    // If the file exists, get information from it
                    if (_saveFileDataWriter.CheckToSeeIfFileExists())
                        characterName.text = WorldSaveGameManager.Instance.characterSlot06.characterName;
                    // If the file does not exist, disable the game object
                    else gameObject.SetActive(false);
                    break;
                }
                // save slot 07
                case CharacterSlot.CharacterSlot_07:
                {
                    _saveFileDataWriter.SaveFileName =
                        WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    // If the file exists, get information from it
                    if (_saveFileDataWriter.CheckToSeeIfFileExists())
                        characterName.text = WorldSaveGameManager.Instance.characterSlot07.characterName;
                    // If the file does not exist, disable the game object
                    else gameObject.SetActive(false);
                    break;
                }
                // save slot 08
                case CharacterSlot.CharacterSlot_08:
                {
                    _saveFileDataWriter.SaveFileName =
                        WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    // If the file exists, get information from it
                    if (_saveFileDataWriter.CheckToSeeIfFileExists())
                        characterName.text = WorldSaveGameManager.Instance.characterSlot08.characterName;
                    // If the file does not exist, disable the game object
                    else gameObject.SetActive(false);
                    break;
                }
                // save slot 09
                case CharacterSlot.CharacterSlot_09:
                {
                    _saveFileDataWriter.SaveFileName =
                        WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    // If the file exists, get information from it
                    if (_saveFileDataWriter.CheckToSeeIfFileExists())
                        characterName.text = WorldSaveGameManager.Instance.characterSlot09.characterName;
                    // If the file does not exist, disable the game object
                    else gameObject.SetActive(false);
                    break;
                }
                // save slot 10
                case CharacterSlot.CharacterSlot_10:
                {
                    _saveFileDataWriter.SaveFileName =
                        WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    // If the file exists, get information from it
                    if (_saveFileDataWriter.CheckToSeeIfFileExists())
                        characterName.text = WorldSaveGameManager.Instance.characterSlot10.characterName;
                    // If the file does not exist, disable the game object
                    else gameObject.SetActive(false);
                    break;
                }
                case CharacterSlot.NO_SLOT: break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public void LoadGameFromCharacterSlot()
        {
            WorldSaveGameManager.Instance.currentCharacterSlotBeingUsed = characterSlot;
            WorldSaveGameManager.Instance.LoadGame();
        }

        public void SelectCurrentSlot() => TitleScreenManager.Instance.SelectCharacterSlot(characterSlot);
    }
}
