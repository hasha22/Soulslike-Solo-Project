using UnityEngine;
using UnityEngine.AI;

public class AICharacterManager : CharacterManager
{
    [Header("Current State")]
    [SerializeField] private AIState currentState;

    [Header("NavMesh Agent")]
    [HideInInspector] public NavMeshAgent navMeshAgent;

    [Header("States")]
    public IdleState idle;
    public PursueTargetState pursueTarget;

    [HideInInspector] public AICharacterCombatManager aiCharacterCombatManager;
    [HideInInspector] public AICharacterNetworkManager aiCharacterNetworkManager;
    [HideInInspector] public AICharacterLocomotionManager aiCharacterLocomotionManager;
    [HideInInspector] public AICharacterAnimatorManager aiCharacterAnimatorManager;
    protected override void Awake()
    {
        aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
        aiCharacterNetworkManager = GetComponent<AICharacterNetworkManager>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();
        aiCharacterAnimatorManager = GetComponent<AICharacterAnimatorManager>();

        // Copy of scriptable objects
        idle = Instantiate(idle);
        pursueTarget = Instantiate(pursueTarget);
        currentState = idle;

        // To avoid errors, don't get why it does this. 
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

        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;

        if (navMeshAgent.enabled)
        {
            Vector3 agentDestination = navMeshAgent.destination;
            float remainingDistance = Vector3.Distance(agentDestination, transform.position);

            if (remainingDistance > navMeshAgent.stoppingDistance)
            {
                aiCharacterNetworkManager.isMoving.Value = true;
            }
            else
            {
                aiCharacterNetworkManager.isMoving.Value = false;
            }
        }
        else
        {
            aiCharacterNetworkManager.isMoving.Value = false;
        }
    }
}
