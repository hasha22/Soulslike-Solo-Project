using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerUIHUDManager : MonoBehaviour
{
    [Header("Stat Bars")]
    [SerializeField] UI_StatBar healthBar;
    [SerializeField] UI_StatBar staminaBar;
    [SerializeField] UI_StatBar focusPointsBar;

    [Header("Quick Slots")]
    [SerializeField] Image rightWeaponQuickSlotIcon;
    [SerializeField] Image leftWeaponQuickSlotIcon;
    [SerializeField] Image magicQuickSlotIcon;
    [SerializeField] Image itemSlotIcon;

    [SerializeField] private TextMeshProUGUI stuff;
    public void SetNewStaminaValue(float oldValue, float newValue)
    {
        staminaBar.SetStat(Mathf.RoundToInt(newValue));
    }
    public void SetNewHealthValue(float oldValue, float newValue)
    {
        healthBar.SetStat(Mathf.RoundToInt(newValue));
    }
    public void SetNewFPValue(float oldValue, float newValue)
    {
        focusPointsBar.SetStat(Mathf.RoundToInt(newValue));
    }
    public void SetMaxStaminaValue(int maxStamina, float currentStamina)
    {
        staminaBar.SetMaxStat(maxStamina, currentStamina);
    }
    public void SetMaxHealthValue(int maxHealth, float currentHealth)
    {
        healthBar.SetMaxStat(maxHealth, currentHealth);
    }
    public void SetMaxFPValue(int maxFocusPoints, float currentFocusPoints)
    {
        focusPointsBar.SetMaxStat(maxFocusPoints, currentFocusPoints);
    }
    public void SetRightWeaponQuickSlotIcon(int weaponID)
    {
        WeaponItem weaponItem = WorldItemDatabase.instance.GetWeaponByID(weaponID);
        if (weaponItem == null)
        {
            Debug.Log("ITEM IS NULL");
            rightWeaponQuickSlotIcon.enabled = false;
            rightWeaponQuickSlotIcon.sprite = null;
            return;
        }

        if (weaponItem.itemIcon == null)
        {
            Debug.Log("NO ICON!");
            rightWeaponQuickSlotIcon.enabled = false;
            rightWeaponQuickSlotIcon.sprite = null;
            return;
        }

        rightWeaponQuickSlotIcon.sprite = weaponItem.itemIcon;
        rightWeaponQuickSlotIcon.enabled = true;
    }
    public void SetLeftWeaponQuickSlotIcon(int weaponID)
    {
        WeaponItem weaponItem = WorldItemDatabase.instance.GetWeaponByID(weaponID);
        if (weaponItem == null)
        {
            Debug.Log("ITEM IS NULL");
            leftWeaponQuickSlotIcon.enabled = false;
            leftWeaponQuickSlotIcon.sprite = null;
            return;
        }

        if (weaponItem.itemIcon == null)
        {
            Debug.Log("NO ICON!");
            leftWeaponQuickSlotIcon.enabled = false;
            leftWeaponQuickSlotIcon.sprite = null;
            return;
        }

        leftWeaponQuickSlotIcon.sprite = weaponItem.itemIcon;
        leftWeaponQuickSlotIcon.enabled = true;
    }
}
