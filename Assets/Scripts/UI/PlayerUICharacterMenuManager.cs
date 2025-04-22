using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;

public class PlayerUICharacterMenuManager : PlayerUIMenu
{
    
    public void ReturnToDeskTop()
    {
        NetworkManager.Singleton.Shutdown();

        Application.Quit();
    }

}
