using UnityEngine;

public class AICharacterManager : CharacterManager
{
    [Header("Current State")]
    [SerializeField] AIState currentState;

    [HideInInspector] public AICharacterCombatManager aiCharacterCombatManager;
    protected override void Awake()
    {
        aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();

        // To avoid errors
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        characterNetworkManager = GetComponent<CharacterNetworkManager>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        characterCombatManager = GetComponent<CharacterCombatManager>();
        characterSFX = GetComponent<CharacterSFX>();
        characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        ProcessStateMachine();
    }
    private void ProcessStateMachine()
    {
        AIState nextState = currentState?.Tick(this);
        if (nextState != null)
        {
            currentState = nextState;
        }
    }
}
