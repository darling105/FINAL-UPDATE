using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class AshOfWar : Item
{
    [Header("Ash Of War")]
    public WeaponClass[] usableWeaponClasses;

    [Header("Costs")]
    public int focusPointCost = 20;
    public int staminaCost = 20;

    public virtual void AttemptToPerformAction(PlayerManager playerPerformingAction)
    {
        Debug.Log("PERFORMED!");
    }

    public virtual bool CanIUseThisAbility(PlayerManager playerPerformingAction)
    {
        return false;
    }

    protected virtual void DeductStaminaCost(PlayerManager playerPerformingAction)
    {
        playerPerformingAction.playerNetworkManager.currentStamina.Value -= staminaCost;
    }

    protected virtual void DeductFocusPointCost(PlayerManager playerPerformingAction)
    {
       
    }
}
