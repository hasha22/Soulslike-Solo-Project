using UnityEngine;

public class PlayMainMenuSFX : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayStartButtonSFX()
    {
        audioSource.PlayOneShot(TitleScreenSFX.instance.pressStartSFX);
    }
}
