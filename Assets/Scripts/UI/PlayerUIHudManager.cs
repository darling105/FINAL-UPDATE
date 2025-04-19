using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class PlayerUIHudManager : MonoBehaviour
{
    [SerializeField] CanvasGroup[] canvasGroup;

    [Header("STAT BARS")]
    [SerializeField] UI_StatBar healthBar;
    [SerializeField] UI_StatBar staminaBar;
    [SerializeField] UI_StatBar focusPointsBar;

    [Header("QUICK SLOTS")]
    [SerializeField] Image rightWeaponQuickSlotIcon;
    [SerializeField] Image leftWeaponQuickSlotIcon;
    [SerializeField] Image quickSlotItemQuickSlotIcon;
    [SerializeField] TextMeshProUGUI quickSlotItemCount;

    [Header("Boss Health Bar")]
    public Transform bossHealthBarParent;
    public GameObject bossHealthBarObject;

    public void ToggleHUD(bool status)
    {
        if (status)
        {
            foreach (var canvas in canvasGroup)
            {
                canvas.alpha = 1;
            }
        }
        else
        {
            foreach (var canvas in canvasGroup)
            {
                canvas.alpha = 0;
            }
        }
    }

    public void RefreshHUD()
    {
        healthBar.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(true);
        staminaBar.gameObject.SetActive(false);
        staminaBar.gameObject.SetActive(true);
        focusPointsBar.gameObject.SetActive(false);
        focusPointsBar.gameObject.SetActive(true);
    }

    public void SetNewStaminaValue(float oldValue, float newValue)
    {
        staminaBar.SetStat(Mathf.RoundToInt(newValue));
    }

    public void SetMaxStaminaValue(float maxStamina)
    {
        staminaBar.SetMaxStat(Mathf.RoundToInt(maxStamina));
    }

    public void SetNewHealthValue(int oldValue, int newValue)
    {
        healthBar.SetStat(newValue);
    }

    public void SetMaxHealthValue(int maxHealth)
    {
        healthBar.SetMaxStat(maxHealth);
    }

    public void SetNewFocusPointsValue(int oldValue, int newValue)
    {
        focusPointsBar.SetStat(newValue);
    }

    public void SetMaxFocusPointsValue(int maxFocusPoints)
    {
        focusPointsBar.SetMaxStat(maxFocusPoints);
    }

    public void SetRightWeaponQuickSlotIcon(int weaponID)
    {
        WeaponItem weapon = WorldItemDatabase.instance.GetWeaponByID(weaponID);
        if (weapon == null)
        {
            Debug.Log("Item is null");
            rightWeaponQuickSlotIcon.enabled = false;
            rightWeaponQuickSlotIcon.sprite = null;
            return;
        }

        if (weapon.itemIcon == null)
        {
            Debug.Log("Item has no icon");
            rightWeaponQuickSlotIcon.enabled = false;
            rightWeaponQuickSlotIcon.sprite = null;
            return;
        }
        rightWeaponQuickSlotIcon.sprite = weapon.itemIcon;
        rightWeaponQuickSlotIcon.enabled = true;
    }

    public void SetLeftWeaponQuickSlotIcon(int weaponID)
    {
        WeaponItem weapon = WorldItemDatabase.instance.GetWeaponByID(weaponID);
        if (weapon == null)
        {
            Debug.Log("Item is null");
            leftWeaponQuickSlotIcon.enabled = false;
            leftWeaponQuickSlotIcon.sprite = null;
            return;
        }

        if (weapon.itemIcon == null)
        {
            Debug.Log("Item has no icon");
            leftWeaponQuickSlotIcon.enabled = false;
            leftWeaponQuickSlotIcon.sprite = null;
            return;
        }
        leftWeaponQuickSlotIcon.sprite = weapon.itemIcon;
        leftWeaponQuickSlotIcon.enabled = true;
    }

    public void SetQuickSlotItemQuickSlotIcon(QuickSlotItem quickSlotItem)
    {
        if (quickSlotItem == null)
        {
            Debug.Log("Item is null");
            quickSlotItemQuickSlotIcon.enabled = false;
            quickSlotItemQuickSlotIcon.sprite = null;
            quickSlotItemCount.enabled = false;
            return;
        }

        if (quickSlotItem.itemIcon == null)
        {
            Debug.Log("Item has no icon");
            quickSlotItemQuickSlotIcon.enabled = false;
            quickSlotItemQuickSlotIcon.sprite = null;
            quickSlotItemCount.enabled = false;
            return;
        }
        quickSlotItemQuickSlotIcon.sprite = quickSlotItem.itemIcon;
        quickSlotItemQuickSlotIcon.enabled = true;

        if (quickSlotItem.isConsumable)
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
            quickSlotItemCount.text = quickSlotItem.GetCurrentAmount(player).ToString();
            quickSlotItemCount.enabled = true;
        }
        else
        {
            quickSlotItemCount.enabled = false;
        }
    }

}
