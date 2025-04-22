using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnergySiteInteractable : Interactable
{
    [Header("Energy Site")]
    public int energySiteID;

    [Header("VFX")]
    [SerializeField] GameObject activatedParticles;

    [Header("Active")]
    public NetworkVariable<bool> isActivated = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Interaction Text")]
    [SerializeField] string unactivatedInteractionText = "Restore Energy Site";
    [SerializeField] string activatedInteractionText = "Rest";

    [Header("Teleport Transform")]
    [SerializeField] Transform teleportTransform;

    protected override void Start()
    {
        base.Start();

        if (IsOwner)
        {
            if (WorldSaveGameManager.instance.currentCharacterData.energySites.ContainsKey(energySiteID))
            {
                isActivated.Value = WorldSaveGameManager.instance.currentCharacterData.energySites[energySiteID];
            }
            else
            {
                isActivated.Value = false;
            }
        }

        if (isActivated.Value)
        {
            activatedParticles.SetActive(true);
            interactableText = activatedInteractionText;
        }
        else
        {
            activatedParticles.SetActive(false);
            interactableText = unactivatedInteractionText;
        }

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsOwner)
            OnIsActiveChanged(false, isActivated.Value);

        isActivated.OnValueChanged += OnIsActiveChanged;

        WorldObjectManager.instance.AddEnergySiteToList(this);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        isActivated.OnValueChanged -= OnIsActiveChanged;
    }

    private void RestoreEnergySite(PlayerManager player)
    {
        isActivated.Value = true;

        if (WorldSaveGameManager.instance.currentCharacterData.energySites.ContainsKey(energySiteID))
            WorldSaveGameManager.instance.currentCharacterData.energySites.Remove(energySiteID);

        WorldSaveGameManager.instance.currentCharacterData.energySites.Add(energySiteID, true);

        player.playerAnimatorManager.PlayTargetActionAnimation("Activate_Energy_Site_01", true);

        PlayerUIManager.instance.playerUIPopUpManager.SendEnergySiteRestorPopUp("ENERGY SITE RESTORED");

        WorldSaveGameManager.instance.SaveGame();

        StartCoroutine(WaitForAnimationAndPopUpThenRestoreCollider());

    }

    private void RestSiteOfGrace(PlayerManager player)
    {
        PlayerUIManager.instance.playerUIEnergySiteManager.OpenMenu();

        interactableCollider.enabled = true;
        player.playerNetworkManager.currentHealth.Value = player.playerNetworkManager.maxHealth.Value;
        player.playerNetworkManager.currentStamina.Value = player.playerNetworkManager.maxStamina.Value;
        player.playerNetworkManager.currentFocusPoints.Value = player.playerNetworkManager.maxFocusPoints.Value;
        player.playerNetworkManager.remainingHealthFlasks.Value = 3;
        player.playerNetworkManager.remainingFocusPointFlasks.Value = 3;

        WorldAIManager.instance.ResetAllCharacters();
        WorldSaveGameManager.instance.SaveGame();
    }

    private IEnumerator WaitForAnimationAndPopUpThenRestoreCollider()
    {
        yield return new WaitForSeconds(2);
        interactableCollider.enabled = true;
    }

    private void OnIsActiveChanged(bool oldStatus, bool newStatus)
    {
        if (isActivated.Value)
        {
            activatedParticles.SetActive(true);
            interactableText = activatedInteractionText;
        }
        else
        {
            activatedParticles.SetActive(false);
            interactableText = unactivatedInteractionText;
        }
    }

    public override void Interact(PlayerManager player)
    {
        base.Interact(player);

        if (player.isPerformingAction)
            return;
        if (player.playerCombatManager.isUsingItem)
            return;

        WorldSaveGameManager.instance.currentCharacterData.lastEnergySiteRestedAt = energySiteID;

        if (!isActivated.Value)
        {
            RestoreEnergySite(player);
        }
        else
        {
            RestSiteOfGrace(player);
        }
    }

    public void TeleportToEnergySite()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        PlayerUIManager.instance.playerUILoadingScreenManager.ActivateLoadingScreen();

        player.transform.position = teleportTransform.position;

        PlayerUIManager.instance.playerUILoadingScreenManager.DeactivateLoadingScreen();
    }

}
