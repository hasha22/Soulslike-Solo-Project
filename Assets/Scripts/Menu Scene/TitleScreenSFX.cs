using UnityEngine;

public class TitleScreenSFX : MonoBehaviour
{
    public static TitleScreenSFX instance;

    [Header("Title Screen SFX")]
    [SerializeField] public AudioClip titleScreenMusic;
    [SerializeField] public AudioClip pressStartSFX;
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
