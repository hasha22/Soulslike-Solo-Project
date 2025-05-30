using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    CharacterManager character;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
    public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
    {
        effect.ProcessEffect(character);
    }
    public void PlayBloodSpatterVFX(Vector3 contactPoint)
    {
        GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.instance.bloodSpatterVFX, contactPoint, Quaternion.identity);
    }
    public void PlayBoneShatterVFX(Vector3 contactPoint)
    {
        GameObject boneShatter = Instantiate(WorldCharacterEffectsManager.instance.boneShatterVFX, contactPoint, Quaternion.identity);
    }
}
