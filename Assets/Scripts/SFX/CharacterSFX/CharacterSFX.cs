using UnityEngine;

public class CharacterSFX : MonoBehaviour
{
    private AudioSource audioSource;
    [HideInInspector] public CharacterManager character;
    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        character = GetComponent<CharacterManager>();
    }

    public void PlayRollSoundFX()
    {
        audioSource.PlayOneShot(WorldSFXManager.instance.rollingSFX);
    }
    public void PlayFootstepSFX()
    {
        if (!character.isPerformingAction)
        {
            if (character.tag == "Player")
            {
                int index = Random.Range(0, WorldSFXManager.instance.playerFootstepClips.Length);
                audioSource.PlayOneShot(WorldSFXManager.instance.playerFootstepClips[index]);
            }
            else if (character.tag == "Skeleton")
            {
                int index = Random.Range(0, WorldSFXManager.instance.skeletonFootstepClips.Length);
                audioSource.PlayOneShot(WorldSFXManager.instance.skeletonFootstepClips[index], 0.5f);
            }
        }
    }
    public void PlaySoundFX(AudioClip soundFX, float volume = 1, bool randomizePitch = true, float pitchRandom = 0.1f)
    {
        audioSource.pitch = 1;
        audioSource.spatialBlend = 0f;
        if (randomizePitch)
        {
            audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
        }
        audioSource.PlayOneShot(soundFX, volume);
    }
    public void PlayWhooshSoundFX(int index)
    {
        AudioClip whooshToPlay = WorldSFXManager.instance.swordWhooshes[index];
        audioSource.PlayOneShot(whooshToPlay, 0.5f);
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
