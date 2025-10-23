using UnityEngine;
using System.Linq;
using System.IO.IsolatedStorage;

public class WorldActionManager : MonoBehaviour
{
    public static WorldActionManager instance;

    [Header("World Item Actions")]
    public WeaponItemAction[] weaponItemAction;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        for (int i = 0; i < weaponItemAction.Length; i++)
        {
            weaponItemAction[i].actionID = i;
        }
    }
    
    public WeaponItemAction GetWeaponItemActionByID(int ID)
    {
        return weaponItemAction.FirstOrDefault(action => action.actionID == ID);
    }
}
