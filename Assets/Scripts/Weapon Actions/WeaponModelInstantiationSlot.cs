using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModelInstantiationSlot : MonoBehaviour
{
    public WeaponModelSlot weaponSlot;
    public GameObject currentWeaponModel;

    public void UnloadWeaponModel()
    {
        if (currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }
    }

    public void PlaceWeaponModelIntoSlot(GameObject weaponModel)
    {
        currentWeaponModel = weaponModel;
        weaponModel.transform.parent = transform;

        weaponModel.transform.localPosition = Vector3.zero;
        weaponModel.transform.localRotation = Quaternion.identity;
        weaponModel.transform.localScale = Vector3.one;
    }

    public void PlaceWeaponModelInUnequipSlot(GameObject weaponModel, WeaponClass weaponClass, PlayerManager player)
    {
        currentWeaponModel = weaponModel;
        weaponModel.transform.parent = transform;

        switch (weaponClass)
        {
            case WeaponClass.StraightSword:
                weaponModel.transform.localPosition = new Vector3(-0.2059076f, 0.00509131f, 0.1334106f);
                weaponModel.transform.localRotation = Quaternion.Euler(-164.622f, 88.29f, -9.574005f);
                break;
            case WeaponClass.Shield:
                weaponModel.transform.localPosition = new Vector3(0.166f, 0.013f, 0.039f);
                weaponModel.transform.localRotation = Quaternion.Euler(-143.086f, 161.716f, -28.242f);
                break;
            default:
                break;
        }
    }

}
