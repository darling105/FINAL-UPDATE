using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumables/Flask")]
public class FlaskItem : QuickSlotItem
{
    [Header("Flask Type")]
    public bool healthFlask = true;

    [Header("Restoration Value")]
    [SerializeField] int flaskRestoration = 50;

    [Header("Empty Item")]
    public GameObject emptyFlaskItem;
    public string emptyFlaskAnimation;

    public override bool CanIUseThisItem(PlayerManager player)
    {
        if (!player.playerCombatManager.isUsingItem && player.isPerformingAction)
            return false;

        if (player.playerNetworkManager.isAttacking.Value)
            return false;



        return true;
    }

    public override void AttemptToUseItem(PlayerManager player)
    {
        if (!CanIUseThisItem(player))
            return;

        if (healthFlask && player.playerNetworkManager.remainingHealthFlasks.Value <= 0)
        {
            if (player.playerCombatManager.isUsingItem)
                return;

            player.playerCombatManager.isUsingItem = true;

            if (player.IsOwner)
            {
                player.playerAnimatorManager.PlayTargetActionAnimation(emptyFlaskAnimation, false, false, true, true, false);
                player.playerNetworkManager.HideWeaponsServerRpc();
            }

            Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
            GameObject emptyFlask = Instantiate(emptyFlaskItem, player.playerEquipmentManager.rightHandWeaponSlot.transform);
            player.playerEffectsManager.activeQuickSlotItemFX = emptyFlask;
            return;

        }

        if (!healthFlask && player.playerNetworkManager.remainingFocusPointFlasks.Value <= 0)
        {
            if (player.playerCombatManager.isUsingItem)
                return;

            player.playerCombatManager.isUsingItem = true;

            if (player.IsOwner)
            {
                player.playerAnimatorManager.PlayTargetActionAnimation(emptyFlaskAnimation, false, false, true, true, false);
                player.playerNetworkManager.HideWeaponsServerRpc();
            }

            Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
            GameObject emptyFlask = Instantiate(emptyFlaskItem, player.playerEquipmentManager.rightHandWeaponSlot.transform);
            player.playerEffectsManager.activeQuickSlotItemFX = emptyFlask;
            return;
        }

        if (player.playerCombatManager.isUsingItem)
        {
            if (player.IsOwner)
                player.playerNetworkManager.isChugging.Value = true;

            return;
        }

        player.playerCombatManager.isUsingItem = true;

        player.playerEffectsManager.activeQuickSlotItemFX = Instantiate(itemModel, player.playerEquipmentManager.rightHandWeaponSlot.transform);

        if (player.IsOwner)
        {
            player.playerAnimatorManager.PlayTargetActionAnimation(useItemAnimation, false, false, true, true, false);
            player.playerNetworkManager.HideWeaponsServerRpc();
        }
    }

    public override void SuccessfullyUsedItem(PlayerManager player)
    {
        base.SuccessfullyUsedItem(player);

        if (player.IsOwner)
        {
            if (healthFlask)
            {
                player.playerNetworkManager.currentHealth.Value += flaskRestoration;
                player.playerNetworkManager.remainingHealthFlasks.Value -= 1;
            }
            else
            {
                player.playerNetworkManager.currentFocusPoints.Value += flaskRestoration;
                player.playerNetworkManager.remainingFocusPointFlasks.Value -= 1;
            }

            PlayerUIManager.instance.playerUIHudManager.SetQuickSlotItemQuickSlotIcon(player.playerInventoryManager.currentQuickSlotItem);
        }

        if (healthFlask && player.playerNetworkManager.remainingHealthFlasks.Value <= 0)
        {
            Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
            GameObject emptyFlask = Instantiate(emptyFlaskItem, player.playerEquipmentManager.rightHandWeaponSlot.transform);
            player.playerEffectsManager.activeQuickSlotItemFX = emptyFlask;
        }
        if (!healthFlask && player.playerNetworkManager.remainingFocusPointFlasks.Value <= 0)
        {
            Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
            GameObject emptyFlask = Instantiate(emptyFlaskItem, player.playerEquipmentManager.rightHandWeaponSlot.transform);
            player.playerEffectsManager.activeQuickSlotItemFX = emptyFlask;
        }

        PlayHealingFX(player);
    }

    private void PlayHealingFX(PlayerManager player)
    {
        Instantiate(WorldCharacterEffectsManager.instance.healingFlaskVFX, player.transform);
        player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.healingFlaskSFX);
    }

    public override int GetCurrentAmount(PlayerManager player)
    {
        int currentAmount = 0;

        if (healthFlask)
            currentAmount = player.playerNetworkManager.remainingHealthFlasks.Value;

        if (!healthFlask)
            currentAmount = player.playerNetworkManager.remainingFocusPointFlasks.Value;

        return currentAmount;
    }

}
