public class PlayerAnimationManager : CharacterAnimatorManager
{
    PlayerManager player;
    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

}
