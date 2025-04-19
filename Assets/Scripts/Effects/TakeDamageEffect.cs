using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damge")]
public class TakeDamageEffect : InstantCharacterEffects
{
    [Header("Character Causing Damage")]
    public CharacterManager characterCausingDamage;

    [Header("Damage")]
    public float physicalDamage = 0;
    public float magicDamage = 0;
    public float fireDamage = 0;
    public float lightningDamage = 0;
    public float holyDamage = 0;

    [Header("Final Damage")]
    protected int finalDamageDealt = 0;

    [Header("Poise")]
    public float poiseDamage = 0;
    public bool poiseIsBroken = false;

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public bool manuallySelectDamageAnimation = false;
    public string damageAnimation;

    [Header("Sound FX")]
    public bool willPlayDamageSFX = true;
    public AudioClip elementalDamageSoundFX;

    [Header("Direction Damage Taken From")]
    public float angleHitFrom;
    public Vector3 contactPoint;


    public override void ProcessEffect(CharacterManager character)
    {
        if (character.characterNetworkManager.isInvulnerable.Value)
            return;

        base.ProcessEffect(character);

        if (character.isDead.Value)
            return;

        CalculateDamage(character);
        PlayDirectionalBasedDamageAnimation(character);
        PlayDamageVFX(character);
        PlayDamageSFX(character);
        CalculateStanceDamage(character);
    }

    protected virtual void CalculateDamage(CharacterManager character)
    {
        if (!character.IsOwner)
            return;
        if (characterCausingDamage != null)
        {

        }

        finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

        if (finalDamageDealt <= 0)
        {
            finalDamageDealt = 1;
        }

        character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;

        character.characterStatsManager.totalPoiseDamage -= poiseDamage;
        character.characterCombatManager.previousPoiseDamageTaken = poiseDamage;

        float remainingPoise = character.characterStatsManager.basePoiseDefense +
            character.characterStatsManager.offensivePoiseDamage +
            character.characterStatsManager.totalPoiseDamage;

        if (remainingPoise <= 0)
            poiseIsBroken = true;

        character.characterStatsManager.poiseResetTimer = character.characterStatsManager.defaultPoiseResetTime;
    }

    protected virtual void CalculateStanceDamage(CharacterManager character)
    {
        AICharacterManager aiCharacter = character as AICharacterManager;

        int stanceDamage = Mathf.RoundToInt(poiseDamage);

        if (aiCharacter != null)
        {
            aiCharacter.aiCharacterCombatManager.DamageStance(stanceDamage);
        }
    }

    protected virtual void PlayDamageVFX(CharacterManager character)
    {
        character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
    }

    protected virtual void PlayDamageSFX(CharacterManager character)
    {
        AudioClip physicalDamageSFX = WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.physicalDamageSFX);

        character.characterSoundFXManager.PlaySoundFX(physicalDamageSFX);
        character.characterSoundFXManager.PlayDamageGruntSoundFX();
    }

    protected virtual void PlayDirectionalBasedDamageAnimation(CharacterManager character)
    {
        if (!character.IsOwner)
            return;

        if (character.isDead.Value)
            return;

        if (poiseIsBroken)
        {
            if (angleHitFrom >= 145 && angleHitFrom <= 180)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
            }
            else if (angleHitFrom <= -145 && angleHitFrom >= -180)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
            }
            else if (angleHitFrom >= -45 && angleHitFrom <= 45)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.backward_Medium_Damage);
            }
            else if (angleHitFrom >= -144 && angleHitFrom <= -45)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.left_Medium_Damage);
            }
            else if (angleHitFrom >= 45 && angleHitFrom <= 144)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.right_Medium_Damage);
            }
        }
        else
        {
            if (angleHitFrom >= 145 && angleHitFrom <= 180)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Ping_Damage);
            }
            else if (angleHitFrom <= -145 && angleHitFrom >= -180)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Ping_Damage);
            }
            else if (angleHitFrom >= -45 && angleHitFrom <= 45)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.backward_Ping_Damage);
            }
            else if (angleHitFrom >= -144 && angleHitFrom <= -45)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.left_Ping_Damage);
            }
            else if (angleHitFrom >= 45 && angleHitFrom <= 144)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.right_Ping_Damage);
            }
        }

        character.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;

        if (poiseIsBroken)
        {
            character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
            character.characterCombatManager.DestoryAllCurrentActionsFX();
        }
        else
        {
            character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, false, false, true, true);
        }
    }
}
