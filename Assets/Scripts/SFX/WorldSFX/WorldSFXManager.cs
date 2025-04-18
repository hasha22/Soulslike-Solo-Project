using UnityEngine;

public class WorldSFXManager : MonoBehaviour
{
    public static WorldSFXManager instance { get; private set; }

    [Header("Player Action Sounds")]
    [SerializeField] public AudioClip rollingSFX;
    [SerializeField] public AudioClip[] footstepClips;
    [SerializeField] public AudioClip[] walkingClips;
    [SerializeField] public AudioClip[] backstepClips;

    //[Header("Character Action Sounds")]

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
    }
}
