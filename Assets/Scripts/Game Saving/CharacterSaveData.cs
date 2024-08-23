using UnityEngine;

namespace Game_Saving
{
    [System.Serializable]
    // Since we want to reference this data for every save file.
    // This script is not a mono behavior script and serializable instead.
    public class CharacterSaveData
    {
        [Header("Scene Index")] 
        public int sceneIndex;
        
        [Header("Character Name")] 
        public string characterName = "Character";
        
        [Header("Time Played")] 
        public float secondsPlayed;
        
        // We can only save basic variables, so no vector3 or quaternion.
        [Header("World Coordinates")] 
        public float xPosition;
        public float yPosition;
        public float zPosition;
        
        [Header("Resources")]
        public float currentHealth;
        public float currentStamina;
        
        [Header("Stats")]
        public int vitality;
        public int endurance;
    }
}