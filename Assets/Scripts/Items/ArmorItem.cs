using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorItem : EquipmentItem
{
    [Header("Equipment Absorption Bonus")]
    public float physicalDamageAbsorption;
    public float fireDamageAbsorption;
    public float magicDamageAbsorption;
    public float lightningDamageAbsorption;
    public float holyDamageAbsorption;

    [Header("Equipment Resistances Bonus")]
    public float immunity;  //rot and poisen
    public float robustness; //frost and bleed
    public float focus; // madness and sleep
    public float vitality; //death curse

    [Header("Poise")]
    public float poise;

    public EquipmentModel[] equipmentModels;

}
