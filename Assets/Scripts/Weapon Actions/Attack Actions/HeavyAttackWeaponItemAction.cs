using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Action/Heavy Attack Action")]
public class HeavyAttackWeaponItemAction : WeaponItemAction
{
    [SerializeField] string heavy_Attack_01 = "Main_Heavy_Attack_01";
    [SerializeField] string heavy_Attack_02 = "Main_Heavy_Attack_02";
    [SerializeField] string heavy_Jumping_Attack_01 = "Main_Heavy_Jump_Attack_01";

    [SerializeField] string th_heavy_Attack_01 = "TH_Heavy_Attack_01";
    [SerializeField] string th_heavy_Attack_02 = "TH_Heavy_Attack_02";
    [SerializeField] string th_heavy_Jumping_Attack_01 = "TH_Heavy_Jump_Attack_01";
    public override void AttempToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttempToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
            return;

        if (!playerPerformingAction.playerLocomotionManager.isGrounded)
        {
            PerformJumpingHeavyAttack(playerPerformingAction, weaponPerformingAction);
            return;
        }

        if (playerPerformingAction.playerNetworkManager.isJumping.Value)
            return;

        PerformHeavyAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
        {
            PerformTwoHandHeavyAttack(playerPerformingAction, weaponPerformingAction);
        }
        else
        {
            PerformMainHandHeavyAttack(playerPerformingAction, weaponPerformingAction);
        }
    }

    private void PerformMainHandHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;

            if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == heavy_Attack_01)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack02, heavy_Attack_02, true);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, heavy_Attack_01, true);
            }
        }
        else if (!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, heavy_Attack_01, true);
        }
    }

    private void PerformTwoHandHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;

            if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == th_heavy_Attack_01)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack02, th_heavy_Attack_02, true);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, th_heavy_Attack_01, true);
            }
        }
        else if (!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, th_heavy_Attack_01, true);
        }
    }

    private void PerformJumpingHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
        {
            PerformTwoHandJumpingHeavyAttak(playerPerformingAction, weaponPerformingAction);
        }
        else
        {
            PerformMainHandJumpingHeavyAttak(playerPerformingAction, weaponPerformingAction);
        }
    }



    private void PerformMainHandJumpingHeavyAttak(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.isPerformingAction)
            return;

        playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyJumpingAttack01, heavy_Jumping_Attack_01, true);
    }

    private void PerformTwoHandJumpingHeavyAttak(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.isPerformingAction)
            return;

        playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyJumpingAttack01, th_heavy_Jumping_Attack_01, true);
    }

}
