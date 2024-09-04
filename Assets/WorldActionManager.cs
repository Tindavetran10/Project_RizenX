using System.Linq;
using UnityEngine;
using Weapon_Actions;

public class WorldActionManager : MonoBehaviour
{
    public static WorldActionManager instance;

    [Header("Weapon Item Actions")]
    public WeaponItemActions[] weaponItemActions;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        for (var i = 0; i < weaponItemActions.Length; i++) 
            weaponItemActions[i].actionId = i;
    }
    
    public WeaponItemActions GetWeaponItemActionByID(int ID) => 
        weaponItemActions.FirstOrDefault(action => action.actionId == ID);
}
