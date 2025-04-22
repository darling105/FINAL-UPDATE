using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerStatsManager playerStatsManager;
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;
    [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
    [HideInInspector] public PlayerCombatManager playerCombatManager;
    [HideInInspector] public PlayerInteractionManager playerInteractionManager;
    [HideInInspector] public PlayerNetworkManager playerNetworkManager;
    [HideInInspector] public PlayerEffectsManager playerEffectsManager;
    [HideInInspector] public PlayerBodyManager playerBodyManager;
    public string characterName = "Character";
    protected override void Awake()
    {
        base.Awake();

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        playerInteractionManager = GetComponent<PlayerInteractionManager>();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        playerBodyManager = GetComponent<PlayerBodyManager>();

    }

    protected override void Update()
    {
        base.Update();

        if (!IsOwner)
            return;

        playerLocomotionManager.HandleAllMovement();

        playerStatsManager.RegenerateStamina();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        if (!IsOwner)
            return;

        PlayerCamera.instance.HandleAllCameraActions();
    }

    protected override void OnEnable()
    {
        base.OnEnable();


    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (!IsOwner)
            characterNetworkManager.currentHealth.OnValueChanged -= characterNetworkManager.CheckHP;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallBack;

        if (IsOwner)
        {
            PlayerCamera.instance.player = this;
            PlayerInputManager.instance.player = this;
            PlayerUIManager.instance.localPlayer = this;
            WorldSaveGameManager.instance.player = this;

            playerNetworkManager.vitality.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
            playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;
            playerNetworkManager.mind.OnValueChanged += playerNetworkManager.SetNewMaxFocusPointsValue;

            playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue;
            playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
            playerNetworkManager.currentFocusPoints.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewFocusPointsValue;
            playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;
        }

        if (!IsOwner)
            characterNetworkManager.currentHealth.OnValueChanged += characterUIManager.OnHPChanged;

        //body type
        playerNetworkManager.isMale.OnValueChanged += playerNetworkManager.OnIsMaleChanged;
        playerNetworkManager.hairColorRed.OnValueChanged += playerNetworkManager.OnHairColorRedChanged;
        playerNetworkManager.hairColorGreen.OnValueChanged += playerNetworkManager.OnHairColorGreenChanged;
        playerNetworkManager.hairColorBlue.OnValueChanged += playerNetworkManager.OnHairColorBlueChanged;

        playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.CheckHP;

        //Lock On
        playerNetworkManager.isLockedOn.OnValueChanged += playerNetworkManager.OnIsLockedOnChanged;
        playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged += playerNetworkManager.OnLockOnTargetIDChange;

        //body
        playerNetworkManager.hairStyleID.OnValueChanged += playerNetworkManager.OnHairStyleIDChanged;

        //equipment
        playerNetworkManager.currentRightHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentRightHandWeaponIDChange;
        playerNetworkManager.currentLeftHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
        playerNetworkManager.currentWeaponBeingUsed.OnValueChanged += playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;
        playerNetworkManager.currentQuickSlotItemID.OnValueChanged += playerNetworkManager.OnCurrentQuickSlotItemIDChange;
        playerNetworkManager.isChugging.OnValueChanged += playerNetworkManager.OnIsChuggingChanged;
        playerNetworkManager.isBlocking.OnValueChanged += playerNetworkManager.OnIsBlockingChanged;
        playerNetworkManager.headEquipmentID.OnValueChanged += playerNetworkManager.OnHeadEquipmentChanged;
        playerNetworkManager.bodyEquipmentID.OnValueChanged += playerNetworkManager.OnBodyEquipmentChanged;
        playerNetworkManager.legEquipmentID.OnValueChanged += playerNetworkManager.OnLegEquipmentChanged;
        playerNetworkManager.handEquipmentID.OnValueChanged += playerNetworkManager.OnHandEquipmentChanged;

        //twohand
        playerNetworkManager.isTwoHandingWeapon.OnValueChanged += playerNetworkManager.OnIsTwoHandingWeaponChanged;
        playerNetworkManager.isTwoHandingRightWeapon.OnValueChanged += playerNetworkManager.OnIsTwoHandingRightWeaponChanged;
        playerNetworkManager.isTwoHandingLeftWeapon.OnValueChanged += playerNetworkManager.OnIsTwoHandingLeftWeaponChanged;

        //flags
        playerNetworkManager.isChargingAttack.OnValueChanged += playerNetworkManager.OnIsChargingAttackChanged;

        if (IsOwner && !IsServer)
        {
            LoadGameDataFromCurrentCharacterData(ref WorldSaveGameManager.instance.currentCharacterData);
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallBack;

        if (IsOwner)
        {
            playerNetworkManager.vitality.OnValueChanged -= playerNetworkManager.SetNewMaxHealthValue;
            playerNetworkManager.endurance.OnValueChanged -= playerNetworkManager.SetNewMaxStaminaValue;
            playerNetworkManager.mind.OnValueChanged -= playerNetworkManager.SetNewMaxFocusPointsValue;

            playerNetworkManager.currentHealth.OnValueChanged -= PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue;
            playerNetworkManager.currentStamina.OnValueChanged -= PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
            playerNetworkManager.currentFocusPoints.OnValueChanged -= PlayerUIManager.instance.playerUIHudManager.SetNewFocusPointsValue;
            playerNetworkManager.currentStamina.OnValueChanged -= playerStatsManager.ResetStaminaRegenTimer;
        }

        if (!IsOwner)
            characterNetworkManager.currentHealth.OnValueChanged -= characterUIManager.OnHPChanged;

        playerNetworkManager.isMale.OnValueChanged -= playerNetworkManager.OnIsMaleChanged;
        playerNetworkManager.hairColorRed.OnValueChanged -= playerNetworkManager.OnHairColorRedChanged;
        playerNetworkManager.hairColorGreen.OnValueChanged -= playerNetworkManager.OnHairColorGreenChanged;
        playerNetworkManager.hairColorBlue.OnValueChanged -= playerNetworkManager.OnHairColorBlueChanged;

        playerNetworkManager.currentHealth.OnValueChanged -= playerNetworkManager.CheckHP;

        //Lock On
        playerNetworkManager.isLockedOn.OnValueChanged -= playerNetworkManager.OnIsLockedOnChanged;
        playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged -= playerNetworkManager.OnLockOnTargetIDChange;

        //body type
        playerNetworkManager.hairStyleID.OnValueChanged -= playerNetworkManager.OnHairStyleIDChanged;

        //equimpemt
        playerNetworkManager.currentRightHandWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentRightHandWeaponIDChange;
        playerNetworkManager.currentLeftHandWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
        playerNetworkManager.currentWeaponBeingUsed.OnValueChanged -= playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;
        playerNetworkManager.currentQuickSlotItemID.OnValueChanged -= playerNetworkManager.OnCurrentQuickSlotItemIDChange;
        playerNetworkManager.isChugging.OnValueChanged -= playerNetworkManager.OnIsChuggingChanged;
        playerNetworkManager.headEquipmentID.OnValueChanged -= playerNetworkManager.OnHeadEquipmentChanged;
        playerNetworkManager.bodyEquipmentID.OnValueChanged -= playerNetworkManager.OnBodyEquipmentChanged;
        playerNetworkManager.legEquipmentID.OnValueChanged -= playerNetworkManager.OnLegEquipmentChanged;
        playerNetworkManager.handEquipmentID.OnValueChanged -= playerNetworkManager.OnHandEquipmentChanged;

        playerNetworkManager.isTwoHandingWeapon.OnValueChanged -= playerNetworkManager.OnIsTwoHandingWeaponChanged;
        playerNetworkManager.isTwoHandingRightWeapon.OnValueChanged -= playerNetworkManager.OnIsTwoHandingRightWeaponChanged;
        playerNetworkManager.isTwoHandingLeftWeapon.OnValueChanged -= playerNetworkManager.OnIsTwoHandingLeftWeaponChanged;

        //flags
        playerNetworkManager.isChargingAttack.OnValueChanged -= playerNetworkManager.OnIsChargingAttackChanged;
    }

    private void OnClientConnectedCallBack(ulong clientID)
    {
        WorldGameSessionManager.instance.AddPlayerToActivePlayerList(this);
        if (!IsServer && IsOwner)
        {
            foreach (var player in WorldGameSessionManager.instance.players)
            {
                if (player != this)
                {
                    player.LoadOtherPlayerCharacterWhenJoiningServer();
                }
            }
        }
    }

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {
        if (IsOwner)
        {
            PlayerUIManager.instance.playerUIPopUpManager.SendYouDiedPopUP();
        }
        WorldSoundFXManager.instance.StopBossMusic();

        WorldGameSessionManager.instance.WaitThenRevivePlayer();

        return base.ProcessDeathEvent(manuallySelectDeathAnimation);
    }

    public override void ReviveCharacter()
    {
        base.ReviveCharacter();

        if (IsOwner)
        {
            isDead.Value = false;

            playerNetworkManager.currentHealth.Value = playerNetworkManager.maxHealth.Value;
            playerNetworkManager.currentStamina.Value = playerNetworkManager.maxStamina.Value;
            playerNetworkManager.currentFocusPoints.Value = playerNetworkManager.maxFocusPoints.Value;
            playerNetworkManager.remainingHealthFlasks.Value = 3;
            playerNetworkManager.remainingFocusPointFlasks.Value = 3;

            playerAnimatorManager.PlayTargetActionAnimation("Empty", true);
        }
    }

    public void OnCurrentWeaponBeingUsedIDChange(int newID)
    {
        WeaponItem weaponData = WorldItemDatabase.instance.GetWeaponByID(newID);
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
        playerCombatManager.currentWeaponBeingUsed = newWeapon;
        playerEquipmentManager.LoadRightWeapon();

        if (playerCombatManager.currentWeaponBeingUsed != null)
        {
            playerAnimatorManager.UpdateAnimatorController(playerCombatManager.currentWeaponBeingUsed.weaponAnimator);
        }
    }

    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;

        currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
        currentCharacterData.isMale = playerNetworkManager.isMale.Value;
        currentCharacterData.xPosition = transform.position.x;
        currentCharacterData.yPosition = transform.position.y;
        currentCharacterData.zPosition = transform.position.z;

        currentCharacterData.vitality = playerNetworkManager.vitality.Value;
        currentCharacterData.endurance = playerNetworkManager.endurance.Value;
        currentCharacterData.mind = playerNetworkManager.mind.Value;
        currentCharacterData.strength = playerNetworkManager.strength.Value;
        currentCharacterData.dexterity = playerNetworkManager.dexterity.Value;
        currentCharacterData.intelligence = playerNetworkManager.intelligence.Value;
        currentCharacterData.faith = playerNetworkManager.faith.Value;

        currentCharacterData.hairStyleID = playerNetworkManager.hairStyleID.Value;
        currentCharacterData.hairColorRed = playerNetworkManager.hairColorRed.Value;
        currentCharacterData.hairColorGreen = playerNetworkManager.hairColorGreen.Value;
        currentCharacterData.hairColorBlue = playerNetworkManager.hairColorBlue.Value;

        currentCharacterData.currentHealthFlasksRemaining = playerNetworkManager.remainingHealthFlasks.Value;
        currentCharacterData.currentFocusPointsRemaining = playerNetworkManager.remainingFocusPointFlasks.Value;

        currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;
        currentCharacterData.currentStamina = playerNetworkManager.currentStamina.Value;
        currentCharacterData.currentFocusPoints = playerNetworkManager.currentFocusPoints.Value;
        currentCharacterData.shades = playerStatsManager.shades;

        //equipment
        currentCharacterData.headEquipment = playerNetworkManager.headEquipmentID.Value;
        currentCharacterData.bodyEquipment = playerNetworkManager.bodyEquipmentID.Value;
        currentCharacterData.legEquipment = playerNetworkManager.legEquipmentID.Value;
        currentCharacterData.handEquipment = playerNetworkManager.handEquipmentID.Value;

        currentCharacterData.rightWeaponIndex = playerInventoryManager.rightHandWeaponIndex;
        currentCharacterData.rightWeapon01 = WorldSaveGameManager.instance.GetSerializableWeaponsFromWeaponItem(playerInventoryManager.weaponInRightHandSlot[0]);
        currentCharacterData.rightWeapon02 = WorldSaveGameManager.instance.GetSerializableWeaponsFromWeaponItem(playerInventoryManager.weaponInRightHandSlot[1]);
        currentCharacterData.rightWeapon03 = WorldSaveGameManager.instance.GetSerializableWeaponsFromWeaponItem(playerInventoryManager.weaponInRightHandSlot[2]);

        currentCharacterData.leftWeaponIndex = playerInventoryManager.leftHandWeaponIndex;
        currentCharacterData.leftWeapon01 = WorldSaveGameManager.instance.GetSerializableWeaponsFromWeaponItem(playerInventoryManager.weaponInLeftHandSlot[0]);
        currentCharacterData.leftWeapon02 = WorldSaveGameManager.instance.GetSerializableWeaponsFromWeaponItem(playerInventoryManager.weaponInLeftHandSlot[1]);
        currentCharacterData.leftWeapon03 = WorldSaveGameManager.instance.GetSerializableWeaponsFromWeaponItem(playerInventoryManager.weaponInLeftHandSlot[2]);

        currentCharacterData.quickSlotIndex = playerInventoryManager.quickSlotItemIndex;
        currentCharacterData.quickSlotItem01 = WorldSaveGameManager.instance.GetSerializableQuickSlotItemFromQuickSlotItem(playerInventoryManager.quickSlotItemsInQuickSlots[0]);
        currentCharacterData.quickSlotItem02 = WorldSaveGameManager.instance.GetSerializableQuickSlotItemFromQuickSlotItem(playerInventoryManager.quickSlotItemsInQuickSlots[1]);
        currentCharacterData.quickSlotItem03 = WorldSaveGameManager.instance.GetSerializableQuickSlotItemFromQuickSlotItem(playerInventoryManager.quickSlotItemsInQuickSlots[2]);

        currentCharacterData.weaponsInventory = new List<SerializableWeapons>();
        currentCharacterData.quickSlotItemsInventory = new List<SerializableQuickSlotItem>();
        currentCharacterData.headEquipmentInInventory = new List<int>();
        currentCharacterData.bodyEquipmentInInventory = new List<int>();
        currentCharacterData.legEquipmentInInventory = new List<int>();
        currentCharacterData.handEquipmentInInventory = new List<int>();

        for (int i = 0; i < playerInventoryManager.itemsInInventory.Count; i++)
        {
            if (playerInventoryManager.itemsInInventory[i] == null)
                continue;

            WeaponItem weaponInInventory = playerInventoryManager.itemsInInventory[i] as WeaponItem;
            HeadEquipmentItem headEquipmentInInventory = playerInventoryManager.itemsInInventory[i] as HeadEquipmentItem;
            BodyEquipmentItem bodyEquipmentInInventory = playerInventoryManager.itemsInInventory[i] as BodyEquipmentItem;
            LegEquipmentItem legEquipmentInInventory = playerInventoryManager.itemsInInventory[i] as LegEquipmentItem;
            HandEquipmentItem handEquipmentInInventory = playerInventoryManager.itemsInInventory[i] as HandEquipmentItem;
            QuickSlotItem quickSlotItemInInventory = playerInventoryManager.itemsInInventory[i] as QuickSlotItem;

            if (weaponInInventory != null)
                currentCharacterData.weaponsInventory.Add(WorldSaveGameManager.instance.GetSerializableWeaponsFromWeaponItem(weaponInInventory));

            if (quickSlotItemInInventory != null)
                currentCharacterData.quickSlotItemsInventory.Add(WorldSaveGameManager.instance.GetSerializableQuickSlotItemFromQuickSlotItem(quickSlotItemInInventory));

            if (headEquipmentInInventory != null)
                currentCharacterData.headEquipmentInInventory.Add(headEquipmentInInventory.itemID);
            if (bodyEquipmentInInventory != null)
                currentCharacterData.bodyEquipmentInInventory.Add(bodyEquipmentInInventory.itemID);
            if (legEquipmentInInventory != null)
                currentCharacterData.legEquipmentInInventory.Add(legEquipmentInInventory.itemID);
            if (handEquipmentInInventory != null)
                currentCharacterData.handEquipmentInInventory.Add(handEquipmentInInventory.itemID);

        }

    }

    public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        playerNetworkManager.characterName.Value = currentCharacterData.characterName;
        playerNetworkManager.isMale.Value = currentCharacterData.isMale;
        playerBodyManager.ToggleBodyType(currentCharacterData.isMale);
        Vector3 myPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
        transform.position = myPosition;

        playerNetworkManager.vitality.Value = currentCharacterData.vitality;
        playerNetworkManager.endurance.Value = currentCharacterData.endurance;
        playerNetworkManager.mind.Value = currentCharacterData.mind;
        playerNetworkManager.strength.Value = currentCharacterData.strength;
        playerNetworkManager.dexterity.Value = currentCharacterData.dexterity;
        playerNetworkManager.intelligence.Value = currentCharacterData.intelligence;
        playerNetworkManager.faith.Value = currentCharacterData.faith;

        // playerStatsManager.shades = currentCharacterData.shades;
        // PlayerUIManager.instance.playerUIHudManager.SetShadesCount(currentCharacterData.shades);
        playerStatsManager.AddShades(currentCharacterData.shades);

        playerNetworkManager.maxHealth.Value = playerStatsManager.CalculateHealhtBasedOnVitalityLevel(currentCharacterData.vitality);
        playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(currentCharacterData.endurance);
        playerNetworkManager.maxFocusPoints.Value = playerStatsManager.CalculateFocusPointsBasedOnMindLevel(currentCharacterData.mind);
        playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
        playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;
        playerNetworkManager.currentFocusPoints.Value = currentCharacterData.currentFocusPoints;

        playerNetworkManager.remainingHealthFlasks.Value = currentCharacterData.currentHealthFlasksRemaining;
        playerNetworkManager.remainingFocusPointFlasks.Value = currentCharacterData.currentFocusPointsRemaining;

        playerNetworkManager.hairStyleID.Value = currentCharacterData.hairStyleID;
        playerNetworkManager.hairColorRed.Value = currentCharacterData.hairColorRed;
        playerNetworkManager.hairColorGreen.Value = currentCharacterData.hairColorGreen;
        playerNetworkManager.hairColorBlue.Value = currentCharacterData.hairColorBlue;

        //equipment
        if (WorldItemDatabase.instance.GetHeadEquipmentByID(currentCharacterData.headEquipment))
        {
            HeadEquipmentItem headEquipment = Instantiate(WorldItemDatabase.instance.GetHeadEquipmentByID(currentCharacterData.headEquipment));
            playerInventoryManager.headEquipment = headEquipment;
        }
        else
        {
            playerInventoryManager.headEquipment = null;
        }
        if (WorldItemDatabase.instance.GetBodyEquipmentByID(currentCharacterData.bodyEquipment))
        {
            BodyEquipmentItem bodyEquipment = Instantiate(WorldItemDatabase.instance.GetBodyEquipmentByID(currentCharacterData.bodyEquipment));
            playerInventoryManager.bodyEquipment = bodyEquipment;
        }
        else
        {
            playerInventoryManager.bodyEquipment = null;
        }
        if (WorldItemDatabase.instance.GetLegEquipmentByID(currentCharacterData.legEquipment))
        {
            LegEquipmentItem legEquipment = Instantiate(WorldItemDatabase.instance.GetLegEquipmentByID(currentCharacterData.legEquipment));
            playerInventoryManager.legEquipment = legEquipment;
        }
        else
        {
            playerInventoryManager.legEquipment = null;
        }
        if (WorldItemDatabase.instance.GetHandEquipmentByID(currentCharacterData.handEquipment))
        {
            HandEquipmentItem handEquipment = Instantiate(WorldItemDatabase.instance.GetHandEquipmentByID(currentCharacterData.handEquipment));
            playerInventoryManager.handEquipment = handEquipment;
        }
        else
        {
            playerInventoryManager.handEquipment = null;
        }

        //weapon
        playerInventoryManager.rightHandWeaponIndex = currentCharacterData.rightWeaponIndex;
        playerInventoryManager.weaponInRightHandSlot[0] = currentCharacterData.rightWeapon01.GetWeapon();
        playerInventoryManager.weaponInRightHandSlot[1] = currentCharacterData.rightWeapon02.GetWeapon();
        playerInventoryManager.weaponInRightHandSlot[2] = currentCharacterData.rightWeapon03.GetWeapon();

        playerInventoryManager.leftHandWeaponIndex = currentCharacterData.leftWeaponIndex;
        playerInventoryManager.weaponInLeftHandSlot[0] = currentCharacterData.leftWeapon01.GetWeapon();
        playerInventoryManager.weaponInLeftHandSlot[1] = currentCharacterData.leftWeapon02.GetWeapon();
        playerInventoryManager.weaponInLeftHandSlot[2] = currentCharacterData.leftWeapon03.GetWeapon();

        playerInventoryManager.quickSlotItemIndex = currentCharacterData.quickSlotIndex;
        playerInventoryManager.quickSlotItemsInQuickSlots[0] = currentCharacterData.quickSlotItem01.GetQuickSlotItem();
        playerInventoryManager.quickSlotItemsInQuickSlots[1] = currentCharacterData.quickSlotItem02.GetQuickSlotItem();
        playerInventoryManager.quickSlotItemsInQuickSlots[2] = currentCharacterData.quickSlotItem03.GetQuickSlotItem();
        playerEquipmentManager.LoadQuickSlotItem(playerInventoryManager.quickSlotItemsInQuickSlots[playerInventoryManager.quickSlotItemIndex]);


        playerInventoryManager.rightHandWeaponIndex = currentCharacterData.rightWeaponIndex;
        if (currentCharacterData.rightWeaponIndex >= 0)
        {
            playerInventoryManager.currentRightHandWeapon = playerInventoryManager.weaponInRightHandSlot[currentCharacterData.rightWeaponIndex];
            playerNetworkManager.currentRightHandWeaponID.Value = playerInventoryManager.weaponInRightHandSlot[currentCharacterData.rightWeaponIndex].itemID;
        }
        else
        {
            playerNetworkManager.currentRightHandWeaponID.Value = WorldItemDatabase.instance.unarmedWeapon.itemID;
        }
        playerInventoryManager.leftHandWeaponIndex = currentCharacterData.leftWeaponIndex;
        if (currentCharacterData.leftWeaponIndex >= 0)
        {
            playerInventoryManager.currentLeftHandWeapon = playerInventoryManager.weaponInLeftHandSlot[currentCharacterData.leftWeaponIndex];
            playerNetworkManager.currentLeftHandWeaponID.Value = playerInventoryManager.weaponInLeftHandSlot[currentCharacterData.leftWeaponIndex].itemID;
        }
        else
        {
            playerNetworkManager.currentLeftHandWeaponID.Value = WorldItemDatabase.instance.unarmedWeapon.itemID;
        }

        for (int i = 0; i < currentCharacterData.weaponsInventory.Count; i++)
        {
            WeaponItem weapon = currentCharacterData.weaponsInventory[i].GetWeapon();
            playerInventoryManager.AddItemToInventory(weapon);
        }

        for (int i = 0; i < currentCharacterData.quickSlotItemsInventory.Count; i++)
        {
            QuickSlotItem quickSlotItem = currentCharacterData.quickSlotItemsInventory[i].GetQuickSlotItem();
            playerInventoryManager.AddItemToInventory(quickSlotItem);
        }

        for (int i = 0; i < currentCharacterData.headEquipmentInInventory.Count; i++)
        {
            EquipmentItem equipment = WorldItemDatabase.instance.GetHeadEquipmentByID(currentCharacterData.headEquipmentInInventory[i]);
            playerInventoryManager.AddItemToInventory(equipment);
        }
        for (int i = 0; i < currentCharacterData.bodyEquipmentInInventory.Count; i++)
        {
            EquipmentItem equipment = WorldItemDatabase.instance.GetBodyEquipmentByID(currentCharacterData.bodyEquipmentInInventory[i]);
            playerInventoryManager.AddItemToInventory(equipment);
        }
        for (int i = 0; i < currentCharacterData.legEquipmentInInventory.Count; i++)
        {
            EquipmentItem equipment = WorldItemDatabase.instance.GetLegEquipmentByID(currentCharacterData.legEquipmentInInventory[i]);
            playerInventoryManager.AddItemToInventory(equipment);
        }
        for (int i = 0; i < currentCharacterData.handEquipmentInInventory.Count; i++)
        {
            EquipmentItem equipment = WorldItemDatabase.instance.GetHandEquipmentByID(currentCharacterData.handEquipmentInInventory[i]);
            playerInventoryManager.AddItemToInventory(equipment);
        }

        playerEquipmentManager.EquipArmor();


    }

    public void LoadOtherPlayerCharacterWhenJoiningServer()
    {
        playerNetworkManager.OnIsMaleChanged(false, playerNetworkManager.isMale.Value);
        playerNetworkManager.OnHairStyleIDChanged(0, playerNetworkManager.hairStyleID.Value);
        playerNetworkManager.OnHairColorRedChanged(0, playerNetworkManager.hairColorRed.Value);
        playerNetworkManager.OnHairColorGreenChanged(0, playerNetworkManager.hairColorGreen.Value);
        playerNetworkManager.OnHairColorBlueChanged(0, playerNetworkManager.hairColorBlue.Value);

        playerNetworkManager.OnCurrentRightHandWeaponIDChange(0, playerNetworkManager.currentRightHandWeaponID.Value);
        playerNetworkManager.OnCurrentLeftHandWeaponIDChange(0, playerNetworkManager.currentLeftHandWeaponID.Value);

        playerNetworkManager.OnHeadEquipmentChanged(0, playerNetworkManager.headEquipmentID.Value);
        playerNetworkManager.OnBodyEquipmentChanged(0, playerNetworkManager.bodyEquipmentID.Value);
        playerNetworkManager.OnLegEquipmentChanged(0, playerNetworkManager.legEquipmentID.Value);
        playerNetworkManager.OnHandEquipmentChanged(0, playerNetworkManager.handEquipmentID.Value);

        playerNetworkManager.OnIsTwoHandingRightWeaponChanged(false, playerNetworkManager.isTwoHandingRightWeapon.Value);
        playerNetworkManager.OnIsTwoHandingLeftWeaponChanged(false, playerNetworkManager.isTwoHandingLeftWeapon.Value);

        playerNetworkManager.OnIsBlockingChanged(false, playerNetworkManager.isBlocking.Value);

        if (playerNetworkManager.isLockedOn.Value)
        {
            playerNetworkManager.OnLockOnTargetIDChange(0, playerNetworkManager.currentTargetNetworkObjectID.Value);
        }
    }

    public void PlayerWeaponAction(int actionID, int weaponID)
    {
        WeaponItemAction weaponAction = WorldActionManager.instance.GetWeaponItemActionByID(actionID);

        if (weaponAction != null)
        {
            weaponAction.AttempToPerformAction(this, WorldItemDatabase.instance.GetWeaponByID(weaponID));
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy hành động vũ khí với ID: {actionID}");
        }
    }
}
