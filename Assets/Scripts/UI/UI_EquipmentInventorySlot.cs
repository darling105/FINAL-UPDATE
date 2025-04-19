using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class UI_EquipmentInventorySlot : MonoBehaviour
{
    public Image itemIcon;
    public Image highlightedIcon;
    [SerializeField] public Item currentItem;

    public void AddItem(Item item)
    {
        if (item == null)
        {
            itemIcon.enabled = false;
            return;
        }

        itemIcon.enabled = true;

        currentItem = item;
        itemIcon.sprite = item.itemIcon;

    }

    public void SelectSlot()
    {
        highlightedIcon.enabled = true;
    }

    public void DeselectSlot()
    {
        highlightedIcon.enabled = false;
    }

    public void EquipItem()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
        Item equippedItem;

        switch (PlayerUIManager.instance.playerUIEquipmentManager.currentSelectedEquipmentSlot)
        {
            case EquipmentType.RightWeapon01:
                equippedItem = player.playerInventoryManager.weaponInRightHandSlot[0];
                if (equippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }

                player.playerInventoryManager.weaponInRightHandSlot[0] = currentItem as WeaponItem;

                player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                if (player.playerInventoryManager.rightHandWeaponIndex == 0)
                    player.playerNetworkManager.currentRightHandWeaponID.Value = currentItem.itemID;

                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

                break;
            case EquipmentType.RightWeapon02:
                equippedItem = player.playerInventoryManager.weaponInRightHandSlot[1];

                if (equippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }

                player.playerInventoryManager.weaponInRightHandSlot[1] = currentItem as WeaponItem;
                player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                if (player.playerInventoryManager.rightHandWeaponIndex == 1)
                    player.playerNetworkManager.currentRightHandWeaponID.Value = currentItem.itemID;

                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;
            case EquipmentType.RightWeapon03:
                equippedItem = player.playerInventoryManager.weaponInRightHandSlot[2];

                if (equippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }

                player.playerInventoryManager.weaponInRightHandSlot[2] = currentItem as WeaponItem;
                player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                if (player.playerInventoryManager.rightHandWeaponIndex == 2)
                    player.playerNetworkManager.currentRightHandWeaponID.Value = currentItem.itemID;

                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;
            case EquipmentType.LeftWeapon01:
                equippedItem = player.playerInventoryManager.weaponInLeftHandSlot[0];

                if (equippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }

                player.playerInventoryManager.weaponInLeftHandSlot[0] = currentItem as WeaponItem;
                player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                if (player.playerInventoryManager.leftHandWeaponIndex == 0)
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = currentItem.itemID;

                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;
            case EquipmentType.LeftWeapon02:
                equippedItem = player.playerInventoryManager.weaponInLeftHandSlot[1];

                if (equippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }

                player.playerInventoryManager.weaponInLeftHandSlot[1] = currentItem as WeaponItem;
                player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                if (player.playerInventoryManager.leftHandWeaponIndex == 1)
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = currentItem.itemID;

                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;
            case EquipmentType.LeftWeapon03:
                equippedItem = player.playerInventoryManager.weaponInLeftHandSlot[2];

                if (equippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }

                player.playerInventoryManager.weaponInLeftHandSlot[2] = currentItem as WeaponItem;
                player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                if (player.playerInventoryManager.leftHandWeaponIndex == 2)
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = currentItem.itemID;

                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;

            case EquipmentType.Head:
                equippedItem = player.playerInventoryManager.headEquipment;
                if (equippedItem != null)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }
                player.playerInventoryManager.headEquipment = currentItem as HeadEquipmentItem;
                player.playerInventoryManager.RemoveItemFromInventory(currentItem);
                player.playerEquipmentManager.LoadHeadEquipment(player.playerInventoryManager.headEquipment);
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;
            case EquipmentType.Body:
                equippedItem = player.playerInventoryManager.bodyEquipment;
                if (equippedItem != null)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }
                player.playerInventoryManager.bodyEquipment = currentItem as BodyEquipmentItem;
                player.playerInventoryManager.RemoveItemFromInventory(currentItem);
                player.playerEquipmentManager.LoadBodyEquipment(player.playerInventoryManager.bodyEquipment);
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;

            case EquipmentType.Legs:
                equippedItem = player.playerInventoryManager.legEquipment;
                if (equippedItem != null)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }
                player.playerInventoryManager.legEquipment = currentItem as LegEquipmentItem;
                player.playerInventoryManager.RemoveItemFromInventory(currentItem);
                player.playerEquipmentManager.LoadLegEquipment(player.playerInventoryManager.legEquipment);
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;
            case EquipmentType.Hands:
                equippedItem = player.playerInventoryManager.handEquipment;
                if (equippedItem != null)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }
                player.playerInventoryManager.handEquipment = currentItem as HandEquipmentItem;
                player.playerInventoryManager.RemoveItemFromInventory(currentItem);
                player.playerEquipmentManager.LoadHandEquipment(player.playerInventoryManager.handEquipment);
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;
            case EquipmentType.QuickSlot01:

                equippedItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[0];

                if (equippedItem != null)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }
                player.playerInventoryManager.quickSlotItemsInQuickSlots[0] = currentItem as QuickSlotItem;

                player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                if (player.playerInventoryManager.quickSlotItemIndex == 0)
                    player.playerNetworkManager.currentQuickSlotItemID.Value = currentItem.itemID;

                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

                break;
            case EquipmentType.QuickSlot02:
                equippedItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[1];

                if (equippedItem != null)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }
                player.playerInventoryManager.quickSlotItemsInQuickSlots[1] = currentItem as QuickSlotItem;

                player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                if (player.playerInventoryManager.quickSlotItemIndex == 1)
                    player.playerNetworkManager.currentQuickSlotItemID.Value = currentItem.itemID;

                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

                break;
            case EquipmentType.QuickSlot03:
                equippedItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[2];

                if (equippedItem != null)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }
                player.playerInventoryManager.quickSlotItemsInQuickSlots[2] = currentItem as QuickSlotItem;

                player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                if (player.playerInventoryManager.quickSlotItemIndex == 2)
                    player.playerNetworkManager.currentQuickSlotItemID.Value = currentItem.itemID;

                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

                break;
            default:
                break;
        }

        PlayerUIManager.instance.playerUIEquipmentManager.SelectLastSelectedEquipmentSlot();

    }
}
