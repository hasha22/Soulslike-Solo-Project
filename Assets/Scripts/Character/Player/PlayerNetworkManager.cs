using Unity.Collections;
using Unity.Netcode;
public class PlayerNetworkManager : CharacterNetworkManager
{
    public NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>("Character", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    PlayerManager player;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    // will be used later for leveling up / items
    public void SetNewVigorValue(int oldValue, int newValue)
    {
        maxHealth.Value = player.playerStatManager.CalculateHealthBasedOnVigorLevel(newValue);
        currentHealth.Value = maxHealth.Value;
    }
    public void SetNewEnduranceValue(int oldValue, int newValue)
    {
        maxStamina.Value = player.playerStatManager.CalculateStaminaBasedOnEnduranceLevel(newValue);
        currentStamina.Value = maxStamina.Value;
    }
    public void SetNewMindValue(int oldValue, int newValue)
    {
        maxFocusPoints.Value = player.playerStatManager.CalculateFocusPointsBasedOnMindLevel(newValue);
        maxFocusPoints.Value = maxFocusPoints.Value;
    }

}
