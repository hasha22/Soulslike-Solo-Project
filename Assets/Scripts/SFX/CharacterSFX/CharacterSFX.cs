using UnityEngine;

public class CharacterSFX : MonoBehaviour
{
    private AudioSource audioSource;
    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRollSoundFX()
    {
        audioSource.PlayOneShot(WorldSFXManager.instance.rollingSFX);
    }
    public void PlayFootstepSFX()
    {
        int index = Random.Range(0, WorldSFXManager.instance.footstepClips.Length);
        audioSource.PlayOneShot(WorldSFXManager.instance.footstepClips[index]);
    }

    public void PlayWalkSFX()
    {
        //int index = Random.Range(0, WorldSoundFXManager.instance.walkingClips.Length);
        //audioSource.PlayOneShot(WorldSoundFXManager.instance.walkingClips[index]);
    }
    public void PlayBackstepSFX()
    {

    }
}
