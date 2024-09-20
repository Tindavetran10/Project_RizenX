using System.Collections.Generic;
using Character.Player;
using UnityEngine;

namespace World_Manager
{
    public class WorldGameSessionManager : MonoBehaviour
    {
        public static WorldGameSessionManager instance;
    
        [Header("Active Players In Session")]
        public List<PlayerManager> players = new();

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else Destroy(gameObject);
        }

        public void AddPlayerToActivePlayerList(PlayerManager player)
        {
            // Check the list, if it does not already contain the player, add it
            if (!players.Contains(player)) 
                players.Add(player);

            // Check the list for null slots, and remove them
            for (var i = players.Count - 1; i > -1; i--)
            {
                if(players[i] == null)
                    players.RemoveAt(i);
            }
        }
    
        public void RemovePlayerFromActivePlayerList(PlayerManager player)
        {
            // Check the list, if it contains the player, remove it
            if(players.Contains(player))
                players.Remove(player);
        
            // Check the list for null slots, and remove them
            for (var i = players.Count - 1; i > -1; i--)
            {
                if(players[i] == null)
                    players.RemoveAt(i);
            }
        }
    
    }
}
