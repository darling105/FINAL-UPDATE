using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIToggleHud : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerUIManager.instance.playerUIHudManager.ToggleHUD(false);
    }

    private void OnDisable()
    {
        PlayerUIManager.instance.playerUIHudManager.ToggleHUD(true);
    }
}
