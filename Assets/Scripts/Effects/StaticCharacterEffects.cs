using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCharacterEffects : ScriptableObject
{
    [Header("Effect ID")]
    public int staticEffectID =0;

    public virtual void ProcessStaticEffect(CharacterManager character)
    {
        
    }

    public virtual void RemoveStaticEffect(CharacterManager character)
    {
        
    }
}
