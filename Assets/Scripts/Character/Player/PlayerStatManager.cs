public class PlayerStatManager : CharacterStatManager
{
    PlayerManager player;
    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }
    protected override void Start()
    {
        base.Start();

        CalculateHealthBasedOnVigorLevel(player.playerNetworkManager.vigor.Value);
        CalculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value);
        CalculateFocusPointsBasedOnMindLevel(player.playerNetworkManager.mind.Value);
    }
}
