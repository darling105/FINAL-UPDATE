using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetIsChugging : StateMachineBehaviour
{
    PlayerManager player;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null)
            player = animator.GetComponent<PlayerManager>();

        if (player == null)
            return;

        if (player.playerNetworkManager.isChugging.Value && player.IsOwner)
        {
            FlaskItem currentFlask = player.playerInventoryManager.currentQuickSlotItem as FlaskItem;

            if (currentFlask.healthFlask)
            {
                if (player.playerNetworkManager.remainingHealthFlasks.Value <= 0)
                {
                    player.playerAnimatorManager.PlayTargetActionAnimation(currentFlask.emptyFlaskAnimation, false, false, true, true, false);
                    player.playerNetworkManager.HideWeaponsServerRpc();
                }
            }
            else
            {
                if (player.playerNetworkManager.remainingFocusPointFlasks.Value <= 0)
                {
                    player.playerAnimatorManager.PlayTargetActionAnimation(currentFlask.emptyFlaskAnimation, false, false, true, true, false);
                    player.playerNetworkManager.HideWeaponsServerRpc();
                }
            }
        }

        if (player.playerNetworkManager.isChugging.Value)
        {
            FlaskItem currentFlask = player.playerInventoryManager.currentQuickSlotItem as FlaskItem;

            if (currentFlask.healthFlask)
            {
                if (player.playerNetworkManager.remainingHealthFlasks.Value <= 0)
                {
                    Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
                    GameObject emptyFlask = Instantiate(currentFlask.emptyFlaskItem, player.playerEquipmentManager.rightHandWeaponSlot.transform);
                    player.playerEffectsManager.activeQuickSlotItemFX = emptyFlask;
                }
            }
            else
            {
                if (player.playerNetworkManager.remainingFocusPointFlasks.Value <= 0)
                {
                    Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
                    GameObject emptyFlask = Instantiate(currentFlask.emptyFlaskItem, player.playerEquipmentManager.rightHandWeaponSlot.transform);
                    player.playerEffectsManager.activeQuickSlotItemFX = emptyFlask;
                }
            }
        }

        if (player.IsOwner)
            player.playerNetworkManager.isChugging.Value = false;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
