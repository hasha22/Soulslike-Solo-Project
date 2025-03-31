using UnityEngine;

public class CharacterStatManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Stamina Regeneration")]
    private float staminaRegenerationTimer = 0;
    private float staminaTickTimer = 0;
    [SerializeField] float staminaRegenerationDelay = 2;
    [SerializeField] int staminaRegenerationAmount = 1;
    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Start()
    {

    }

    // following calculations are the same as the Elden Ring calculations for determining stats
    public int CalculateHealthBasedOnVigorLevel(int vigor)
    {
        float health = 0;
        float exponent;
        float baseNumber;
        decimal result;
        if (vigor <= 25)
        {
            result = decimal.Round(decimal.Divide((vigor - 1), 24), 3);
            baseNumber = (float)result;
            exponent = 1.5f;
            health = 300 + 500 * Mathf.Pow(baseNumber, exponent);
        }
        if (vigor >= 26 && vigor <= 40)
        {
            result = decimal.Round(decimal.Divide((vigor - 25), 15), 3);
            baseNumber = (float)result;
            exponent = 1.1f;
            health = 800 + 650 * Mathf.Pow(baseNumber, exponent);
        }
        if (vigor >= 41 && vigor <= 60)
        {
            result = decimal.Round(decimal.Divide((vigor - 40), 20), 3);
            baseNumber = (float)result;
            exponent = 1.2f;
            health = 1450 + 450 * (1 - (1 - Mathf.Pow(baseNumber, exponent)));
        }
        if (vigor >= 61 && vigor <= 99)
        {
            result = decimal.Round(decimal.Divide((vigor - 60), 39), 3);
            baseNumber = (float)result;
            exponent = 1.2f;
            health = 1900 + 200 * (1 - (1 - Mathf.Pow(baseNumber, exponent)));
        }
        return Mathf.RoundToInt(health);
    }
    public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
    {
        float stamina = 0;
        float floatNumber;
        decimal result;
        if (stamina <= 15)
        {
            result = decimal.Round(decimal.Divide((endurance - 1), 14), 3);
            floatNumber = (float)result;
            stamina = 80 + 25 * floatNumber;
        }
        if (endurance >= 16 && endurance <= 35)
        {
            result = decimal.Round(decimal.Divide((endurance - 15), 15), 3);
            floatNumber = (float)result;
            stamina = 105 + 25 * floatNumber;
        }
        if (endurance >= 36 && endurance <= 60)
        {
            result = decimal.Round(decimal.Divide((endurance - 30), 20), 3);
            floatNumber = (float)result;
            stamina = 130 + 25 * floatNumber;
        }
        if (endurance >= 61 && endurance <= 99)
        {
            result = decimal.Round(decimal.Divide((endurance - 50), 49), 3);
            floatNumber = (float)result;
            stamina = 155 + 15 * floatNumber;
        }
        return Mathf.RoundToInt(stamina);
    }
    public int CalculateFocusPointsBasedOnMindLevel(int mind)
    {
        float focusPoints = 0;
        float floatNumber;
        float exponent;
        decimal result;
        if (mind <= 15)
        {
            result = decimal.Round(decimal.Divide((mind - 1), 14), 3);
            floatNumber = (float)result;
            focusPoints = 50 + 45 * floatNumber;
        }
        if (mind >= 16 && mind <= 35)
        {
            result = decimal.Round(decimal.Divide((mind - 15), 20), 3);
            floatNumber = (float)result;
            focusPoints = 95 + 105 * floatNumber;
        }
        if (mind >= 36 && mind <= 60)
        {
            result = decimal.Round(decimal.Divide((mind - 35), 25), 3);
            floatNumber = (float)result;
            exponent = 1.2f;
            focusPoints = 200 + 150 * (1 - (1 - (Mathf.Pow(floatNumber, exponent))));
        }
        if (mind >= 61 && mind <= 99)
        {
            result = decimal.Round(decimal.Divide((mind - 60), 39), 3);
            floatNumber = (float)result;
            focusPoints = 350 + 100 * floatNumber;
        }
        return Mathf.RoundToInt(focusPoints);
    }
    public virtual void RegenerateStamina()
    {
        if (!character.IsOwner)
            return;
        if (character.characterNetworkManager.isSprinting.Value)
            return;
        if (character.isPerformingAction)
            return;

        staminaRegenerationTimer += Time.deltaTime;
        if (staminaRegenerationTimer >= staminaRegenerationDelay)
        {
            if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
            {
                staminaTickTimer += Time.deltaTime;

                if (staminaTickTimer >= 0.1)
                {
                    staminaTickTimer = 0;
                    character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                }
            }
        }
    }

    public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount)
    {
        if (currentStaminaAmount < previousStaminaAmount)
        {
            staminaRegenerationTimer = 0;
        }
    }
}
