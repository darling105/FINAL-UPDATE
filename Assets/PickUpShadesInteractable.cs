using UnityEngine;

public class PickUpShadesInteractable : Interactable
{
   public int shadesCount = 0;

    public override void Interact(PlayerManager player)
    {
        WorldSaveGameManager.instance.currentCharacterData.hasDeadSpot = false;
        player.playerStatsManager.AddShades(shadesCount);
        Destroy(gameObject);
    }
}
