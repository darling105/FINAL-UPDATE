using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGameSessionManager : MonoBehaviour
{
    public static WorldGameSessionManager instance;

    [Header("Active Player In Session")]
    public List<PlayerManager> players = new List<PlayerManager>();

    private Coroutine reviveCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void WaitThenRevivePlayer()
    {
        if (reviveCoroutine != null)
            StopCoroutine(reviveCoroutine);

        reviveCoroutine = StartCoroutine(RevivePlayerCoroutine(4));
    }

    private IEnumerator RevivePlayerCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        PlayerUIManager.instance.playerUILoadingScreenManager.ActivateLoadingScreen();

        PlayerUIManager.instance.localPlayer.ReviveCharacter();

        for (int i = 0; i < WorldObjectManager.instance.energySites.Count; i++)
        {
            if (WorldObjectManager.instance.energySites[i].energySiteID == WorldSaveGameManager.instance.currentCharacterData.lastEnergySiteRestedAt)
            {
                WorldObjectManager.instance.energySites[i].TeleportToEnergySite();
                break;
            }
        }
    }

    public void AddPlayerToActivePlayerList(PlayerManager player)
    {
        if (!players.Contains(player))
        {
            players.Add(player);
        }

        for (int i = players.Count - 1; i > -1; i--)
        {
            if (players[i] == null)
            {
                players.RemoveAt(i);
            }
        }
    }

    public void RemovePlayerFromActivePlayerList(PlayerManager player)
    {
        if (players.Contains(player))
        {
            players.Remove(player);
        }

        for (int i = players.Count - 1; i > -1; i++)
        {
            if (players[i] == null)
            {
                players.RemoveAt(i);
            }
        }
    }
}
