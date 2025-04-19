using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Blocked Damage")]
public class TakeBlockedDamageEffect : InstantCharacterEffects
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
    private int finalDamageDealt = 0;

    [Header("Poise")]
    public float poiseDamage = 0;
    public bool poiseIsBroken = false;

    [Header("Stamina")]
    public float staminaDamage = 0;
    public float finalStaminaDamage = 0;

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

        Debug.Log("Blocked");

        if (character.isDead.Value)
            return;

        CalculateDamage(character);
        CalculateStaminaDamage(character);
        PlayDirectionalBasedDamageAnimation(character);
        PlayDamageVFX(character);
        PlayDamageSFX(character);

        CheckForGuardBreak(character);
    }

    private void CalculateDamage(CharacterManager character)
    {
        if (characterCausingDamage != null)
        {

        }

        Debug.Log("Physical Damage: " + physicalDamage);

        physicalDamage -= (physicalDamage * (character.characterStatsManager.blockingPhysicalAbsorption / 100));
        magicDamage -= (magicDamage * (character.characterStatsManager.blockingMagicAbsorption / 100));
        fireDamage -= (fireDamage * (character.characterStatsManager.blockingFireAbsorption / 100));
        lightningDamage -= (lightningDamage * (character.characterStatsManager.blockingLightningAbsorption / 100));
        holyDamage -= (holyDamage * (character.characterStatsManager.blockingHolyAbsorption / 100));

        finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

        if (finalDamageDealt <= 0)
        {
            finalDamageDealt = 1;
        }

        Debug.Log("Character is taking: " + physicalDamage + " damage.");
        character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;

    }

    private void CalculateStaminaDamage(CharacterManager character)
    {
        if (!character.IsOwner)
            return;

        finalStaminaDamage = staminaDamage;

        float staminaDamageAbsorption = finalStaminaDamage * (character.characterStatsManager.blockingStability / 100);
        float staminaDamageAfterAbsorption = finalStaminaDamage - staminaDamageAbsorption;

        character.characterNetworkManager.currentStamina.Value -= staminaDamageAfterAbsorption;
    }

    private void CheckForGuardBreak(CharacterManager character)
    {
        if (!character.IsOwner)
            return;

        if(character.characterNetworkManager.currentStamina.Value <= 0)
        {
            character.characterAnimatorManager.PlayTargetActionAnimation("Guard_Break_01", true);
            character.characterNetworkManager.isBlocking.Value = false;
        }
    }

    private void PlayDamageVFX(CharacterManager character)
    {
        //
    }

    private void PlayDamageSFX(CharacterManager character)
    {
        character.characterSoundFXManager.PlayBlockSoundFX();
    }

    private void PlayDirectionalBasedDamageAnimation(CharacterManager character)
    {
        if (!character.IsOwner)
            return;

        if (character.isDead.Value)
            return;

        DamageIntensity damageIntensity = WorldUtilityManager.instance.GetDamageIntensityBasedOnPoiseDamage(poiseDamage);

        switch (damageIntensity)
        {
            case DamageIntensity.Ping:
                damageAnimation = "Block_01";
                break;
            case DamageIntensity.Light:
                damageAnimation = "Block_01";
                break;
            case DamageIntensity.Medium:
                damageAnimation = "Block_02";
                break;
            case DamageIntensity.Heavy:
                damageAnimation = "Block_02";
                break;
        }
        character.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;
        character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, false);
    }
}
