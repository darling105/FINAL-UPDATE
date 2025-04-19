using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    public PlayerManager player;
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    protected override void Start()
    {
        base.Start();
        CalculateHealhtBasedOnVitalityLevel(player.playerNetworkManager.vitality.Value);
        CalculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value);
        CalculateFocusPointsBasedOnMindLevel(player.playerNetworkManager.mind.Value);
    }

    public void CalculateTotalArmorAbsorption()
    {
        armorPhysicalDamageAbsorption = 0;
        armorFireDamageAbsorption = 0;
        armorMagicDamageAbsorption = 0;
        armorLightningDamageAbsorption = 0;
        armorHolyDamageAbsorption = 0;

        armorImmunity = 0;
        armorRobustness = 0;
        armorFocus = 0;
        armorVitality = 0;

        basePoiseDefense = 0;

        //HEAD EQUIPMENT
        if (player.playerInventoryManager.headEquipment != null)
        {
            armorPhysicalDamageAbsorption += player.playerInventoryManager.headEquipment.physicalDamageAbsorption;
            armorFireDamageAbsorption += player.playerInventoryManager.headEquipment.fireDamageAbsorption;
            armorMagicDamageAbsorption += player.playerInventoryManager.headEquipment.magicDamageAbsorption;
            armorLightningDamageAbsorption += player.playerInventoryManager.headEquipment.lightningDamageAbsorption;
            armorHolyDamageAbsorption += player.playerInventoryManager.headEquipment.holyDamageAbsorption;

            armorImmunity += player.playerInventoryManager.headEquipment.immunity;
            armorRobustness += player.playerInventoryManager.headEquipment.robustness;
            armorFocus += player.playerInventoryManager.headEquipment.focus;
            armorVitality += player.playerInventoryManager.headEquipment.vitality;

            basePoiseDefense += player.playerInventoryManager.headEquipment.poise;
        }
        //BODY EQUIPMENT
        if (player.playerInventoryManager.bodyEquipment != null)
        {
            armorPhysicalDamageAbsorption += player.playerInventoryManager.bodyEquipment.physicalDamageAbsorption;
            armorFireDamageAbsorption += player.playerInventoryManager.bodyEquipment.fireDamageAbsorption;
            armorMagicDamageAbsorption += player.playerInventoryManager.bodyEquipment.magicDamageAbsorption;
            armorLightningDamageAbsorption += player.playerInventoryManager.bodyEquipment.lightningDamageAbsorption;
            armorHolyDamageAbsorption += player.playerInventoryManager.bodyEquipment.holyDamageAbsorption;

            armorImmunity += player.playerInventoryManager.bodyEquipment.immunity;
            armorRobustness += player.playerInventoryManager.bodyEquipment.robustness;
            armorFocus += player.playerInventoryManager.bodyEquipment.focus;
            armorVitality += player.playerInventoryManager.bodyEquipment.vitality;

            basePoiseDefense += player.playerInventoryManager.bodyEquipment.poise;
        }
        //LEGS EQUIPMENT
        if (player.playerInventoryManager.legEquipment != null)
        {
            armorPhysicalDamageAbsorption += player.playerInventoryManager.legEquipment.physicalDamageAbsorption;
            armorFireDamageAbsorption += player.playerInventoryManager.legEquipment.fireDamageAbsorption;
            armorMagicDamageAbsorption += player.playerInventoryManager.legEquipment.magicDamageAbsorption;
            armorLightningDamageAbsorption += player.playerInventoryManager.legEquipment.lightningDamageAbsorption;
            armorHolyDamageAbsorption += player.playerInventoryManager.legEquipment.holyDamageAbsorption;

            armorImmunity += player.playerInventoryManager.legEquipment.immunity;
            armorRobustness += player.playerInventoryManager.legEquipment.robustness;
            armorFocus += player.playerInventoryManager.legEquipment.focus;
            armorVitality += player.playerInventoryManager.legEquipment.vitality;

            basePoiseDefense += player.playerInventoryManager.legEquipment.poise;
        }
        //HAND EQUIPMENT
        if (player.playerInventoryManager.handEquipment != null)
        {
            armorPhysicalDamageAbsorption += player.playerInventoryManager.handEquipment.physicalDamageAbsorption;
            armorFireDamageAbsorption += player.playerInventoryManager.handEquipment.fireDamageAbsorption;
            armorMagicDamageAbsorption += player.playerInventoryManager.handEquipment.magicDamageAbsorption;
            armorLightningDamageAbsorption += player.playerInventoryManager.handEquipment.lightningDamageAbsorption;
            armorHolyDamageAbsorption += player.playerInventoryManager.handEquipment.holyDamageAbsorption;

            armorImmunity += player.playerInventoryManager.handEquipment.immunity;
            armorRobustness += player.playerInventoryManager.handEquipment.robustness;
            armorFocus += player.playerInventoryManager.handEquipment.focus;
            armorVitality += player.playerInventoryManager.handEquipment.vitality;

            basePoiseDefense += player.playerInventoryManager.handEquipment.poise;
        }
    }
}
