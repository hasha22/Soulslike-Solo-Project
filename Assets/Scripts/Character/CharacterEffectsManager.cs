using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    CharacterManager character;

    [Header("VFX")]
    [SerializeField] GameObject bloodSpatterVFX;
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
        if (bloodSpatterVFX != null)
        {
            GameObject bloodSplatter = Instantiate(bloodSpatterVFX, contactPoint, Quaternion.identity);
        }
        else
        {
            GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.instance.bloodSpatterVFX, contactPoint, Quaternion.identity);
        }
    }
}
