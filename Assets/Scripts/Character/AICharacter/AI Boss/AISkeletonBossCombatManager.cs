using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISkeletonBossCombatManager : AICharacterCombatManager
{
    AISkeletonBossCharacterManager skeletonBossManager;

    [Header("Sword Damage Collider")]
    [SerializeField] SkeletonBossClubDamageCollider clubDamageCollider;

    [Header("Damage")]
    [SerializeField] int baseDamage = 50;
    [SerializeField] int basePoiseDamage = 50;
    [SerializeField] float attack01DamageModifier = 1.0f;
    [SerializeField] float attack02DamageModifier = 1.2f;
    [SerializeField] float attack03DamageModifier = 1.5f;

    protected override void Awake()
    {
        base.Awake();
        skeletonBossManager = GetComponent<AISkeletonBossCharacterManager>();
    }

    public void SetAttack01Damage()
    {
        aiCharacter.characterSoundFXManager.PlayAttackGruntSoundFX();
        clubDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
        clubDamageCollider.poiseDamage = basePoiseDamage * attack01DamageModifier;
    }

    public void SetAttack02Damage()
    {
        aiCharacter.characterSoundFXManager.PlayAttackGruntSoundFX();
        clubDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
        clubDamageCollider.poiseDamage = basePoiseDamage * attack02DamageModifier;
    }

    public void SetAttack03Damage()
    {
        aiCharacter.characterSoundFXManager.PlayAttackGruntSoundFX();
        clubDamageCollider.physicalDamage = baseDamage * attack03DamageModifier;
        clubDamageCollider.poiseDamage = basePoiseDamage * attack03DamageModifier;
    }

    public void OpenClubDamageCollider()
    {   
        clubDamageCollider.EnableDamageCollider();
        skeletonBossManager.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(skeletonBossManager.skeletonBossSoundFXManager.clubWhooshes));
    }

    public void CloseClubDamageCollider()
    {
        clubDamageCollider.DisableDamageCollider();
    }

    public void ActivateSkeletonBossStomp()
    {

    }

    public override void PivotTowardsTarget(AICharacterManager aiCharacter)
    {
         if (aiCharacter.isPerformingAction)
            return;

        if (viewAbleAngle >= 61 && viewAbleAngle <= 110)
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Right_Turn", true);
        else if (viewAbleAngle <= -61 && viewAbleAngle >= -110)
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Left_Turn", true);
        else if (viewAbleAngle >= 146 && viewAbleAngle <= 180)
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Right_Turn", true);
        else if (viewAbleAngle <= -146 && viewAbleAngle >= -180)
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Left_Turn", true);
    }

}
