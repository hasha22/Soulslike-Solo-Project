using UnityEngine;

public class PlayMainMenuTheme : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayMainMenuMusic()
    {
        audioSource.clip = TitleScreenSFX.instance.titleScreenMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
}
