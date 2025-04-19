using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damge")]
public class TakeStaminaDamageEffect : InstantCharacterEffects
{
    public float staminaDamage;
    public override void ProcessEffect(CharacterManager character)
    {
        CalculateStaminaDamage(character);
    }

    private void CalculateStaminaDamage(CharacterManager character)
    {
        if (character.IsOwner)
        {
            character.characterNetworkManager.currentStamina.Value -= staminaDamage;
        }
    }
}
