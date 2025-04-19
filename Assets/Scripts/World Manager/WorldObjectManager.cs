using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WorldObjectManager : MonoBehaviour
{
    public static WorldObjectManager instance;

    [Header("Objects")]
    [SerializeField] List<NetworkObjectSpawner> networkObjectSpawners;
    [SerializeField] List<GameObject> spawnedInObjects;

    [Header("Fog Wall")]
    public List<FogWallInteractable> fogWalls;

    [Header("Energy Sites")]
    public List<EnergySiteInteractable> energySites;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnObject(NetworkObjectSpawner networkObjectSpawner)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            networkObjectSpawners.Add(networkObjectSpawner);
            networkObjectSpawner.AttemptToSpawnObject();
        }

    }

    public void AddFogWallToList(FogWallInteractable fogWall)
    {
        if (!fogWalls.Contains(fogWall))
        {
            fogWalls.Add(fogWall);
        }
    }
    public void RemoveFogWallFromList(FogWallInteractable fogWall)
    {
        if (fogWalls.Contains(fogWall))
        {
            fogWalls.Remove(fogWall);
        }
    }

    public void AddEnergySiteToList(EnergySiteInteractable energySite)
    {
        if (!energySites.Contains(energySite))
        {
            energySites.Add(energySite);
        }
    }
    public void RemoveEnergySiteFromList(EnergySiteInteractable energySite)
    {
        if (energySites.Contains(energySite))
        {
            energySites.Remove(energySite);
        }
    }

}
