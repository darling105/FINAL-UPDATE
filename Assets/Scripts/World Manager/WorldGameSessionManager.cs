using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGameSessionManager : MonoBehaviour
{
    public static WorldGameSessionManager instance;

    [Header("Active Player In Session")]
    public List<PlayerManager> players = new List<PlayerManager>();

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
