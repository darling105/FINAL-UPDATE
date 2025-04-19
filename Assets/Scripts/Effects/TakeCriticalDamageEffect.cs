using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Critical Damage Effect")]
public class TakeCriticalDamageEffect : TakeDamageEffect
{
    public override void ProcessEffect(CharacterManager character)
    {
        if (character.characterNetworkManager.isInvulnerable.Value)
            return;

        if (character.isDead.Value)
            return;

        CalculateDamage(character);

        character.characterCombatManager.pendingCriticalDamage = finalDamageDealt;
    }

    protected override void CalculateDamage(CharacterManager character)
    {
        if (!character.IsOwner)
            return;
        if (characterCausingDamage != null)
        {

        }

        finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

        if (finalDamageDealt <= 0)
            finalDamageDealt = 1;

        

        character.characterStatsManager.totalPoiseDamage -= poiseDamage;
        character.characterCombatManager.previousPoiseDamageTaken = poiseDamage;

        float remainingPoise = character.characterStatsManager.basePoiseDefense +
            character.characterStatsManager.offensivePoiseDamage +
            character.characterStatsManager.totalPoiseDamage;

        if (remainingPoise <= 0)
            poiseIsBroken = true;

        character.characterStatsManager.poiseResetTimer = character.characterStatsManager.defaultPoiseResetTime;
    }
}
