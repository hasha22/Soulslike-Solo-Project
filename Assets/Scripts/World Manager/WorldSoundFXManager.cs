using UnityEngine;

public class WorldSoundFXManager : MonoBehaviour
{
    public static WorldSoundFXManager instance;

    [Header("Action Sounds")]
    [SerializeField] public AudioClip rollingSFX;
    [SerializeField] public AudioClip[] footstepClips;
    [SerializeField] public AudioClip[] walkingClips;
    [SerializeField] public AudioClip[] backstepClips;

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
