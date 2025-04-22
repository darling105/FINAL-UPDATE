using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUILevelUpManager : PlayerUIMenu
{
    [Header("Levels")]
    [SerializeField] int[] playerLevels = new int[693];
    [SerializeField] int baseLevelCost = 83;
    [SerializeField] int totalLevelUpCost = 0;

    [Header("Character Stats")]
    [SerializeField] TextMeshProUGUI characterLevelText;
    [SerializeField] TextMeshProUGUI shadesHeldText;
    [SerializeField] TextMeshProUGUI shadesNeededText;
    [SerializeField] TextMeshProUGUI vitalityLevelText;
    [SerializeField] TextMeshProUGUI mindLevelText;
    [SerializeField] TextMeshProUGUI enduranceLevelText;
    [SerializeField] TextMeshProUGUI strengthLevelText;
    [SerializeField] TextMeshProUGUI dexterityLevelText;
    [SerializeField] TextMeshProUGUI intelligenceLevelText;
    [SerializeField] TextMeshProUGUI faithLevelText;

    [Header("Projected Character Stats")]
    [SerializeField] TextMeshProUGUI projectedCharacterLevelText;
    [SerializeField] TextMeshProUGUI projectedShadesHeldText;
    [SerializeField] TextMeshProUGUI projectedVitalityLevelText;
    [SerializeField] TextMeshProUGUI projectedMindLevelText;
    [SerializeField] TextMeshProUGUI projectedEnduranceLevelText;
    [SerializeField] TextMeshProUGUI projectedStrengthLevelText;
    [SerializeField] TextMeshProUGUI projectedDexterityLevelText;
    [SerializeField] TextMeshProUGUI projectedIntelligenceLevelText;
    [SerializeField] TextMeshProUGUI projectedFaithLevelText;

    [Header("Slider")]
    public Attribute currentSelectedAttribute;
    public Slider vitalityLevelSlider;
    public Slider mindLevelSlider;
    public Slider enduranceLevelSlider;
    public Slider strengthLevelSlider;
    public Slider dexterityLevelSlider;
    public Slider intelligenceLevelSlider;
    public Slider faithLevelSlider;

    [Header("Button")]
    [SerializeField] Button confirmButton;

    private void Awake()
    {
        SetAllLevelsCost();
    }

    public override void OpenMenu()
    {
        base.OpenMenu();

        SetCurrentStats();
    }

    private void SetCurrentStats()
    {
        characterLevelText.text = PlayerUIManager.instance.localPlayer.characterStatsManager.CalculateCharacterLevelBasedOnStats().ToString();
        projectedCharacterLevelText.text = PlayerUIManager.instance.localPlayer.characterStatsManager.CalculateCharacterLevelBasedOnStats().ToString();

        shadesHeldText.text = PlayerUIManager.instance.localPlayer.playerStatsManager.shades.ToString();
        projectedShadesHeldText.text = PlayerUIManager.instance.localPlayer.playerStatsManager.shades.ToString();
        shadesNeededText.text = "0";

        vitalityLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.vitality.Value.ToString();
        projectedVitalityLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.vitality.Value.ToString();
        vitalityLevelSlider.minValue = PlayerUIManager.instance.localPlayer.playerNetworkManager.vitality.Value;

        mindLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.mind.Value.ToString();
        projectedMindLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.mind.Value.ToString();
        mindLevelSlider.minValue = PlayerUIManager.instance.localPlayer.playerNetworkManager.mind.Value;

        enduranceLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.endurance.Value.ToString();
        projectedEnduranceLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.endurance.Value.ToString();
        enduranceLevelSlider.minValue = PlayerUIManager.instance.localPlayer.playerNetworkManager.endurance.Value;

        strengthLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.strength.Value.ToString();
        projectedStrengthLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.strength.Value.ToString();
        strengthLevelSlider.minValue = PlayerUIManager.instance.localPlayer.playerNetworkManager.strength.Value;

        dexterityLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.dexterity.Value.ToString();
        projectedDexterityLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.dexterity.Value.ToString();
        dexterityLevelSlider.minValue = PlayerUIManager.instance.localPlayer.playerNetworkManager.dexterity.Value;

        intelligenceLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.intelligence.Value.ToString();
        projectedIntelligenceLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.intelligence.Value.ToString();
        intelligenceLevelSlider.minValue = PlayerUIManager.instance.localPlayer.playerNetworkManager.intelligence.Value;

        faithLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.faith.Value.ToString();
        projectedFaithLevelText.text = PlayerUIManager.instance.localPlayer.playerNetworkManager.faith.Value.ToString();
        faithLevelSlider.minValue = PlayerUIManager.instance.localPlayer.playerNetworkManager.faith.Value;

        vitalityLevelSlider.Select();
        vitalityLevelSlider.OnSelect(null);
        mindLevelSlider.Select();
        mindLevelSlider.OnSelect(null);
        enduranceLevelSlider.Select();
        enduranceLevelSlider.OnSelect(null);
        strengthLevelSlider.Select();
        strengthLevelSlider.OnSelect(null);
        dexterityLevelSlider.Select();
        dexterityLevelSlider.OnSelect(null);
        intelligenceLevelSlider.Select();
        intelligenceLevelSlider.OnSelect(null);
        faithLevelSlider.Select();
        faithLevelSlider.OnSelect(null);
    }

    public void UpdateSliderBasedOnCurrentlySelectedAttributes()
    {
        PlayerManager player = PlayerUIManager.instance.localPlayer;

        switch (currentSelectedAttribute)
        {
            case Attribute.Vitality:
                projectedVitalityLevelText.text = vitalityLevelSlider.value.ToString();
                break;
            case Attribute.Mind:
                projectedMindLevelText.text = mindLevelSlider.value.ToString();
                break;
            case Attribute.Endurance:
                projectedEnduranceLevelText.text = enduranceLevelSlider.value.ToString();
                break;
            case Attribute.Strength:
                projectedStrengthLevelText.text = strengthLevelSlider.value.ToString();
                break;
            case Attribute.Dexterity:
                projectedDexterityLevelText.text = dexterityLevelSlider.value.ToString();
                break;
            case Attribute.Intelligence:
                projectedIntelligenceLevelText.text = intelligenceLevelSlider.value.ToString();
                break;
            case Attribute.Faith:
                projectedFaithLevelText.text = faithLevelSlider.value.ToString();
                break;
            default:
                break;
        }

        CalculateLevelCost(
            player.characterStatsManager.CalculateCharacterLevelBasedOnStats(),
            player.characterStatsManager.CalculateCharacterLevelBasedOnStats(true));

        projectedCharacterLevelText.text = player.characterStatsManager.CalculateCharacterLevelBasedOnStats(true).ToString();
        shadesNeededText.text = totalLevelUpCost.ToString();

        if (totalLevelUpCost > player.playerStatsManager.shades)
        {
            confirmButton.interactable = false;
        }
        else
        {
            confirmButton.interactable = true;
        }

        ChangeTextColorsDependingOnCosts();
    }

    public void ConfirmLevels()
    {
        PlayerManager player = PlayerUIManager.instance.localPlayer;

        player.playerStatsManager.shades -= totalLevelUpCost;

        player.playerNetworkManager.vitality.Value = Mathf.RoundToInt(vitalityLevelSlider.value);
        player.playerNetworkManager.mind.Value = Mathf.RoundToInt(mindLevelSlider.value);
        player.playerNetworkManager.endurance.Value = Mathf.RoundToInt(enduranceLevelSlider.value);
        player.playerNetworkManager.strength.Value = Mathf.RoundToInt(strengthLevelSlider.value);
        player.playerNetworkManager.dexterity.Value = Mathf.RoundToInt(dexterityLevelSlider.value);
        player.playerNetworkManager.intelligence.Value = Mathf.RoundToInt(intelligenceLevelSlider.value);
        player.playerNetworkManager.faith.Value = Mathf.RoundToInt(faithLevelSlider.value);

        SetCurrentStats();
        ChangeTextColorsDependingOnCosts();

        WorldSaveGameManager.instance.SaveGame();
    }

    public void SetAllLevelsCost()
    {
        for (int i = 0; i < playerLevels.Length; i++)
        {
            if (i == 0)
                continue;

            playerLevels[i] = baseLevelCost + (50 * i);
        }
    }

    private void CalculateLevelCost(int currentLevel, int projectedLevel)
    {
        int totalCost = 0;
        for (int i = 0; i < projectedLevel; i++)
        {
            if (i < currentLevel)
                continue;

            if (i > playerLevels.Length)
                continue;

            totalCost += playerLevels[i];
        }

        totalLevelUpCost = totalCost;

        projectedShadesHeldText.text = (PlayerUIManager.instance.localPlayer.playerStatsManager.shades - totalCost).ToString();

        if (totalCost > PlayerUIManager.instance.localPlayer.playerStatsManager.shades)
        {
            projectedShadesHeldText.color = Color.red;
        }
        else
        {
            projectedShadesHeldText.color = Color.white;
        }

    }

    private void ChangeTextColorsDependingOnCosts()
    {
        PlayerManager player = PlayerUIManager.instance.localPlayer;

        int projectedVitalityLevel = Mathf.RoundToInt(vitalityLevelSlider.value);
        int projectedMindLevel = Mathf.RoundToInt(mindLevelSlider.value);
        int projectedEnduranceLevel = Mathf.RoundToInt(enduranceLevelSlider.value);
        int projectedStrengthLevel = Mathf.RoundToInt(strengthLevelSlider.value);
        int projectedDexterityLevel = Mathf.RoundToInt(dexterityLevelSlider.value);
        int projectedIntelligenceLevel = Mathf.RoundToInt(intelligenceLevelSlider.value);
        int projectedFaithLevel = Mathf.RoundToInt(faithLevelSlider.value);

        ChangeTextFieldToSpecificColorBasedOnStats(player, projectedVitalityLevelText, player.playerNetworkManager.vitality.Value, projectedVitalityLevel);
        ChangeTextFieldToSpecificColorBasedOnStats(player, projectedMindLevelText, player.playerNetworkManager.mind.Value, projectedMindLevel);
        ChangeTextFieldToSpecificColorBasedOnStats(player, projectedEnduranceLevelText, player.playerNetworkManager.endurance.Value, projectedEnduranceLevel);
        ChangeTextFieldToSpecificColorBasedOnStats(player, projectedStrengthLevelText, player.playerNetworkManager.strength.Value, projectedStrengthLevel);
        ChangeTextFieldToSpecificColorBasedOnStats(player, projectedDexterityLevelText, player.playerNetworkManager.dexterity.Value, projectedDexterityLevel);
        ChangeTextFieldToSpecificColorBasedOnStats(player, projectedIntelligenceLevelText, player.playerNetworkManager.intelligence.Value, projectedIntelligenceLevel);
        ChangeTextFieldToSpecificColorBasedOnStats(player, projectedFaithLevelText, player.playerNetworkManager.faith.Value, projectedFaithLevel);

        int projectedPlayerLevel = player.characterStatsManager.CalculateCharacterLevelBasedOnStats(true);
        int playerLevel = player.characterStatsManager.CalculateCharacterLevelBasedOnStats();

        if (projectedPlayerLevel == playerLevel)
        {
            projectedShadesHeldText.color = Color.white;
            projectedCharacterLevelText.color = Color.white;
            shadesNeededText.color = Color.white;
        }

        if (totalLevelUpCost <= player.playerStatsManager.shades)
        {
            shadesNeededText.color = Color.white;

            if (projectedPlayerLevel > playerLevel)
            {
                projectedShadesHeldText.color = Color.red;
                projectedCharacterLevelText.color = Color.blue;
            }

        }
        else
        {
            shadesNeededText.color = Color.red;
            if (projectedPlayerLevel > playerLevel)
                projectedCharacterLevelText.color = Color.red;
        }

    }

    private void ChangeTextFieldToSpecificColorBasedOnStats(PlayerManager player, TextMeshProUGUI textField, int stat, int projectedStat)
    {

        if (projectedStat == stat)
            textField.color = Color.white;


        if (totalLevelUpCost <= player.playerStatsManager.shades)
        {
            if (projectedStat > stat)
            {
                textField.color = Color.blue;
            }
            else
            {
                textField.color = Color.white;
            }
        }
        else
        {
            if (projectedStat > stat)
            {
                textField.color = Color.red;
            }
            else
            {
                textField.color = Color.white;
            }
        }
    }

}
