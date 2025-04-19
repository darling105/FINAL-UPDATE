using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : EquipmentItem
{

    [Header("Animations")]
    public AnimatorOverrideController weaponAnimator;

    [Header("Model Instantiation")]
    public WeaponModelType weaponModelType;

    [Header("Weapon Model")]
    public GameObject weaponModel;

    [Header("Weapon Class")]
    public WeaponClass weaponClass;

    [Header("Weapon Requirements")]
    public int strengthREQ = 0;
    public int dexterityREQ = 0;
    public int intelligenceREQ = 0;
    public int faithREQ = 0;

    [Header("Weapon Base Damage")]
    public int physicalDamage = 0;
    public int magicDamage = 0;
    public int fireDamage = 0;
    public int lightningDamage = 0;
    public int holyDamage = 0;

    [Header("Weapon Base Poise Damage")]
    public int poiseDamage = 10;

    [Header("Attack Modifiers")]
    public float light_Attack_01_Modifier = 1.0f;
    public float light_Attack_02_Modifier = 1.2f;
    public float light_Jumping_Attack_01_Modifier = 1.3f;
    public float heavy_Attack_01_Modifier = 1.4f;
    public float heavy_Attack_02_Modifier = 1.5f;
    public float heavy_Jumping_Attack_01_Modifier = 1.6f;
    public float charge_Attack_01_Modifier = 2.0f;
    public float charge_Attack_02_Modifier = 2.1f;
    public float run_Attack_01_Modifier = 1.5f;
    public float roll_Attack_01_Modifier = 1.3f;
    public float backstep_Attack_01_Modifier = 1.3f;


    [Header("Stamina Cost Modifiers")]
    public int baseStaminaCost = 20;
    public float lightAttackStaminaMultiplier = 0.9f;
    public float heavyAttackStaminaMultiplier = 1.2f;
    public float chargedAttackStaminaMultiplier = 1.5f;
    public float runningAttackStaminaMultiplier = 1.5f;
    public float rollingAttackStaminaMultiplier = 1.5f;
    public float backstepAttackStaminaMultiplier = 1.5f;
    

    [Header("Weapon Blocking Absorption")]
    public float physicalBlockAbsorption = 50;
    public float magicBlockAbsorption = 50;
    public float fireBlockAbsorption = 50;
    public float lightningBlockAbsorption = 50;
    public float holyBlockAbsorption = 50;
    public float stabilty = 50;

    [Header("Actions")]
    public WeaponItemAction oh_RB_Action;
    public WeaponItemAction oh_RT_Action;
    public WeaponItemAction oh_LB_Action;
    public AshOfWar ashOfWarAction;

    [Header("SFX")]
    public AudioClip[] whooshes;
    public AudioClip[] blockingSFX;

}
