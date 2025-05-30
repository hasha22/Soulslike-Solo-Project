using UnityEngine;

public class WorldSFXManager : MonoBehaviour
{
    public static WorldSFXManager instance { get; private set; }

    [Header("Player Action Sounds")]
    [SerializeField] public AudioClip rollingSFX;
    [SerializeField] public AudioClip[] playerFootstepClips;
    [SerializeField] public AudioClip[] walkingClips;
    [SerializeField] public AudioClip[] backstepClips;

    [Header("Enemy Action Sounds")]
    [SerializeField] public AudioClip[] skeletonFootstepClips;

    [Header("Damage Sounds")]
    [SerializeField] public AudioClip[] bloodSpillSFX;
    [SerializeField] public AudioClip[] boneShatterSFX;

    //[Header("Character Action Sounds")]

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public AudioClip ChooseRandomSFX(AudioClip[] array)
    {
        int index = Random.Range(0, array.Length);
        return array[index];
    }
}
