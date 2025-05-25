using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
public class CharacterManager : NetworkBehaviour
{
    [Header("Status")]
    public NetworkVariable<bool> isAlive = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterNetworkManager characterNetworkManager;
    [HideInInspector] public PlayerManager player;
    [HideInInspector] public CharacterEffectsManager characterEffectsManager;
    [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
    [HideInInspector] public CharacterCombatManager characterCombatManager;
    [HideInInspector] public CharacterSFX characterSFX;
    [HideInInspector] public CharacterLocomotionManager characterLocomotionManager;

    [Header("Flags")]
    public bool isPerformingAction = false;
    public bool isGrounded = true;
    public bool canRotate = true;
    public bool canMove = true;
    public bool hadLoadedPosition = false;

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        characterNetworkManager = GetComponent<CharacterNetworkManager>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        characterCombatManager = GetComponent<CharacterCombatManager>();
        characterSFX = GetComponent<CharacterSFX>();
        characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
    }
    protected virtual void Start()
    {
        IgnoreOwnColliders();
    }
    protected virtual void Update()
    {
        animator.SetBool("IsGrounded", isGrounded);
        if (IsOwner)
        {
            characterNetworkManager.networkPosition.Value = transform.position;
            characterNetworkManager.networkRotation.Value = transform.rotation;
        }
        else
        {
            //Position
            transform.position = Vector3.SmoothDamp(transform.position,
                characterNetworkManager.networkPosition.Value,
                ref characterNetworkManager.networkPositionVelocity,
                characterNetworkManager.networkPositionSmoothTime);
            //Rotation
            transform.rotation = Quaternion.Slerp(transform.rotation,
                characterNetworkManager.networkRotation.Value,
                characterNetworkManager.networkRotationSmoothTime);
        }
    }
    protected virtual void LateUpdate()
    {

    }
    public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation)
    {
        if (!IsOwner)
            yield return null;
        characterNetworkManager.currentHealth.Value = 0;
        isAlive.Value = false;

        if (!manuallySelectDeathAnimation)
        {
            characterAnimatorManager.PlayActionAnimation("Dead_01", true);
        }

        yield return new WaitForSeconds(5);
    }
    public virtual void ReviveCharacter()
    {

    }
    protected virtual void IgnoreOwnColliders()
    {
        Collider characterControllerCollider = GetComponent<Collider>();
        Collider[] damageableCharacterColliders = GetComponentsInChildren<Collider>();

        List<Collider> ignoreColliders = new List<Collider>();
        foreach (var collider in damageableCharacterColliders)
        {
            ignoreColliders.Add(collider);
        }
        ignoreColliders.Add(characterControllerCollider);
        foreach (var collider in ignoreColliders)
        {
            foreach (var otherCollider in ignoreColliders)
            {
                Physics.IgnoreCollision(collider, otherCollider, true);
            }
        }
    }
}
