using System;
using System.IO;
using UnityEngine;

namespace Game_Saving
{
    public class SaveFileDataWriter
    {
        public string SaveDataDirectoryPath = "";
        public string SaveFileName = "";
        
        // Before we create a save file, we need to check if the file already exists. (Max 10 character slots)
        public bool CheckToSeeIfFileExists() => File.Exists(Path.Combine(SaveDataDirectoryPath, SaveFileName));

        // Used to delete character save files
        public void DeleteSaveFile() => File.Delete(Path.Combine(SaveDataDirectoryPath, SaveFileName));

        // Used to create a new character save file
        public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
        {
            // Make a path to save the file (a location on the machine)
            var savePath = Path.Combine(SaveDataDirectoryPath, SaveFileName);

            try
            {
                // Create the directory for the file will be written to, if it doesn't exist
                Directory.CreateDirectory(Path.GetDirectoryName(savePath) ?? string.Empty);
                Debug.Log("Directory created:" + savePath);
                
                // Serialize the data to a JSON format
                var dataToStore = JsonUtility.ToJson(characterData, true);
                
                // Write the data to the file
                using var stream = new FileStream(savePath, FileMode.Create);
                using var fileWriter = new StreamWriter(stream);
                fileWriter.Write(dataToStore);
            }
            catch (Exception e)
            {
                Debug.LogError("Error saving file: " + e.Message);
                throw;
            }
        }
        
        // Used to Load a save file
        public CharacterSaveData LoadSaveFile()
        {
            CharacterSaveData characterData = null;
            // Make a path to save the file (a location on the machine)
            var loadPath = Path.Combine(SaveDataDirectoryPath, SaveFileName);

            if (File.Exists(loadPath))
            {
                try
                {
                    string dataToLoad;

                    using (var stream = new FileStream(loadPath, FileMode.Open))
                    {
                        using (var fileReader = new StreamReader(stream)) 
                            dataToLoad = fileReader.ReadToEnd();
                    }
                
                    // Deserialize the data from a JSON format
                    characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return characterData;
        }
    }
}