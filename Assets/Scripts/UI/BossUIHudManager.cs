using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BossUIHudManager : MonoBehaviour
{
    [SerializeField] AIBossCharacterManager bossCharacter;
    [Header("STAT BARS")]
    [SerializeField] UI_StatBar bossHealthBar;
    [SerializeField] TextMeshProUGUI bossNameText;

    private void Awake()
    {
        bossCharacter  = GetComponent<AIBossCharacterManager>();
    }

    public void SetNewBossHealthValue(float newValue)
    {
        bossHealthBar.SetStat(Mathf.RoundToInt(newValue));
    }

    public void SetMaxBossHealthValue(float maxValue)
    {
        bossHealthBar.SetMaxStat(Mathf.RoundToInt(maxValue));
    }

    public void SetBossName(string bossName)
    {
        bossNameText.text = bossName;
    }
}
