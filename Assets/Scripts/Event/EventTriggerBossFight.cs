using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerBossFight : MonoBehaviour
{
    [SerializeField] int bossID;

    private void Start()
    {
        AIBossCharacterManager boss = WorldAIManager.instance.GetBossCharacterByID(bossID);

        if (boss != null && boss.hasBeenDefeated.Value)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        AIBossCharacterManager boss = WorldAIManager.instance.GetBossCharacterByID(bossID);

        if (boss != null)
        {
            boss.WakeBoss();
        }
    }
}
