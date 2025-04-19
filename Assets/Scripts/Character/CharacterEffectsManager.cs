using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Current Active FX")]
    public GameObject activeQuickSlotItemFX;

    [Header("VFX")]
    [SerializeField] GameObject bloodSplatterVFX;
    [SerializeField] GameObject criticalbloodSplatterVFX;

    [Header("Static Effects")]
    public List<StaticCharacterEffects> staticEffects = new List<StaticCharacterEffects>();

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public virtual void ProcessInstantEffect(InstantCharacterEffects effect)
    {
        effect.ProcessEffect(character);
    }

    public void PlayBloodSplatterVFX(Vector3 contactPoint)
    {
        if (bloodSplatterVFX != null)
        {
            GameObject bloodSplatter = Instantiate(bloodSplatterVFX, contactPoint, Quaternion.identity);
        }
        else
        {
            GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.instance.bloodSplatterVFX, contactPoint, Quaternion.identity);
        }
    }

     public void PlayCriticalBloodSplatterVFX(Vector3 contactPoint)
    {
        if (bloodSplatterVFX != null)
        {
            GameObject bloodSplatter = Instantiate(criticalbloodSplatterVFX, contactPoint, Quaternion.identity);
        }
        else
        {
            GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.instance.criticalBloodSplatterVFX, contactPoint, Quaternion.identity);
        }
    }

    public void AddStaticEffect(StaticCharacterEffects effect)
    {
        staticEffects.Add(effect);

        effect.ProcessStaticEffect(character);

        for (int i = staticEffects.Count - 1; i > -1; i--)
        {
            if (staticEffects[i] == null)
                staticEffects.RemoveAt(i);

        }
    }

    public void RemoveStaticEffect(int effectID)
    {
        StaticCharacterEffects effect;

        for (int i = 0; i < staticEffects.Count; i++)
        {
            if (staticEffects[i] !=null)
            {
                if(staticEffects[i].staticEffectID == effectID)
                {
                    effect = staticEffects[i];
                    effect.RemoveStaticEffect(character);
                    staticEffects.Remove(effect);
                }
            }
        }

        for (int i = staticEffects.Count - 1; i > -1; i--)
        {
            if (staticEffects[i] == null)
                staticEffects.RemoveAt(i);

        }
    }

}
