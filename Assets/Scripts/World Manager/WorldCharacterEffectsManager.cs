using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCharacterEffectsManager : MonoBehaviour
{
    public static WorldCharacterEffectsManager instance;

    [Header("Damage")]
    public TakeDamageEffect takeDamageEffect;
    public TakeBlockedDamageEffect takeBlockedDamageEffect;
    public TakeCriticalDamageEffect takeCriticalDamageEffect;

    [Header("Two Handing")]
    public TwoHandingEffect twoHandingEffect;

    [Header("Instant Effects")]
    [SerializeField] List<InstantCharacterEffects> instantEffects;

    [Header("Static Effects")]
    [SerializeField] List<StaticCharacterEffects> staticEffects;


    [Header("VFX")]
    public GameObject bloodSplatterVFX;
    public GameObject criticalBloodSplatterVFX;
    public GameObject healingFlaskVFX;

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
        for (int i = 0; i < staticEffects.Count; i++)
        {
            staticEffects[i].staticEffectID = i;
        }
    }

}
