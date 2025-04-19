using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBossCharacterStatsManager : CharacterStatsManager
{
    [SerializeField] AIBossCharacterManager bossCharacter;


    protected override void Awake()
    {
        base.Awake();
        bossCharacter = GetComponent<AIBossCharacterManager>();
    }

    protected override void Start()
    {
        base.Start();
    }
}
