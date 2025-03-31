using UnityEngine;

public class MainMenuSFX : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Title Screen Sounds")]
    [SerializeField] public AudioClip titleScreenMusic;
    [SerializeField] public AudioClip pressStartSFX;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayStartButtonSFX()
    {
        audioSource.PlayOneShot(pressStartSFX);
    }
    public void PlayMainMenuMusic()
    {
        audioSource.clip = titleScreenMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
}
