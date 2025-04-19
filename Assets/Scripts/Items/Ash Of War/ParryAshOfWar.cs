using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Ash Of War/Parry")]
public class ParryAshOfWar : AshOfWar
{
    public override void AttemptToPerformAction(PlayerManager playerPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction);

        if (!CanIUseThisAbility(playerPerformingAction))
            return;

        DeductStaminaCost(playerPerformingAction);
        DeductFocusPointCost(playerPerformingAction);
        PerformParryTypeBasedOnWeapon(playerPerformingAction);
    }

    public override bool CanIUseThisAbility(PlayerManager playerPerformingAction)
    {
        if (playerPerformingAction.isPerformingAction)
        {
            Debug.Log("Cannot perform this action while performing another action.");
            return false;
        }
        if (playerPerformingAction.playerNetworkManager.isJumping.Value)
        {
            Debug.Log("Cannot perform this action while jumping.");
            return false;
        }
        if (!playerPerformingAction.playerLocomotionManager.isGrounded)
        {
            Debug.Log("Cannot perform this action while not grounded.");
            return false;
        }
        if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
        {
            Debug.Log("Not enough stamina to perform this action.");
            return false;
        }

        return true;

    }

    private void PerformParryTypeBasedOnWeapon(PlayerManager playerPerformingAction)
    {
        WeaponItem weaponBeingUsed = playerPerformingAction.playerCombatManager.currentWeaponBeingUsed;

        switch (weaponBeingUsed.weaponClass)
        {
            case WeaponClass.StraightSword:
                break;
            case WeaponClass.Shield:
                playerPerformingAction.playerAnimatorManager.PlayTargetActionAnimation("Parry_01", true);
                break;
            case WeaponClass.SmallShield:
                playerPerformingAction.playerAnimatorManager.PlayTargetActionAnimation("Parry_01", true);
                break;
            case WeaponClass.Fist:
                break;
            default:
                break;
        }
    }

}
