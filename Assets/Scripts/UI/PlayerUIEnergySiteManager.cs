using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIEnergySiteManager : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] GameObject menu;


    public void OpenEnergySiteManagerMenu()
    {
        PlayerUIManager.instance.menuWindowIsOpen = true;
        menu.SetActive(true);
    }

    public void CloseEnergySiteManagerMenu()
    {
        PlayerUIManager.instance.menuWindowIsOpen = false;
        menu.SetActive(false);
    }

    public void OpenTeleportLocationMenu()
    {
        CloseEnergySiteManagerMenu();
        PlayerUIManager.instance.playerUITeleportLocationManager.OpenTeleportLocationManagerMenu();
    }

}
