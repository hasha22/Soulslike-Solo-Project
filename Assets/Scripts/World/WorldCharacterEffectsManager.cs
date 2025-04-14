using System.Collections.Generic;
using UnityEngine;

public class WorldCharacterEffectsManager : MonoBehaviour
{
    public static WorldCharacterEffectsManager instance { get; private set; }

    [Header("Damage")]
    public TakeHealthDamage takeHealthDamageEffect;

    [SerializeField] List<InstantCharacterEffect> instantEffects;
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
        GenerateEffectIDs();
    }
    private void GenerateEffectIDs()
    {
        for (int i = 0; i < instantEffects.Count; i++)
        {
            instantEffects[i].instantEffectID = i;
        }
    }
}
