using UnityEngine;


public class PlayerUIHUDManager : MonoBehaviour
{
    [SerializeField] UI_StatBar healthBar;
    [SerializeField] UI_StatBar staminaBar;
    [SerializeField] UI_StatBar focusPointsBar;

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
    public void SetMaxStaminaValue(int maxStamina)
    {
        staminaBar.SetMaxStat(maxStamina);
    }
    public void SetMaxHealthValue(int maxHealth)
    {
        healthBar.SetMaxStat(maxHealth);
    }
    public void SetMaxFPValue(int maxFocusPoints)
    {
        focusPointsBar.SetMaxStat(maxFocusPoints);
    }
}
