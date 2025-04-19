using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUICharacterMenuManager : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] GameObject menu;

    public void OpenCharacterMenu()
    {
        PlayerUIManager.instance.menuWindowIsOpen = true;
        menu.SetActive(true);
    }

    public void CloseCharacterMenu()
    {
        PlayerUIManager.instance.menuWindowIsOpen = false;
        menu.SetActive(false);
    }

    public void CloseCharacterMenuAfterFixedUpdate()
    {
        StartCoroutine(WaitThenCloseMenu());
    }

    private IEnumerator WaitThenCloseMenu()
    {
        yield return new WaitForFixedUpdate();

        PlayerUIManager.instance.menuWindowIsOpen = false;
        menu.SetActive(false);
    }
}
