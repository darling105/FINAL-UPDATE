using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Netcode;

public class WorldAIManager : MonoBehaviour
{
    public static WorldAIManager instance;

    [Header("Loading")]
    public bool isPerformingLoadingOperation = false;

    [Header("Characters")]
    [SerializeField] List<AICharacterSpawner> aiCharacterSpawners;
    [SerializeField] List<AICharacterManager> spawnedInCharacters;
    private Coroutine spawnAllCharactersCoroutine;
    private Coroutine despawnAllCharactersCoroutine;
    private Coroutine resetAllCharactersCoroutine;

    [Header("Bosses")]
    [SerializeField] List<AIBossCharacterManager> spawnedInBoss;


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

    public void SpawnCharacter(AICharacterSpawner aiCharacterSpawner)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            aiCharacterSpawners.Add(aiCharacterSpawner);
            aiCharacterSpawner.AttemptToSpawnCharacter();
        }
    }

    public void AddCharacterToSpawnedCharactersList(AICharacterManager character)
    {
        if (spawnedInCharacters.Contains(character))
            return;

        spawnedInCharacters.Add(character);

        AIBossCharacterManager bossCharacter = character as AIBossCharacterManager;
        if (bossCharacter != null)
        {
            if (spawnedInBoss.Contains(bossCharacter))
                return;

            spawnedInBoss.Add(bossCharacter);
        }
    }

    public AIBossCharacterManager GetBossCharacterByID(int ID)
    {
        return spawnedInBoss.FirstOrDefault(boss => boss.bossID == ID);
    }


    public void SpawnAllCharacters()
    {
        isPerformingLoadingOperation = true;

        if (spawnAllCharactersCoroutine != null)
            StopCoroutine(spawnAllCharactersCoroutine);

        spawnAllCharactersCoroutine = StartCoroutine(SpawnAllCharactersCoroutine());

    }

    private IEnumerator SpawnAllCharactersCoroutine()
    {
        for (int i = 0; i < aiCharacterSpawners.Count; i++)
        {
            yield return new WaitForFixedUpdate();

            aiCharacterSpawners[i].AttemptToSpawnCharacter();

            yield return null;
        }

        isPerformingLoadingOperation = false;

        yield return null;
    }

    public void ResetAllCharacters()
    {
        isPerformingLoadingOperation = true;

        if (resetAllCharactersCoroutine != null)
            StopCoroutine(resetAllCharactersCoroutine);

        resetAllCharactersCoroutine = StartCoroutine(ResetAllCharactersCoroutine());

    }
    private IEnumerator ResetAllCharactersCoroutine()
    {
        for (int i = 0; i < spawnedInCharacters.Count; i++)
        {
            yield return new WaitForFixedUpdate();

            aiCharacterSpawners[i].ResetCharacter();

            yield return null;
        }

        isPerformingLoadingOperation = false;

        yield return null;
    }

    private void DespawnAllCharacters()
    {
        isPerformingLoadingOperation = true;

        if (spawnAllCharactersCoroutine != null)
            StopCoroutine(spawnAllCharactersCoroutine);

        spawnAllCharactersCoroutine = StartCoroutine(SpawnAllCharactersCoroutine());
    }

    private IEnumerator DespawnAllCharactersCoroutine()
    {
        for (int i = 0; i < spawnedInCharacters.Count; i++)
        {
            yield return new WaitForFixedUpdate();

            spawnedInCharacters[i].GetComponent<NetworkObject>().Despawn();

            yield return null;
        }

        spawnedInCharacters.Clear();
        spawnedInBoss.Clear();
        isPerformingLoadingOperation = false;

        yield return null;
    }

    private void DisableAllCharacters()
    {

    }

}
