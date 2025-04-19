using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUtilityManager : MonoBehaviour
{
    public static WorldUtilityManager instance;

    [SerializeField] LayerMask characterLayers;
    [SerializeField] LayerMask enviroLayers;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public LayerMask GetCharacterLayers()
    {
        return characterLayers;
    }

    public LayerMask GetEnviroLayers()
    {
        return enviroLayers;
    }

    public bool CanIDamageThisTarget(CharacterGroup attackingCharacter, CharacterGroup targetCharacter)
    {
        if (attackingCharacter == CharacterGroup.Team01)
        {
            switch (targetCharacter)
            {
                case CharacterGroup.Team01: return false;
                case CharacterGroup.Team02: return true;
                default:
                    break;
            }
        }
        else if (attackingCharacter == CharacterGroup.Team02)
        {
            switch (targetCharacter)
            {
                case CharacterGroup.Team01: return true;
                case CharacterGroup.Team02: return false;
                default:
                    break;
            }
        }

        return false;
    }

    public float GetAngleOfTarget(Transform characterTransform, Vector3 targetsDirection)
    {
        targetsDirection.y = 0;
        float viewAbleAngle = Vector3.Angle(characterTransform.forward, targetsDirection);
        Vector3 cross = Vector3.Cross(characterTransform.forward, targetsDirection);

        if (cross.y < 0)
            viewAbleAngle = -viewAbleAngle;

        return viewAbleAngle;

    }

    public DamageIntensity GetDamageIntensityBasedOnPoiseDamage(float poiseDamage)
    {
        DamageIntensity damageIntensity = DamageIntensity.Ping;

        if (poiseDamage >= 10)
            damageIntensity = DamageIntensity.Light;

        if (poiseDamage >= 30)
            damageIntensity = DamageIntensity.Medium;

        if (poiseDamage >= 70)
            damageIntensity = DamageIntensity.Heavy;

        return damageIntensity;
    }

    public Vector3 GetRipostingPositionBasedOnWeaponClass(WeaponClass weaponClass)
    {
        Vector3 position = new Vector3(0f, 0f, 0f);

        switch (weaponClass)
        {
            case WeaponClass.StraightSword:
                break;
            case WeaponClass.Shield:
                break;
            default:
                break;
        }

        return position;
    }

    public Vector3 GetBackstabbingPositionBasedOnWeaponClass(WeaponClass weaponClass)
    {
        Vector3 position = new Vector3(0f, 0f, 0f);
        switch (weaponClass)
        {
            case WeaponClass.StraightSword:
                break;
            case WeaponClass.Shield:
                break;
            default:
                break;
        }

        return position;
    }

}
