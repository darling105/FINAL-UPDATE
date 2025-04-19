using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapons/Melee Weapon")]
public class MeleeWeaponItem : WeaponItem
{
    public float riposte_Attack_01_Modifier = 3.3f;
    public float backstab_Attack_01_Modifier = 3.2f;
}
