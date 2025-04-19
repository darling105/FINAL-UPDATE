using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager character;


    [Header("Stamina Regeneration")]
    [SerializeField] float staminaRegenerationAmount = 2;
    private float staminaTickTimer = 0;
    private float staminaRegenerationTimer = 0;
    [SerializeField] float staminaRegenerationDelay = 2f;

    [Header("Blocking Absorption")]
    public float blockingPhysicalAbsorption;
    public float blockingFireAbsorption;
    public float blockingMagicAbsorption;
    public float blockingLightningAbsorption;
    public float blockingHolyAbsorption;
    public float blockingStability;

    [Header("Armor Absorption")]
    public float armorPhysicalDamageAbsorption;
    public float armorFireDamageAbsorption;
    public float armorMagicDamageAbsorption;
    public float armorLightningDamageAbsorption;
    public float armorHolyDamageAbsorption;

    [Header("Armor Resistances")]
    public float armorImmunity;  //rot and poisen
    public float armorRobustness; //frost and bleed
    public float armorFocus; // madness and sleep
    public float armorVitality; //death curse
    

    [Header("Poise")]
    public float totalPoiseDamage;
    public float offensivePoiseDamage;
    public float basePoiseDefense;
    public float defaultPoiseResetTime = 8;
    public float poiseResetTimer = 0;


    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        HandlePoiseResetTimer();
    }

    public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
    {
        float stamina = 0;

        stamina = endurance * 10f; // 10 stamina per endurance level

        return Mathf.RoundToInt(stamina);
    }

    public int CalculateHealhtBasedOnVitalityLevel(int vitality)
    {
        float health = 0;

        health = vitality * 15; // 5 stamina per vitality level

        return Mathf.RoundToInt(health);
    }

    public int CalculateFocusPointsBasedOnMindLevel(int mind)
    {
        float focusPoints = 0;

        focusPoints = mind * 10; // 5 focus points per mind level

        return Mathf.RoundToInt(focusPoints);
    }

    public virtual void RegenerateStamina()
    {
        if (!character.IsOwner)
            return;

        if (character.characterNetworkManager.isSprinting.Value)
            return;

        if (character.isPerformingAction)
            return;

        staminaRegenerationTimer += Time.deltaTime;

        if (staminaRegenerationTimer >= staminaRegenerationDelay)
        {
            if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
            {
                staminaTickTimer += Time.deltaTime;
                if (staminaTickTimer >= 0.1f)
                {
                    staminaTickTimer = 0;
                    character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                }
            }
        }
    }

    public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount)
    {
        if (currentStaminaAmount < previousStaminaAmount)
            staminaRegenerationTimer = 0;
    }

    protected virtual void HandlePoiseResetTimer()
    {
        if (poiseResetTimer > 0)
        {
            poiseResetTimer -= Time.deltaTime;
        }
        else
        {
            totalPoiseDamage = 0;
        }
    }

}
