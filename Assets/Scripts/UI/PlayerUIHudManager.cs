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

    [Header("SHADE")]
    [SerializeField] float shadeUpdateCountDelayTimer = 2.5f;
    private int pendingShadesToAdd = 0;
    private Coroutine waitThenAddShadesCoroutine;
    [SerializeField] TextMeshProUGUI shadesToAddText;
    [SerializeField] TextMeshProUGUI shadesCountText;

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

    public void SetShadesCount(int shadesToAdd)
    {
        pendingShadesToAdd += shadesToAdd;

        if (waitThenAddShadesCoroutine != null)
            StopCoroutine(waitThenAddShadesCoroutine);

        waitThenAddShadesCoroutine = StartCoroutine(WaitThenUpdateShadesCount());

        //shadesCountText.text = PlayerUIManager.instance.localPlayer.playerStatsManager.shades.ToString();
    }

    private IEnumerator WaitThenUpdateShadesCount()
    {
        float timer = shadeUpdateCountDelayTimer;
        int shadesToAdd = pendingShadesToAdd;

        if (shadesToAdd >= 0)
        {
            shadesToAddText.text = "+ " + shadesToAdd.ToString();

        }
        else
        {
            shadesToAddText.text = "- " + Mathf.Abs(shadesToAdd).ToString();
        }

        shadesCountText.enabled = true;
        while (timer > 0)
        {
            timer -= Time.deltaTime;

            if (shadesToAdd != pendingShadesToAdd)
            {
                shadesToAdd = pendingShadesToAdd;
                shadesToAddText.text = "+ " + shadesToAdd.ToString();
            }
            yield return null;
        }

        shadesToAddText.enabled = false;
        pendingShadesToAdd = 0;
        shadesCountText.text = PlayerUIManager.instance.localPlayer.playerStatsManager.shades.ToString();

        yield return null;
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
            quickSlotItemCount.text = quickSlotItem.GetCurrentAmount(PlayerUIManager.instance.localPlayer).ToString();
            quickSlotItemCount.enabled = true;
        }
        else
        {
            quickSlotItemCount.enabled = false;
        }
    }

}
