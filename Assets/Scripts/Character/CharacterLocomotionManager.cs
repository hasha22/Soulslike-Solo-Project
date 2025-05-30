using UnityEngine;
#if UNITY_EDITOR
#endif
public class CharacterLocomotionManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Ground Check & Jumping ")]
    [SerializeField] protected float gravityForce = -5.55f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckSphereRadius = 1;
    [SerializeField] protected Vector3 yVelocity; //falling and jumping force velocity 
    [SerializeField] protected float groundedYVelocity = -20;
    [SerializeField] protected float fallStartYVelocity = -5;
    protected bool fallingVelocitySet = false;
    protected float inAirTimer = 0;

    [Header("Flags")]
    public bool isRolling = false;
    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
    protected virtual void Update()
    {
        HandleGroundCheck();
        if (character.isGrounded)
        {
            if (yVelocity.y < 0)
            {
                inAirTimer = 0;
                fallingVelocitySet = false;
                yVelocity.y = groundedYVelocity;
            }
        }
        else
        {
            if (!character.characterNetworkManager.isJumping.Value && !fallingVelocitySet)
            {
                fallingVelocitySet = true;
                yVelocity.y = fallStartYVelocity;
            }
            inAirTimer += Time.deltaTime;
            character.animator.SetFloat("InAirTimer", inAirTimer);

            yVelocity.y += gravityForce * Time.deltaTime;
        }
        character.characterController.Move(yVelocity * Time.deltaTime);

    }
    public void EnableCanRotate()
    {
        character.canRotate = true;
    }
    public void DisableCanRotate()
    {
        character.canRotate = false;
    }
    protected void HandleGroundCheck()
    {
        character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
    }
#if !UNITY_EDITOR
    protected void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
    }
#endif
}
