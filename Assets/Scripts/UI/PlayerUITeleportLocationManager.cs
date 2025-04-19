using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUITeleportLocationManager : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] GameObject menu;

    [SerializeField] GameObject[] teleportLocations;

    public void OpenTeleportLocationManagerMenu()
    {
        PlayerUIManager.instance.menuWindowIsOpen = true;
        menu.SetActive(true);

        CheckForUnlockedTeleports();
    }

    public void CloseTeleportLocationManagerMenu()
    {
        PlayerUIManager.instance.menuWindowIsOpen = false;
        menu.SetActive(false);
    }

    private void CheckForUnlockedTeleports()
    {
        bool hasFirstSelectedButton = false;
        for (int i = 0; i < teleportLocations.Length; i++)
        {
            for (int j = 0; j < WorldObjectManager.instance.energySites.Count; j++)
            {
                if (WorldObjectManager.instance.energySites[j].energySiteID == i)
                {
                    if (WorldObjectManager.instance.energySites[j].isActivated.Value)
                    {
                        teleportLocations[i].SetActive(true);

                        if (!hasFirstSelectedButton)
                        {
                            hasFirstSelectedButton = true;
                            teleportLocations[i].GetComponent<Button>().Select();
                            teleportLocations[i].GetComponent<Button>().OnSelect(null);
                        }
                    }
                    else
                    {
                        teleportLocations[i].SetActive(false);
                    }
                }
            }
        }
    }

    public void TeleportToEnergySite(int siteID)
    {
        for (int i = 0; i < WorldObjectManager.instance.energySites.Count; i++)
        {
            if (WorldObjectManager.instance.energySites[i].energySiteID == siteID)
            {
                WorldObjectManager.instance.energySites[i].TeleportToEnergySite();
                return;
            }
        }
    }
}
