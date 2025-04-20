using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AICharacterNetworkManager : CharacterNetworkManager
{
    AICharacterManager aiCharacter;
    protected override void Awake()
    {
        base.Awake();

        aiCharacter = GetComponent<AICharacterManager>();
    }

    public override void OnIsDeadChanged(bool oldStatus, bool newStatus)
    {
        base.OnIsDeadChanged(oldStatus, newStatus);

        if (aiCharacter.isDead.Value)
        {
            aiCharacter.aiCharacterInventoryManager.DropItem();
            aiCharacter.aiCharacterCombatManager.AwardShadesOnDeath(PlayerUIManager.instance.localPlayer);
        }
    }
}
