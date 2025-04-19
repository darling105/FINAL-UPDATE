using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager player;
    public WeaponItem currentWeaponBeingUsed;

    [Header("Flags")]
    public bool canComboWithMainHandWeapon = false;
    //public bool canComboWithOffhandWeapon = false;
    public bool isUsingItem = false;


    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
    {

        if (player.IsOwner)
        {
            weaponAction.AttempToPerformAction(player, weaponPerformingAction);

            player.playerNetworkManager.NotifyTheServerOfWeaponActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, weaponAction.actionID, weaponPerformingAction.itemID);
        }

    }

    public override void CloseAllDamagaCollider()
    {
        base.CloseAllDamagaCollider();

        player.playerEquipmentManager.rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
        player.playerEquipmentManager.leftWeaponManager.meleeDamageCollider.DisableDamageCollider();
    }

    public override void AttemptRiposte(RaycastHit hit)
    {
        CharacterManager targetCharacter = hit.transform.gameObject.GetComponent<CharacterManager>();

        if (targetCharacter == null)
            return;

        if (!targetCharacter.characterNetworkManager.isRipostable.Value)
            return;

        if (targetCharacter.characterNetworkManager.isBeingCriticallyDamaged.Value)
            return;

        MeleeWeaponItem riposteWeapon;
        MeleeWeaponDamageCollider riposteCollider;

        if (player.playerNetworkManager.isTwoHandingLeftWeapon.Value)
        {
            riposteWeapon = player.playerInventoryManager.currentLeftHandWeapon as MeleeWeaponItem;
            riposteCollider = player.playerEquipmentManager.leftWeaponManager.meleeDamageCollider;
        }
        else
        {
            riposteWeapon = player.playerInventoryManager.currentRightHandWeapon as MeleeWeaponItem;
            riposteCollider = player.playerEquipmentManager.rightWeaponManager.meleeDamageCollider;
        }

        character.characterAnimatorManager.PlayTargetActionAnimationInstantly("Riposte_01", true);

        if (character.IsOwner)
            character.characterNetworkManager.isInvulnerable.Value = true;

        TakeCriticalDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeCriticalDamageEffect);

        damageEffect.physicalDamage = riposteCollider.physicalDamage;
        damageEffect.magicDamage = riposteCollider.magicDamage;
        damageEffect.fireDamage = riposteCollider.fireDamage;
        damageEffect.lightningDamage = riposteCollider.lightningDamage;
        damageEffect.holyDamage = riposteCollider.holyDamage;
        damageEffect.poiseDamage = riposteCollider.poiseDamage;

        damageEffect.physicalDamage *= riposteWeapon.riposte_Attack_01_Modifier;
        damageEffect.magicDamage *= riposteWeapon.riposte_Attack_01_Modifier;
        damageEffect.fireDamage *= riposteWeapon.riposte_Attack_01_Modifier;
        damageEffect.lightningDamage *= riposteWeapon.riposte_Attack_01_Modifier;
        damageEffect.holyDamage *= riposteWeapon.riposte_Attack_01_Modifier;
        damageEffect.poiseDamage *= riposteWeapon.riposte_Attack_01_Modifier;

        targetCharacter.characterNetworkManager.NotifyTheServerOfRiposteServerRpc(
        targetCharacter.NetworkObjectId,
        character.NetworkObjectId,
        "Riposted_01",
        riposteWeapon.itemID,
        damageEffect.physicalDamage,
        damageEffect.magicDamage,
        damageEffect.fireDamage,
        damageEffect.lightningDamage,
        damageEffect.holyDamage,
        damageEffect.poiseDamage);
    }

    public override void AttemptBackstab(RaycastHit hit)
    {
        CharacterManager targetCharacter = hit.transform.gameObject.GetComponent<CharacterManager>();

        if (targetCharacter == null)
            return;

        if (!targetCharacter.characterCombatManager.canBeBackStabbed)
            return;

        if (targetCharacter.characterNetworkManager.isBeingCriticallyDamaged.Value)
            return;

        MeleeWeaponItem backstabWeapon;
        MeleeWeaponDamageCollider backstabCollider;

        if (player.playerNetworkManager.isTwoHandingLeftWeapon.Value)
        {
            backstabWeapon = player.playerInventoryManager.currentLeftHandWeapon as MeleeWeaponItem;
            backstabCollider = player.playerEquipmentManager.leftWeaponManager.meleeDamageCollider;
        }
        else
        {
            backstabWeapon = player.playerInventoryManager.currentRightHandWeapon as MeleeWeaponItem;
            backstabCollider = player.playerEquipmentManager.rightWeaponManager.meleeDamageCollider;
        }

        character.characterAnimatorManager.PlayTargetActionAnimationInstantly("Backstab_01", true);

        if (character.IsOwner)
            character.characterNetworkManager.isInvulnerable.Value = true;

        TakeCriticalDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeCriticalDamageEffect);

        damageEffect.physicalDamage = backstabCollider.physicalDamage;
        damageEffect.magicDamage = backstabCollider.magicDamage;
        damageEffect.fireDamage = backstabCollider.fireDamage;
        damageEffect.lightningDamage = backstabCollider.lightningDamage;
        damageEffect.holyDamage = backstabCollider.holyDamage;
        damageEffect.poiseDamage = backstabCollider.poiseDamage;

        damageEffect.physicalDamage *= backstabWeapon.backstab_Attack_01_Modifier;
        damageEffect.magicDamage *= backstabWeapon.backstab_Attack_01_Modifier;
        damageEffect.fireDamage *= backstabWeapon.backstab_Attack_01_Modifier;
        damageEffect.lightningDamage *= backstabWeapon.backstab_Attack_01_Modifier;
        damageEffect.holyDamage *= backstabWeapon.backstab_Attack_01_Modifier;
        damageEffect.poiseDamage *= backstabWeapon.backstab_Attack_01_Modifier;

        targetCharacter.characterNetworkManager.NotifyTheServerOfBackstabServerRpc(
        targetCharacter.NetworkObjectId,
        character.NetworkObjectId,
        "Backstabbed_01",
        backstabWeapon.itemID,
        damageEffect.physicalDamage,
        damageEffect.magicDamage,
        damageEffect.fireDamage,
        damageEffect.lightningDamage,
        damageEffect.holyDamage,
        damageEffect.poiseDamage);
    }

    public virtual void DrainStaminaBasedOnAttack()
    {
        if (!player.IsOwner)
            return;


        if (currentWeaponBeingUsed == null)
            return;

        float staminaDeducted = 0;


        switch (currentAttackType)
        {
            case AttackType.LightAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaMultiplier;
                break;
            case AttackType.LightAttack02:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaMultiplier;
                break;
            case AttackType.LightJumpingAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaMultiplier;
                break;
            case AttackType.HeavyAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaMultiplier;
                break;
            case AttackType.HeavyAttack02:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaMultiplier;
                break;
            case AttackType.HeavyJumpingAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaMultiplier;
                break;
            case AttackType.ChargedAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargedAttackStaminaMultiplier;
                break;
            case AttackType.ChargedAttack02:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargedAttackStaminaMultiplier;
                break;
            case AttackType.RunningAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.runningAttackStaminaMultiplier;
                break;
            case AttackType.RollingAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.rollingAttackStaminaMultiplier;
                break;
            case AttackType.BackstepAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.backstepAttackStaminaMultiplier;
                break;
            default:
                break;
        }

        player.playerNetworkManager.currentStamina.Value -= Mathf.RoundToInt(staminaDeducted);
    }

    public override void SetTarget(CharacterManager newTarget)
    {
        base.SetTarget(newTarget);

        if (player.IsOwner)
        {
            PlayerCamera.instance.SetLockCameraHeight();
        }
    }

    public override void EnableCanDoCombo()
    {
        if (player.playerNetworkManager.isUsingRightHand.Value)
        {
            player.playerCombatManager.canComboWithMainHandWeapon = true;
        }
        else
        {

        }
    }

    public override void DisableCanDoCombo()
    {
        player.playerCombatManager.canComboWithMainHandWeapon = false;
        // player.playerCombatManager.canComboWithOffhandWeapon = false;
    }

    public void SuccessfullyUseQuickSlotItem()
    {
        if (player.playerInventoryManager.currentQuickSlotItem != null)
            player.playerInventoryManager.currentQuickSlotItem.SuccessfullyUsedItem(player);
    }

    public WeaponItem SelectWeaponToPerformAshOfWar()
    {
        WeaponItem selectedWeapon = player.playerInventoryManager.currentLeftHandWeapon;
        player.playerNetworkManager.SetCharacterActionHand(false);
        player.playerNetworkManager.currentWeaponBeingUsed.Value = selectedWeapon.itemID;
        return selectedWeapon;
    }

}
