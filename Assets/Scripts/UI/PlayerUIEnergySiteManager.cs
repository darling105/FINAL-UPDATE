using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIEnergySiteManager : PlayerUIMenu
{

    public void OpenTeleportLocationMenu()
    {
        CloseMenu();
        PlayerUIManager.instance.playerUITeleportLocationManager.OpenMenu();
    }

    public void OpenLevelUpMenu()
    {
        CloseMenu();
        PlayerUIManager.instance.playerUILevelUpManager.OpenMenu();
    }

}
