using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CharacterUIManager : MonoBehaviour
{
    [Header("UI")]
    public bool hasFloatingHPBar = true;
    public UI_Character_HP_Bar characterHPBar;

    private void Awake()
    {
        characterHPBar = GetComponentInChildren<UI_Character_HP_Bar>();
    }

    public void OnHPChanged(int oldValue, int newValue)
    {
        characterHPBar.oldHealthValue = oldValue;
        characterHPBar.SetStat(newValue);
    }

    public void ResetCharacterHPBar()
    {
        if (characterHPBar == null)
            return;

        characterHPBar.currentDamageTaken =0;
    }
}
