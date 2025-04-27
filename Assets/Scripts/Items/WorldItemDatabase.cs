using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldItemDatabase : MonoBehaviour
{
    public static WorldItemDatabase instance { get; private set; }

    private List<Item> items = new List<Item>();


    [Header("Weapons")]
    [SerializeField] public List<WeaponItem> weapons = new List<WeaponItem>();
    public WeaponItem unarmedWeapon;
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
        foreach (var weapon in weapons)
        {
            items.Add(weapon);
        }
        for (int i = 0; i < items.Count; i++)
        {
            items[i].itemID = i;
        }
    }
    public WeaponItem GetWeaponByID(int ID)
    {
        return weapons.FirstOrDefault(weapon => weapon.itemID == ID);
    }
}
