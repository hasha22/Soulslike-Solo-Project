using System.Linq;
using UnityEngine;

public class WorldActionManager : MonoBehaviour
{
    public static WorldActionManager instance { get; private set; }

    [Header("Weapon Item Actions")]
    public WeaponItemAction[] weaponItemActions;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        for (int i = 0; i < weaponItemActions.Length; i++)
        {
            weaponItemActions[i].actionID = i;
        }
    }
    public WeaponItemAction GetWeaponItemAction(int ID)
    {
        return weaponItemActions.FirstOrDefault(action => action.actionID == ID);
    }
}
