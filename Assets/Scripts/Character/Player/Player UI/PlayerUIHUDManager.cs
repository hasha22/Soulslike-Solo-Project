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
}
