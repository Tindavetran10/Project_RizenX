using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEngine;

public class WorldItemDatabase : MonoBehaviour
{
    public static WorldItemDatabase instance;
    
    public WeaponItem unarmedWeapon;
    
    [Header("Weapons")]
    [SerializeField] private List<WeaponItem> weapons = new();
    
    // A List of Every Item in the Game
    [Header("Items")]
    [SerializeField] private List<Item> items = new();

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else Destroy(this);

        // Add all of our weapons to the item list
        foreach (var weapon in weapons) 
            items.Add(weapon);

        // Assign all of our items a unique ID
        for (var i = 0; i < items.Count; i++) 
            items[i].itemID = i;
    }
    
    public WeaponItem GetWeaponItemByID(int id) => 
        weapons.FirstOrDefault(weapon => weapon.itemID == id);
}
