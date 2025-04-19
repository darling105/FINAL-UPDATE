using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISkeletonCombatManager : AICharacterCombatManager
{
    
    [Header("Sword Damage Collider")]
    [SerializeField] SkeletonSwordDamageCollider swordDamageCollider;

    [Header("Damage")]
    [SerializeField] int baseDamage = 20;
    [SerializeField] int basePoiseDamage = 20;
    [SerializeField] float attack01DamageModifier = 1.0f;
    [SerializeField] float attack02DamageModifier = 1.2f;

    public void SetAttack01Damage()
    {
        swordDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
        swordDamageCollider.poiseDamage = basePoiseDamage * attack01DamageModifier;
    }

    public void SetAttack02Damage()
    {
        swordDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
        swordDamageCollider.poiseDamage = basePoiseDamage * attack02DamageModifier;
    }

    public void OpenSwordCollider()
    {
        aiCharacter.characterSoundFXManager.PlayAttackGruntSoundFX();
        swordDamageCollider.EnableDamageCollider();
    }

    public void CloseSwordCollider()
    {
        swordDamageCollider.DisableDamageCollider();
    }

    public override void CloseAllDamagaCollider()
    {
        base.CloseAllDamagaCollider();

        swordDamageCollider.DisableDamageCollider();
    }

}
