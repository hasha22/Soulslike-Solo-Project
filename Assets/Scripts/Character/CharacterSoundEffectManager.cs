using UnityEngine;

public class CharacterSoundEffectManager : MonoBehaviour
{
    private AudioSource audioSource;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRollSoundFX()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.rollingSFX);
    }
    public void PlayFootstepSFX()
    {
        int index = Random.Range(0, WorldSoundFXManager.instance.footstepClips.Length);
        audioSource.PlayOneShot(WorldSoundFXManager.instance.footstepClips[index]);
    }

    // PlayWalkSFX and PlayBackstepSFX currently not working as intended
    public void PlayWalkSFX()
    {
        //int index = Random.Range(0, WorldSoundFXManager.instance.walkingClips.Length);
        //audioSource.PlayOneShot(WorldSoundFXManager.instance.walkingClips[index]);
    }
    public void PlayBackstepSFX()
    {

    }

}
