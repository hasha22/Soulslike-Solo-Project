using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance { get; private set; }
    public Camera cameraObject;
    public PlayerManager player;

    [SerializeField] Transform cameraPivotTransform;

    //camera performance
    [Header("Camera Settings")]
    [SerializeField] float cameraSmoothSpeed = 1; //the greater the number, the longer it takes for the camera to reach its position during movement
    [SerializeField] float leftAndRightRotationSpeed = 30;
    [SerializeField] float upAndDownRotationSpeed = 30;
    [SerializeField] float minimumPivot = -30; //lowest point to look down
    [SerializeField] float maximumPivot = 60; // highest point to look up
    [SerializeField] float cameraCollisionRadius = 0.2f;
    [SerializeField] LayerMask collideWithLayers;

    [Header("Camera Values")]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPosition;
    [SerializeField] float leftAndRightLookAngle;
    [SerializeField] float upAndDownLookAngle;
    private float cameraZPosition;
    private float targetCameraZPosition;

    [Header("Lock On")]
    [SerializeField] float lockOnRadius = 20;
    [SerializeField] float minimumViewableAngle = -50;
    [SerializeField] float maximumViewableAngle = 50;
    [SerializeField] float lockOnTargetFollowSpeed = 0.2f;
    [SerializeField] float setCameraHeightSpeed = 0.05f;
    [SerializeField] float unlockedCameraHeight = 1.65f;
    [SerializeField] float lockedOnCameraHeight = 2.0f;
    private List<CharacterManager> availableTargets = new List<CharacterManager>();
    private Coroutine cameraLockOnHeightCoroutine;
    public CharacterManager nearestLockOnTarget;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        cameraZPosition = cameraObject.transform.localPosition.z;
    }
    public void HandleAllCameraActions()
    {
        if (player != null)
        {
            HandleFollowTarget();
            HandleRotations();
            HandleCameraCollisions();
        }
    }
    private void HandleFollowTarget()
    {
        Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetCameraPosition;
    }
    private void HandleRotations()
    {
        if (player.playerNetworkManager.isLockedOn.Value)
        {
            // Rotates the game object
            Vector3 rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - transform.position;
            rotationDirection.Normalize();
            rotationDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockOnTargetFollowSpeed);

            // Rotates the pivot
            rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - cameraPivotTransform.position;
            rotationDirection.Normalize();
            targetRotation = Quaternion.LookRotation(rotationDirection);
            cameraPivotTransform.transform.rotation = Quaternion.Slerp(cameraPivotTransform.rotation, targetRotation, lockOnTargetFollowSpeed);

            leftAndRightLookAngle = transform.eulerAngles.y;
            upAndDownLookAngle = transform.eulerAngles.x;
        }
        else
        {
            leftAndRightLookAngle += (PlayerInputManager.instance.horizontalCameraInput * leftAndRightRotationSpeed) * Time.deltaTime;
            upAndDownLookAngle += (PlayerInputManager.instance.verticalCameraInput * upAndDownRotationSpeed) * Time.deltaTime;
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

            Vector3 cameraRotation;
            Quaternion targetRotation;

            //left/right
            cameraRotation = Vector3.zero;
            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            //up/down
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;
        }
    }
    private void HandleCameraCollisions()
    {
        targetCameraZPosition = cameraZPosition;

        RaycastHit hit;
        //direction for collision check
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();

        //checks if there is an object in "direction"
        if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
        {
            //if yes, gets the distance from it
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
        }
        //if target position is less than collision radius, subtract collision radius
        if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
        {
            targetCameraZPosition = -cameraCollisionRadius;
        }

        cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
        cameraObject.transform.localPosition = cameraObjectPosition;
    }
    public void HandleCameraLockOnTargets()
    {
        float shortestDistance = Mathf.Infinity;
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, lockOnRadius, WorldUtilityManager.instance.GetCharacterLayers());

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager lockOnTarget = colliders[i].GetComponent<CharacterManager>();
            if (lockOnTarget != null)
            {
                Vector3 lockOnTargetDirection = lockOnTarget.transform.position - player.transform.position;
                float viewableAngle = Vector3.Angle(lockOnTargetDirection, cameraObject.transform.forward);

                // If target is dead or the player themselves, check next potential target
                if (!lockOnTarget.isAlive.Value)
                    continue;
                if (lockOnTarget.transform.root == player.transform.root)
                    continue;

                // Checks if target is within FOV
                if (viewableAngle >= minimumViewableAngle && viewableAngle <= maximumViewableAngle)
                {
                    RaycastHit hit;
                    // If it hits something, target cannot be seen
                    if (Physics.Linecast(player.playerCombatManager.lockOnTransform.position, lockOnTarget.characterCombatManager.lockOnTransform.position, out hit,
                        WorldUtilityManager.instance.GetEnviroLayers()))
                        continue;
                    else
                    {
                        // Add to potential targets list
                        availableTargets.Add(lockOnTarget);
                    }
                }
            }
        }

        for (int i = 0; i < availableTargets.Count; i++)
        {
            if (availableTargets[i] != null)
            {
                float distanceFromTarget = Vector3.Distance(player.transform.position, availableTargets[i].transform.position);

                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTargets[i];
                }
            }
            else
            {
                ClearLockOnTargets();
                player.playerNetworkManager.isLockedOn.Value = false;
            }
        }
    }
    public void ClearLockOnTargets()
    {
        nearestLockOnTarget = null;
        availableTargets.Clear();
    }
    public void SetLockCameraHeight()
    {
        if (cameraLockOnHeightCoroutine != null)
        {
            StopCoroutine(cameraLockOnHeightCoroutine);
        }
        cameraLockOnHeightCoroutine = StartCoroutine(SetCameraHeight());
    }
    private IEnumerator SetCameraHeight()
    {
        float duration = 1;
        float timer = 0;

        Vector3 velocity = Vector3.zero;
        Vector3 newLockedOnCameraHeight = new Vector3(cameraPivotTransform.transform.localPosition.x, lockedOnCameraHeight);
        Vector3 newUnlockedCameraHeight = new Vector3(cameraPivotTransform.transform.localPosition.x, unlockedCameraHeight);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            if (player != null)
            {
                if (player.playerCombatManager.currentTarget != null)
                {
                    cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedOnCameraHeight, ref velocity, setCameraHeightSpeed);
                    cameraPivotTransform.transform.localRotation = Quaternion.Slerp(cameraPivotTransform.transform.localRotation, Quaternion.Euler(0, 0, 0), lockOnTargetFollowSpeed);
                }
                else
                {
                    cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedCameraHeight, ref velocity, setCameraHeightSpeed);


                }
            }
            yield return null;
        }
        if (player != null)
        {
            if (player.playerCombatManager.currentTarget != null)
            {
                cameraPivotTransform.transform.localPosition = newLockedOnCameraHeight;
                cameraPivotTransform.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                cameraPivotTransform.transform.localPosition = newUnlockedCameraHeight;
            }
        }
        yield return null;
    }
    public IEnumerator WaitThenFindNewTarget()
    {
        while (player.isPerformingAction)
        {
            yield return null;
        }
        ClearLockOnTargets();

        yield return null;
    }

}
