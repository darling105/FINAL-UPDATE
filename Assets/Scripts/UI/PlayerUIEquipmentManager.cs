using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class PlayerUIEquipmentManager : PlayerUIMenu
{

    [Header("Weapon Slots")]
    [SerializeField] Image rightHandSlot01;
    private Button rightHandSlot01Button;
    [SerializeField] Image rightHandSlot02;
    private Button rightHandSlot02Button;
    [SerializeField] Image rightHandSlot03;
    private Button rightHandSlot03Button;
    [SerializeField] Image leftHandSlot01;
    private Button leftHandSlot01Button;
    [SerializeField] Image leftHandSlot02;
    private Button leftHandSlot02Button;
    [SerializeField] Image leftHandSlot03;
    private Button leftHandSlot03Button;

    [Header("Armor Slots")]
    [SerializeField] Image headEquipmentSlot;
    private Button headEquipmentSlotButton;
    [SerializeField] Image bodyEquipmentSlot;
    private Button bodyEquipmentSlotButton;
    [SerializeField] Image legEquipmentSlot;
    private Button legEquipmentSlotButton;
    [SerializeField] Image handEquipmentSlot;
    private Button handEquipmentSlotButton;

    [Header("Equipment Inventory")]
    [SerializeField] GameObject equipmentInventoryWindow;
    public EquipmentType currentSelectedEquipmentSlot;
    [SerializeField] GameObject equipmentInventorySlotPrefab;
    [SerializeField] Transform equipmentInventoryContentWindow;
    [SerializeField] Item currentSelectedItem;

    [Header("Quick Slots")]
    [SerializeField] Image quickSlot01EquipmentSlot;
    [SerializeField] TextMeshProUGUI quickSlot01Count;
    private Button quickSlot01Button;
    [SerializeField] Image quickSlot02EquipmentSlot;
    [SerializeField] TextMeshProUGUI quickSlot02Count;
    private Button quickSlot02Button;
    [SerializeField] Image quickSlot03EquipmentSlot;
    [SerializeField] TextMeshProUGUI quickSlot03Count;
    private Button quickSlot03Button;

    private void Awake()
    {
        rightHandSlot01Button = rightHandSlot01.GetComponentInParent<Button>(true);
        rightHandSlot02Button = rightHandSlot02.GetComponentInParent<Button>(true);
        rightHandSlot03Button = rightHandSlot03.GetComponentInParent<Button>(true);

        leftHandSlot01Button = leftHandSlot01.GetComponentInParent<Button>(true);
        leftHandSlot02Button = leftHandSlot02.GetComponentInParent<Button>(true);
        leftHandSlot03Button = leftHandSlot03.GetComponentInParent<Button>(true);

        headEquipmentSlotButton = headEquipmentSlot.GetComponentInParent<Button>(true);
        bodyEquipmentSlotButton = bodyEquipmentSlot.GetComponentInParent<Button>(true);
        legEquipmentSlotButton = legEquipmentSlot.GetComponentInParent<Button>(true);
        handEquipmentSlotButton = handEquipmentSlot.GetComponentInParent<Button>(true);

        quickSlot01Button = quickSlot01EquipmentSlot.GetComponentInParent<Button>(true);
        quickSlot02Button = quickSlot02EquipmentSlot.GetComponentInParent<Button>(true);
        quickSlot03Button = quickSlot03EquipmentSlot.GetComponentInParent<Button>(true);
    }


    public override void OpenMenu()
    {
        base.OpenMenu();
        ToggleEquipmentButtons(true);
        equipmentInventoryWindow.SetActive(false);
        ClearEquipmentInventory();
        RefreshEquipmentSlotIcons();
    }


    public void RefreshMenu()
    {
        ClearEquipmentInventory();
        RefreshEquipmentSlotIcons();
    }

    private void ToggleEquipmentButtons(bool isEnabled)
    {
        rightHandSlot01Button.enabled = isEnabled;
        rightHandSlot02Button.enabled = isEnabled;
        rightHandSlot03Button.enabled = isEnabled;

        leftHandSlot01Button.enabled = isEnabled;
        leftHandSlot02Button.enabled = isEnabled;
        leftHandSlot03Button.enabled = isEnabled;

        headEquipmentSlotButton.enabled = isEnabled;
        bodyEquipmentSlotButton.enabled = isEnabled;
        legEquipmentSlotButton.enabled = isEnabled;
        handEquipmentSlotButton.enabled = isEnabled;

        quickSlot01Button.enabled = isEnabled;
        quickSlot02Button.enabled = isEnabled;
        quickSlot03Button.enabled = isEnabled;
    }

    public void SelectLastSelectedEquipmentSlot()
    {
        Button lastSelectedButton = null;

        ToggleEquipmentButtons(true);

        switch (currentSelectedEquipmentSlot)
        {
            case EquipmentType.RightWeapon01:
                lastSelectedButton = rightHandSlot01Button;
                break;
            case EquipmentType.RightWeapon02:
                lastSelectedButton = rightHandSlot02Button;
                break;
            case EquipmentType.RightWeapon03:
                lastSelectedButton = rightHandSlot03Button;
                break;
            case EquipmentType.LeftWeapon01:
                lastSelectedButton = leftHandSlot01Button;
                break;
            case EquipmentType.LeftWeapon02:
                lastSelectedButton = leftHandSlot02Button;
                break;
            case EquipmentType.LeftWeapon03:
                lastSelectedButton = leftHandSlot03Button;
                break;
            case EquipmentType.Head:
                lastSelectedButton = headEquipmentSlotButton;
                break;
            case EquipmentType.Body:
                lastSelectedButton = bodyEquipmentSlotButton;
                break;
            case EquipmentType.Legs:
                lastSelectedButton = legEquipmentSlotButton;
                break;
            case EquipmentType.Hands:
                lastSelectedButton = handEquipmentSlotButton;
                break;
            case EquipmentType.QuickSlot01:
                lastSelectedButton = quickSlot01Button;
                break;
            case EquipmentType.QuickSlot02:
                lastSelectedButton = quickSlot02Button;
                break;
            case EquipmentType.QuickSlot03:
                lastSelectedButton = quickSlot03Button;
                break;
            default:
                break;
        }

        if (lastSelectedButton != null)
        {
            lastSelectedButton.Select();
            lastSelectedButton.OnSelect(null);
        }

        equipmentInventoryWindow.SetActive(false);

    }

    private void RefreshEquipmentSlotIcons()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        //right weapon 01
        WeaponItem rightHandWeapon01 = player.playerInventoryManager.weaponInRightHandSlot[0];

        if (rightHandWeapon01.itemIcon != null)
        {
            rightHandSlot01.enabled = true;
            rightHandSlot01.sprite = rightHandWeapon01.itemIcon;
        }
        else
        {
            rightHandSlot01.enabled = false;
        }

        //right weapon 02
        WeaponItem rightHandWeapon02 = player.playerInventoryManager.weaponInRightHandSlot[1];

        if (rightHandWeapon02.itemIcon != null)
        {
            rightHandSlot02.enabled = true;
            rightHandSlot02.sprite = rightHandWeapon02.itemIcon;
        }
        else
        {
            rightHandSlot02.enabled = false;
        }

        //right weapon 03
        WeaponItem rightHandWeapon03 = player.playerInventoryManager.weaponInRightHandSlot[2];

        if (rightHandWeapon03.itemIcon != null)
        {
            rightHandSlot03.enabled = true;
            rightHandSlot03.sprite = rightHandWeapon03.itemIcon;
        }
        else
        {
            rightHandSlot03.enabled = false;
        }

        //left weapon 01
        WeaponItem leftHandWeapon01 = player.playerInventoryManager.weaponInLeftHandSlot[0];

        if (leftHandWeapon01.itemIcon != null)
        {
            leftHandSlot01.enabled = true;
            leftHandSlot01.sprite = leftHandWeapon01.itemIcon;
        }
        else
        {
            leftHandSlot01.enabled = false;
        }

        //left weapon 02
        WeaponItem leftHandWeapon02 = player.playerInventoryManager.weaponInLeftHandSlot[1];

        if (leftHandWeapon02.itemIcon != null)
        {
            leftHandSlot02.enabled = true;
            leftHandSlot02.sprite = leftHandWeapon02.itemIcon;
        }
        else
        {
            leftHandSlot02.enabled = false;
        }

        //left weapon 03
        WeaponItem leftHandWeapon03 = player.playerInventoryManager.weaponInLeftHandSlot[2];

        if (leftHandWeapon03.itemIcon != null)
        {
            leftHandSlot03.enabled = true;
            leftHandSlot03.sprite = leftHandWeapon03.itemIcon;
        }
        else
        {
            leftHandSlot03.enabled = false;
        }

        //head equipment
        HeadEquipmentItem headEquipment = player.playerInventoryManager.headEquipment;
        if (headEquipment != null)
        {
            headEquipmentSlot.enabled = true;
            headEquipmentSlot.sprite = headEquipment.itemIcon;
        }
        else
        {
            headEquipmentSlot.enabled = false;
        }

        //body equipment
        BodyEquipmentItem bodyEquipment = player.playerInventoryManager.bodyEquipment;
        if (bodyEquipment != null)
        {
            bodyEquipmentSlot.enabled = true;
            bodyEquipmentSlot.sprite = bodyEquipment.itemIcon;
        }
        else
        {
            bodyEquipmentSlot.enabled = false;
        }

        //leg equipment
        LegEquipmentItem legEquipment = player.playerInventoryManager.legEquipment;
        if (legEquipment != null)
        {
            legEquipmentSlot.enabled = true;
            legEquipmentSlot.sprite = legEquipment.itemIcon;
        }
        else
        {
            legEquipmentSlot.enabled = false;
        }

        //hand equipment
        HandEquipmentItem handEquipment = player.playerInventoryManager.handEquipment;
        if (handEquipment != null)
        {
            handEquipmentSlot.enabled = true;
            handEquipmentSlot.sprite = handEquipment.itemIcon;
        }
        else
        {
            handEquipmentSlot.enabled = false;
        }

        //quick slot 01
        QuickSlotItem quickSlotEquipment01 = player.playerInventoryManager.quickSlotItemsInQuickSlots[0];
        if (quickSlotEquipment01 != null)
        {
            quickSlot01EquipmentSlot.enabled = true;
            quickSlot01EquipmentSlot.sprite = quickSlotEquipment01.itemIcon;

            if (quickSlotEquipment01.isConsumable)
            {
                quickSlot01Count.enabled = true;
                quickSlot01Count.text = quickSlotEquipment01.GetCurrentAmount(player).ToString();
            }
            else
            {
                quickSlot01Count.enabled = false;
            }
        }
        else
        {
            quickSlot01EquipmentSlot.enabled = false;
            quickSlot01Count.enabled = false;
        }

        //quick slot 02
        QuickSlotItem quickSlotEquipment02 = player.playerInventoryManager.quickSlotItemsInQuickSlots[1];
        if (quickSlotEquipment02 != null)
        {
            quickSlot02EquipmentSlot.enabled = true;
            quickSlot02EquipmentSlot.sprite = quickSlotEquipment02.itemIcon;

            if (quickSlotEquipment02.isConsumable)
            {
                quickSlot02Count.enabled = true;
                quickSlot02Count.text = quickSlotEquipment02.GetCurrentAmount(player).ToString();
            }
            else
            {
                quickSlot02Count.enabled = false;
            }
        }
        else
        {
            quickSlot02EquipmentSlot.enabled = false;
            quickSlot02Count.enabled = false;
        }

        //quick slot 03
        QuickSlotItem quickSlotEquipment03 = player.playerInventoryManager.quickSlotItemsInQuickSlots[2];
        if (quickSlotEquipment03 != null)
        {
            quickSlot03EquipmentSlot.enabled = true;
            quickSlot03EquipmentSlot.sprite = quickSlotEquipment03.itemIcon;

            if (quickSlotEquipment03.isConsumable)
            {
                quickSlot03Count.enabled = true;
                quickSlot03Count.text = quickSlotEquipment03.GetCurrentAmount(player).ToString();
            }
            else
            {
                quickSlot03Count.enabled = false;
            }
        }
        else
        {
            quickSlot03EquipmentSlot.enabled = false;
            quickSlot03Count.enabled = false;
        }
    }

    private void ClearEquipmentInventory()
    {
        foreach (Transform item in equipmentInventoryContentWindow)
        {
            Destroy(item.gameObject);
        }
    }

    public void LoadEquipmentInventory()
    {
        ToggleEquipmentButtons(false);
        equipmentInventoryWindow.SetActive(true);

        switch (currentSelectedEquipmentSlot)
        {
            case EquipmentType.RightWeapon01:
                LoadWeaponInventory();
                break;
            case EquipmentType.RightWeapon02:
                LoadWeaponInventory();
                break;
            case EquipmentType.RightWeapon03:
                LoadWeaponInventory();
                break;
            case EquipmentType.LeftWeapon01:
                LoadWeaponInventory();
                break;
            case EquipmentType.LeftWeapon02:
                LoadWeaponInventory();
                break;
            case EquipmentType.LeftWeapon03:
                LoadWeaponInventory();
                break;
            case EquipmentType.Head:
                LoadHeadEquipmentInventory();
                break;
            case EquipmentType.Body:
                LoadBodyEquipmentInventory();
                break;
            case EquipmentType.Legs:
                LoadLegEquipmentInventory();
                break;
            case EquipmentType.Hands:
                LoadHandEquipmentInventory();
                break;
            case EquipmentType.QuickSlot01:
                LoadQuickSlotInventory();
                break;
            case EquipmentType.QuickSlot02:
                LoadQuickSlotInventory();
                break;
            case EquipmentType.QuickSlot03:
                LoadQuickSlotInventory();
                break;
            default:
                break;
        }
    }

    private void LoadWeaponInventory()
    {

        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        List<WeaponItem> weaponInInventory = new List<WeaponItem>();


        for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
        {
            WeaponItem weapon = player.playerInventoryManager.itemsInInventory[i] as WeaponItem;

            if (weapon != null)
                weaponInInventory.Add(weapon);
        }

        if (weaponInInventory.Count <= 0)
        {
            equipmentInventoryWindow.SetActive(false);
            ToggleEquipmentButtons(true);
            RefreshMenu();
            return;
        }

        bool hasSelectedFirstInventorySlot = false;

        for (int i = 0; i < weaponInInventory.Count; i++)
        {
            GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
            UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
            equipmentInventorySlot.AddItem(weaponInInventory[i]);

            if (!hasSelectedFirstInventorySlot)
            {
                hasSelectedFirstInventorySlot = true;
                Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                inventorySlotButton.Select();
                inventorySlotButton.OnSelect(null);
            }

        }
    }

    private void LoadHeadEquipmentInventory()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        List<HeadEquipmentItem> headEquipmentInventory = new List<HeadEquipmentItem>();


        for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
        {
            HeadEquipmentItem equipment = player.playerInventoryManager.itemsInInventory[i] as HeadEquipmentItem;

            if (equipment != null)
                headEquipmentInventory.Add(equipment);
        }

        if (headEquipmentInventory.Count <= 0)
        {
            equipmentInventoryWindow.SetActive(false);
            ToggleEquipmentButtons(true);
            RefreshMenu();
            return;
        }

        bool hasSelectedFirstInventorySlot = false;

        for (int i = 0; i < headEquipmentInventory.Count; i++)
        {
            GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
            UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
            equipmentInventorySlot.AddItem(headEquipmentInventory[i]);

            if (!hasSelectedFirstInventorySlot)
            {
                hasSelectedFirstInventorySlot = true;
                Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                inventorySlotButton.Select();
                inventorySlotButton.OnSelect(null);
            }

        }
    }

    private void LoadBodyEquipmentInventory()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        List<BodyEquipmentItem> bodyEquipmentInventory = new List<BodyEquipmentItem>();


        for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
        {
            BodyEquipmentItem equipment = player.playerInventoryManager.itemsInInventory[i] as BodyEquipmentItem;

            if (equipment != null)
                bodyEquipmentInventory.Add(equipment);
        }

        if (bodyEquipmentInventory.Count <= 0)
        {
            equipmentInventoryWindow.SetActive(false);
            ToggleEquipmentButtons(true);
            RefreshMenu();
            return;
        }

        bool hasSelectedFirstInventorySlot = false;

        for (int i = 0; i < bodyEquipmentInventory.Count; i++)
        {
            GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
            UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
            equipmentInventorySlot.AddItem(bodyEquipmentInventory[i]);

            if (!hasSelectedFirstInventorySlot)
            {
                hasSelectedFirstInventorySlot = true;
                Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                inventorySlotButton.Select();
                inventorySlotButton.OnSelect(null);
            }

        }
    }

    private void LoadLegEquipmentInventory()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        List<LegEquipmentItem> legEquipmentInventory = new List<LegEquipmentItem>();


        for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
        {
            LegEquipmentItem equipment = player.playerInventoryManager.itemsInInventory[i] as LegEquipmentItem;

            if (equipment != null)
                legEquipmentInventory.Add(equipment);
        }

        if (legEquipmentInventory.Count <= 0)
        {
            equipmentInventoryWindow.SetActive(false);
            ToggleEquipmentButtons(true);
            RefreshMenu();
            return;
        }

        bool hasSelectedFirstInventorySlot = false;

        for (int i = 0; i < legEquipmentInventory.Count; i++)
        {
            GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
            UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
            equipmentInventorySlot.AddItem(legEquipmentInventory[i]);

            if (!hasSelectedFirstInventorySlot)
            {
                hasSelectedFirstInventorySlot = true;
                Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                inventorySlotButton.Select();
                inventorySlotButton.OnSelect(null);
            }

        }
    }

    private void LoadHandEquipmentInventory()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        List<HandEquipmentItem> handEquipmentInventory = new List<HandEquipmentItem>();


        for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
        {
            HandEquipmentItem equipment = player.playerInventoryManager.itemsInInventory[i] as HandEquipmentItem;

            if (equipment != null)
                handEquipmentInventory.Add(equipment);
        }

        if (handEquipmentInventory.Count <= 0)
        {
            equipmentInventoryWindow.SetActive(false);
            ToggleEquipmentButtons(true);
            RefreshMenu();
            return;
        }

        bool hasSelectedFirstInventorySlot = false;

        for (int i = 0; i < handEquipmentInventory.Count; i++)
        {
            GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
            UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
            equipmentInventorySlot.AddItem(handEquipmentInventory[i]);

            if (!hasSelectedFirstInventorySlot)
            {
                hasSelectedFirstInventorySlot = true;
                Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                inventorySlotButton.Select();
                inventorySlotButton.OnSelect(null);
            }

        }
    }

    private void LoadQuickSlotInventory()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        List<QuickSlotItem> quickSlotItemInInventory = new List<QuickSlotItem>();

        for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
        {
            QuickSlotItem quickSlotItem = player.playerInventoryManager.itemsInInventory[i] as QuickSlotItem;

            if (quickSlotItem != null)
                quickSlotItemInInventory.Add(quickSlotItem);
        }

        if (quickSlotItemInInventory.Count <= 0)
        {
            equipmentInventoryWindow.SetActive(false);
            ToggleEquipmentButtons(true);
            RefreshMenu();
            return;
        }

        bool hasSelectedFirstInventorySlot = false;

        for (int i = 0; i < quickSlotItemInInventory.Count; i++)
        {
            GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
            UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
            equipmentInventorySlot.AddItem(quickSlotItemInInventory[i]);

            if (!hasSelectedFirstInventorySlot)
            {
                hasSelectedFirstInventorySlot = true;
                Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                inventorySlotButton.Select();
                inventorySlotButton.OnSelect(null);
            }

        }
    }

    public void SelectEquipmentSlot(int equipmentSlot)
    {
        currentSelectedEquipmentSlot = (EquipmentType)equipmentSlot;
    }

    public void UnEquipSelectedItem()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
        Item unequippedItem;
        switch (currentSelectedEquipmentSlot)
        {
            case EquipmentType.RightWeapon01:
                unequippedItem = player.playerInventoryManager.weaponInRightHandSlot[0];
                if (unequippedItem != null)
                {
                    player.playerInventoryManager.weaponInRightHandSlot[0] = Instantiate(WorldItemDatabase.instance.unarmedWeapon);

                    if (unequippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                }
                if (player.playerInventoryManager.rightHandWeaponIndex == 0)
                    player.playerNetworkManager.currentRightHandWeaponID.Value = WorldItemDatabase.instance.unarmedWeapon.itemID;

                break;
            case EquipmentType.RightWeapon02:
                unequippedItem = player.playerInventoryManager.weaponInRightHandSlot[1];
                if (unequippedItem != null)
                {
                    player.playerInventoryManager.weaponInRightHandSlot[1] = Instantiate(WorldItemDatabase.instance.unarmedWeapon);

                    if (unequippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                }
                if (player.playerInventoryManager.rightHandWeaponIndex == 1)
                    player.playerNetworkManager.currentRightHandWeaponID.Value = WorldItemDatabase.instance.unarmedWeapon.itemID;
                break;
            case EquipmentType.RightWeapon03:
                unequippedItem = player.playerInventoryManager.weaponInRightHandSlot[2];
                if (unequippedItem != null)
                {
                    player.playerInventoryManager.weaponInRightHandSlot[2] = Instantiate(WorldItemDatabase.instance.unarmedWeapon);

                    if (unequippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                }
                if (player.playerInventoryManager.rightHandWeaponIndex == 2)
                    player.playerNetworkManager.currentRightHandWeaponID.Value = WorldItemDatabase.instance.unarmedWeapon.itemID;
                break;
            case EquipmentType.LeftWeapon01:
                unequippedItem = player.playerInventoryManager.weaponInLeftHandSlot[0];
                if (unequippedItem != null)
                {
                    player.playerInventoryManager.weaponInLeftHandSlot[0] = Instantiate(WorldItemDatabase.instance.unarmedWeapon);

                    if (unequippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                }
                if (player.playerInventoryManager.leftHandWeaponIndex == 0)
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = WorldItemDatabase.instance.unarmedWeapon.itemID;
                break;
            case EquipmentType.LeftWeapon02:
                unequippedItem = player.playerInventoryManager.weaponInLeftHandSlot[1];
                if (unequippedItem != null)
                {
                    player.playerInventoryManager.weaponInLeftHandSlot[1] = Instantiate(WorldItemDatabase.instance.unarmedWeapon);

                    if (unequippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                }
                if (player.playerInventoryManager.leftHandWeaponIndex == 1)
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = WorldItemDatabase.instance.unarmedWeapon.itemID;
                break;
            case EquipmentType.LeftWeapon03:
                unequippedItem = player.playerInventoryManager.weaponInLeftHandSlot[2];
                if (unequippedItem != null)
                {
                    player.playerInventoryManager.weaponInLeftHandSlot[2] = Instantiate(WorldItemDatabase.instance.unarmedWeapon);

                    if (unequippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                }
                if (player.playerInventoryManager.leftHandWeaponIndex == 2)
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = WorldItemDatabase.instance.unarmedWeapon.itemID;
                break;
            case EquipmentType.Head:
                unequippedItem = player.playerInventoryManager.headEquipment;

                if (unequippedItem != null)
                    player.playerInventoryManager.AddItemToInventory(unequippedItem);

                player.playerInventoryManager.headEquipment = null;
                player.playerEquipmentManager.LoadHeadEquipment(player.playerInventoryManager.headEquipment);
                break;
            case EquipmentType.Body:
                unequippedItem = player.playerInventoryManager.bodyEquipment;

                if (unequippedItem != null)
                    player.playerInventoryManager.AddItemToInventory(unequippedItem);

                player.playerInventoryManager.bodyEquipment = null;
                player.playerEquipmentManager.LoadBodyEquipment(player.playerInventoryManager.bodyEquipment);
                break;
            case EquipmentType.Legs:
                unequippedItem = player.playerInventoryManager.legEquipment;

                if (unequippedItem != null)
                    player.playerInventoryManager.AddItemToInventory(unequippedItem);

                player.playerInventoryManager.legEquipment = null;
                player.playerEquipmentManager.LoadLegEquipment(player.playerInventoryManager.legEquipment);
                break;
            case EquipmentType.Hands:
                unequippedItem = player.playerInventoryManager.handEquipment;

                if (unequippedItem != null)
                    player.playerInventoryManager.AddItemToInventory(unequippedItem);

                player.playerInventoryManager.handEquipment = null;
                player.playerEquipmentManager.LoadHandEquipment(player.playerInventoryManager.handEquipment);
                break;
            case EquipmentType.QuickSlot01:
                unequippedItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[0];

                if (unequippedItem != null)
                    player.playerInventoryManager.AddItemToInventory(unequippedItem);

                player.playerInventoryManager.quickSlotItemsInQuickSlots[0] = null;

                if (player.playerInventoryManager.quickSlotItemIndex == 0)
                    player.playerNetworkManager.currentQuickSlotItemID.Value = -1;

                break;
            case EquipmentType.QuickSlot02:
                unequippedItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[1];

                if (unequippedItem != null)
                    player.playerInventoryManager.AddItemToInventory(unequippedItem);

                player.playerInventoryManager.quickSlotItemsInQuickSlots[1] = null;

                if (player.playerInventoryManager.quickSlotItemIndex == 1)
                    player.playerNetworkManager.currentQuickSlotItemID.Value = -1;

                break;
            case EquipmentType.QuickSlot03:
                unequippedItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[2];

                if (unequippedItem != null)
                    player.playerInventoryManager.AddItemToInventory(unequippedItem);

                player.playerInventoryManager.quickSlotItemsInQuickSlots[2] = null;

                if (player.playerInventoryManager.quickSlotItemIndex == 2)
                    player.playerNetworkManager.currentQuickSlotItemID.Value = -1;

                break;
            default:
                break;
        }

        RefreshMenu();
    }

}
