using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

[RequireComponent(typeof(NetworkTransform))]
public class CharacterNetworkManager : NetworkBehaviour
{
    CharacterManager character;
    private NetworkTransform networkTransform;
    [Header("Position")]
    public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>
        (Vector3.zero,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>
        (Quaternion.identity,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    public Vector3 networkPositionVelocity;
    public float networkPositionSmoothTime = 0.1f;
    public float networkRotationSmoothTime = 0.1f;

    [Header("Animator")]
    public NetworkVariable<float> verticalMovement = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> horizontalMovement = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> moveAmount = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Flags")]
    public NetworkVariable<bool> isSprinting = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    protected virtual void Awake()
    {
        networkTransform = GetComponent<NetworkTransform>();
        character = GetComponent<CharacterManager>();
    }
    [Rpc(SendTo.Server)]
    public void NotifyServerOfActionAnimationRpc(ulong clientID, string animationID)
    {
        //if is host
        if (IsServer)
        {
            PlayActionAnimationForAllClientsRpc(clientID, animationID);
        }
    }

    [Rpc(SendTo.NotServer)]
    public void PlayActionAnimationForAllClientsRpc(ulong clientID, string animationID)
    {
        PerformActionAnimationFromServer(animationID);
        //doesn't run the animation on the client that sent it
        /*
        if (clientID != NetworkManager.Singleton.LocalClientId)
        {
            PerformActionAnimationFromServer(animationID);
        }
        */
    }

    private void PerformActionAnimationFromServer(string animationID)
    {

        character.animator.CrossFade(animationID, 0.2f);
    }
}


