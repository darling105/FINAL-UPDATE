using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkManager : CharacterNetworkManager
{
    PlayerManager player;
    public NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>("Darling", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);




    [Header("Flasks")]
    public NetworkVariable<int> remainingHealthFlasks = new NetworkVariable<int>(3, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> remainingFocusPointFlasks = new NetworkVariable<int>(3, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isChugging = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Actions")]
    public NetworkVariable<bool> isUsingRightHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isUsingLeftHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Body")]
    public NetworkVariable<int> hairStyleID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> hairColorRed = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> hairColorGreen = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> hairColorBlue = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Equipment")]
    public NetworkVariable<int> currentWeaponBeingUsed = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentRightHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentLeftHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentQuickSlotItemID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Two Handing")]
    public NetworkVariable<int> currentWeaponBeingTwoHanded = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isTwoHandingWeapon = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isTwoHandingRightWeapon = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isTwoHandingLeftWeapon = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Armor")]
    public NetworkVariable<bool> isMale = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> headEquipmentID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> bodyEquipmentID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> legEquipmentID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> handEquipmentID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    public void SetCharacterActionHand(bool rightHandedAction)
    {
        if (rightHandedAction)
        {
            isUsingRightHand.Value = true;
            isUsingLeftHand.Value = false;
        }
        else
        {
            isUsingRightHand.Value = false;
            isUsingLeftHand.Value = true;
        }
    }

    public void SetNewMaxHealthValue(int oldVitality, int newVitality)
    {
        maxHealth.Value = player.playerStatsManager.CalculateHealhtBasedOnVitalityLevel(newVitality);
        PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(maxHealth.Value);
        currentHealth.Value = maxHealth.Value; // Reset current health to max health when vitality changes
    }

    public void SetNewMaxStaminaValue(int oldEndurance, int newEndurance)
    {
        maxStamina.Value = player.playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(newEndurance);
        PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(maxStamina.Value);
        currentStamina.Value = maxStamina.Value; // Reset current stamina to max stamina when endurance changes
    }

    public void SetNewMaxFocusPointsValue(int oldMind, int newMind)
    {
        maxFocusPoints.Value = player.playerStatsManager.CalculateFocusPointsBasedOnMindLevel(newMind);
        PlayerUIManager.instance.playerUIHudManager.SetMaxFocusPointsValue(maxFocusPoints.Value);
        currentFocusPoints.Value = maxFocusPoints.Value; // Reset current focus points to max focus points when focus points changes
    }

    public void OnHairStyleIDChanged(int oldID, int newID)
    {
        player.playerBodyManager.ToggleHairStyle(hairStyleID.Value);
    }

    public void OnHairColorRedChanged(float oldValue, float newValue)
    {
        player.playerBodyManager.SetHairColor();
    }

    public void OnHairColorGreenChanged(float oldValue, float newValue)
    {
        player.playerBodyManager.SetHairColor();
    }

    public void OnHairColorBlueChanged(float oldValue, float newValue)
    {
        player.playerBodyManager.SetHairColor();
    }

    public void OnCurrentRightHandWeaponIDChange(int oldID, int newID)
    {
        if (!player.IsOwner)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
            player.playerInventoryManager.currentRightHandWeapon = newWeapon;
        }

        player.playerEquipmentManager.LoadRightWeapon();

        if (player.IsOwner)
        {
            PlayerUIManager.instance.playerUIHudManager.SetRightWeaponQuickSlotIcon(newID);
        }
    }

    public void OnCurrentLeftHandWeaponIDChange(int oldID, int newID)
    {
        if (!player.IsOwner)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
            player.playerInventoryManager.currentLeftHandWeapon = newWeapon;
        }
        player.playerEquipmentManager.LoadLeftWeapon();

        if (player.IsOwner)
        {
            PlayerUIManager.instance.playerUIHudManager.SetLeftWeaponQuickSlotIcon(newID);
        }
    }

    public void OnCurrentWeaponBeingUsedIDChange(int oldID, int newID)
    {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
        player.playerCombatManager.currentWeaponBeingUsed = newWeapon;

        if (player.IsOwner)
            return;

        if (player.playerCombatManager.currentWeaponBeingUsed != null)
            player.playerAnimatorManager.UpdateAnimatorController(player.playerCombatManager.currentWeaponBeingUsed.weaponAnimator);
    }

    public void OnCurrentQuickSlotItemIDChange(int oldID, int newID)
    {
        QuickSlotItem newQuickSlotItem = null;

        if (WorldItemDatabase.instance.GetQuickSlotItemByID(newID))
            newQuickSlotItem = Instantiate(WorldItemDatabase.instance.GetQuickSlotItemByID(newID));

        if (newQuickSlotItem != null)
        {
            player.playerInventoryManager.currentQuickSlotItem = newQuickSlotItem;
        }
        else
        {
            player.playerInventoryManager.currentQuickSlotItem = null;
        }

        if (player.IsOwner)
            PlayerUIManager.instance.playerUIHudManager.SetQuickSlotItemQuickSlotIcon(player.playerInventoryManager.currentQuickSlotItem);
    }

    public override void OnIsBlockingChanged(bool oldStatus, bool newStatus)
    {
        base.OnIsBlockingChanged(oldStatus, newStatus);

        if (IsOwner)
        {
            player.playerStatsManager.blockingPhysicalAbsorption = player.playerCombatManager.currentWeaponBeingUsed.physicalBlockAbsorption;
            player.playerStatsManager.blockingMagicAbsorption = player.playerCombatManager.currentWeaponBeingUsed.magicBlockAbsorption;
            player.playerStatsManager.blockingFireAbsorption = player.playerCombatManager.currentWeaponBeingUsed.fireBlockAbsorption;
            player.playerStatsManager.blockingLightningAbsorption = player.playerCombatManager.currentWeaponBeingUsed.lightningBlockAbsorption;
            player.playerStatsManager.blockingHolyAbsorption = player.playerCombatManager.currentWeaponBeingUsed.holyBlockAbsorption;
            player.playerStatsManager.blockingStability = player.playerCombatManager.currentWeaponBeingUsed.stabilty;
        }
    }

    public void OnIsTwoHandingWeaponChanged(bool oldStatus, bool newStatus)
    {
        if (!isTwoHandingWeapon.Value)
        {
            if (IsOwner)
            {
                isTwoHandingLeftWeapon.Value = false;
                isTwoHandingRightWeapon.Value = false;
            }
            player.playerEquipmentManager.UnTwoHandWeapon();
            player.playerEffectsManager.RemoveStaticEffect(WorldCharacterEffectsManager.instance.twoHandingEffect.staticEffectID);
        }
        else
        {
            StaticCharacterEffects twoHandEffect = Instantiate(WorldCharacterEffectsManager.instance.twoHandingEffect);
            player.playerEffectsManager.AddStaticEffect(twoHandEffect);
        }
        player.animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon.Value);
    }

    public void OnIsTwoHandingRightWeaponChanged(bool oldStatus, bool newStatus)
    {
        if (!isTwoHandingRightWeapon.Value)
            return;

        if (IsOwner)
        {
            currentWeaponBeingTwoHanded.Value = currentRightHandWeaponID.Value;
            isTwoHandingWeapon.Value = true;
        }

        player.playerInventoryManager.currentTwoHandWeapon = player.playerInventoryManager.currentRightHandWeapon;
        player.playerEquipmentManager.TwoHandRightWeapon();
    }

    public void OnIsTwoHandingLeftWeaponChanged(bool oldStatus, bool newStatus)
    {
        if (!isTwoHandingLeftWeapon.Value)
            return;

        if (IsOwner)
        {
            currentWeaponBeingTwoHanded.Value = currentLeftHandWeaponID.Value;
            isTwoHandingWeapon.Value = true;
        }

        player.playerInventoryManager.currentTwoHandWeapon = player.playerInventoryManager.currentLeftHandWeapon;
        player.playerEquipmentManager.TwoHandLeftWeapon();
    }

    public void OnIsChuggingChanged(bool oldStatus, bool newStatus)
    {
        player.animator.SetBool("isChuggingFlask", isChugging.Value);
    }

    public void OnHeadEquipmentChanged(int oldValue, int newValue)
    {
        if (IsOwner)
            return;

        HeadEquipmentItem equipment = WorldItemDatabase.instance.GetHeadEquipmentByID(headEquipmentID.Value);

        if (equipment != null)
        {
            player.playerEquipmentManager.LoadHeadEquipment(Instantiate(equipment));
        }
        else
        {
            player.playerEquipmentManager.LoadHeadEquipment(null);

        }
    }

    public void OnBodyEquipmentChanged(int oldValue, int newValue)
    {
        if (IsOwner)
            return;

        BodyEquipmentItem equipment = WorldItemDatabase.instance.GetBodyEquipmentByID(bodyEquipmentID.Value);

        if (equipment != null)
        {
            player.playerEquipmentManager.LoadBodyEquipment(Instantiate(equipment));
        }
        else
        {
            player.playerEquipmentManager.LoadBodyEquipment(null);
        }
    }

    public void OnLegEquipmentChanged(int oldValue, int newValue)
    {
        if (IsOwner)
            return;

        LegEquipmentItem equipment = WorldItemDatabase.instance.GetLegEquipmentByID(legEquipmentID.Value);

        if (equipment != null)
        {
            player.playerEquipmentManager.LoadLegEquipment(Instantiate(equipment));
        }
        else
        {
            player.playerEquipmentManager.LoadLegEquipment(null);
        }
    }

    public void OnHandEquipmentChanged(int oldValue, int newValue)
    {
        if (IsOwner)
            return;

        HandEquipmentItem equipment = WorldItemDatabase.instance.GetHandEquipmentByID(handEquipmentID.Value);

        if (equipment != null)
        {
            player.playerEquipmentManager.LoadHandEquipment(Instantiate(equipment));
        }
        else
        {
            player.playerEquipmentManager.LoadHandEquipment(null);
        }
    }

    public void OnIsMaleChanged(bool oldStatus, bool newStatus)
    {
        player.playerBodyManager.ToggleBodyType(isMale.Value);
    }

    [ServerRpc]
    public void NotifyTheServerOfWeaponActionAnimationServerRpc(ulong clientId, int actionID, int weaponID)

    {
        if (IsServer)
        {
            NotifyTheServerOfWeaponActionAnimationClientRpc(clientId, actionID, weaponID);
        }
    }

    [ClientRpc]
    private void NotifyTheServerOfWeaponActionAnimationClientRpc(ulong clientId, int actionID, int weaponID)
    {
        if (clientId != NetworkManager.Singleton.LocalClientId)
        {
            PerformWeaponBasedAction(actionID, weaponID);
        }
    }

    private void PerformWeaponBasedAction(int actionID, int weaponID)
    {
        WeaponItemAction weaponAction = WorldActionManager.instance.GetWeaponItemActionByID(actionID);

        if (weaponAction != null)
        {
            weaponAction.AttempToPerformAction(player, WorldItemDatabase.instance.GetWeaponByID(weaponID));
        }
        else
        {

        }
    }

    [ServerRpc]
    public void HideWeaponsServerRpc()
    {
        if (IsServer)
        {
            HideWeaponsClientRpc();
        }
    }

    [ClientRpc]
    private void HideWeaponsClientRpc()
    {
        if (player.playerEquipmentManager.rightHandWeaponModel != null)
            player.playerEquipmentManager.rightHandWeaponModel.SetActive(false);

        if (player.playerEquipmentManager.leftHandWeaponModel != null)
            player.playerEquipmentManager.leftHandWeaponModel.SetActive(false);
    }

    [ServerRpc]
    public void NotifyServerOfQuickSlotItemActionServerRpc(ulong clientID, int quickSlotItemID)
    {
        NotifyServerOfQuickSlotItemActionClientRpc(clientID, quickSlotItemID);
    }

    [ClientRpc]
    private void NotifyServerOfQuickSlotItemActionClientRpc(ulong clientID, int quickSlotItemID)
    {
        if (clientID != NetworkManager.Singleton.LocalClientId)
        {
            QuickSlotItem item = WorldItemDatabase.instance.GetQuickSlotItemByID(quickSlotItemID);
            item.AttemptToUseItem(player);

        }
    }

}
