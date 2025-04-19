using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Action/Off Hand Melee Action")]
public class OffHandMeleeActions : WeaponItemAction
{
    public override void AttempToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttempToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.playerCombatManager.canBlock)
            return;

        if(playerPerformingAction.playerCombatManager.isUsingItem)
            return;

        if (playerPerformingAction.playerNetworkManager.isAttacking.Value)
        {
            if (playerPerformingAction.IsOwner)
                playerPerformingAction.playerNetworkManager.isAttacking.Value = false;

            return;
        }
        
        if (playerPerformingAction.playerNetworkManager.isBlocking.Value)
            return;

        if (playerPerformingAction.IsOwner)
            playerPerformingAction.playerNetworkManager.isBlocking.Value = true;
    }
}
