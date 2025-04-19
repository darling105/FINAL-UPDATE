using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSwordDamageCollider : DamageCollider
{
    [SerializeField] AICharacterManager skeletonCharacter;

    protected override void Awake()
    {
        base.Awake();

        damageCollider = GetComponent<Collider>();
        skeletonCharacter = GetComponentInParent<AICharacterManager>();
    }

    protected override void GetBlockedDotValue(CharacterManager damageTarget)
    {
        directionFromAttackToDamageTarget = skeletonCharacter.transform.position - damageTarget.transform.position;
        dotValueFromAttackToDamageTarget = Vector3.Dot(directionFromAttackToDamageTarget, damageTarget.transform.forward);
    }

    protected override void DamageTarget(CharacterManager damageTarget)
    {
        if (damageTarget == skeletonCharacter)
            return;
        if (charactersDamaged.Contains(damageTarget))
            return;

        charactersDamaged.Add(damageTarget);

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
        damageEffect.physicalDamage = physicalDamage;
        damageEffect.magicDamage = magicDamage;
        damageEffect.fireDamage = fireDamage;
        damageEffect.lightningDamage = lightningDamage;
        damageEffect.holyDamage = holyDamage;
        damageEffect.contactPoint = contactPoint;
        damageEffect.angleHitFrom = Vector3.SignedAngle(skeletonCharacter.transform.forward, damageTarget.transform.forward, Vector3.up);


        if (damageTarget.IsOwner)
        {
            damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(
                damageTarget.NetworkObjectId,
                skeletonCharacter.NetworkObjectId,
                damageEffect.physicalDamage,
                damageEffect.magicDamage,
                damageEffect.fireDamage,
                damageEffect.lightningDamage,
                damageEffect.holyDamage,
                damageEffect.poiseDamage,
                damageEffect.angleHitFrom,
                damageEffect.contactPoint.x,
                damageEffect.contactPoint.y,
                damageEffect.contactPoint.z);
        }
    }

    protected override void CheckForParry(CharacterManager damageTarget)
    {
        if (charactersDamaged.Contains(damageTarget))
            return;

        if (skeletonCharacter.characterNetworkManager.isParryable.Value)
            return;

        if (!damageTarget.IsOwner)
            return;

        if(damageTarget.characterNetworkManager.isParrying.Value)
        {
            charactersDamaged.Add(damageTarget);
            damageTarget.characterNetworkManager.NotifyTheServerOfParryServerRpc(skeletonCharacter.NetworkObjectId);
            damageTarget.characterAnimatorManager.PlayTargetActionAnimationInstantly("Parried_01", true);
        }
    }


}
