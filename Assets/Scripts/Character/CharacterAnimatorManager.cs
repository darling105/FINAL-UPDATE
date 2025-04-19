using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterAnimatorManager : MonoBehaviour
{
    [Header("Flags")]
    public bool applyRootMotion = false;
    CharacterManager character;
    int vertical;
    int horizontal;

    [Header("Damage Animations")]
    public string lastDamageAnimationPlayed;

    //medium damage animations
    [SerializeField] string hit_Forward_Medium_01 = "Hit_Forward_Medium_01";
    [SerializeField] string hit_Forward_Medium_02 = "Hit_Forward_Medium_02";

    [SerializeField] string hit_Backward_Medium_01 = "Hit_Backward_Medium_01";
    [SerializeField] string hit_Backward_Medium_02 = "Hit_Backward_Medium_02";

    [SerializeField] string hit_Left_Medium_01 = "Hit_Left_Medium_01";
    [SerializeField] string hit_Left_Medium_02 = "Hit_Left_Medium_02";

    [SerializeField] string hit_Right_Medium_01 = "Hit_Right_Medium_01";
    [SerializeField] string hit_Right_Medium_02 = "Hit_Right_Medium_02";

    public List<string> forward_Medium_Damage = new List<string>();
    public List<string> backward_Medium_Damage = new List<string>();
    public List<string> left_Medium_Damage = new List<string>();
    public List<string> right_Medium_Damage = new List<string>();

    //ping damage animations
    [SerializeField] string hit_Forward_Ping_01 = "Hit_Forward_Ping_01";
    [SerializeField] string hit_Forward_Ping_02 = "Hit_Forward_Ping_02";

    [SerializeField] string hit_Backward_Ping_01 = "Hit_Backward_Ping_01";
    [SerializeField] string hit_Backward_Ping_02 = "Hit_Backward_Ping_02";

    [SerializeField] string hit_Left_Ping_01 = "Hit_Left_Ping_01";
    [SerializeField] string hit_Left_Ping_02 = "Hit_Left_Ping_02";

    [SerializeField] string hit_Right_Ping_01 = "Hit_Right_Ping_01";
    [SerializeField] string hit_Right_Ping_02 = "Hit_Right_Ping_02";

    public List<string> forward_Ping_Damage = new List<string>();
    public List<string> backward_Ping_Damage = new List<string>();
    public List<string> left_Ping_Damage = new List<string>();
    public List<string> right_Ping_Damage = new List<string>();

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();

        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }


    protected virtual void Start()
    {
        //medium damage animations
        forward_Medium_Damage.Add(hit_Forward_Medium_01);
        forward_Medium_Damage.Add(hit_Forward_Medium_02);
        backward_Medium_Damage.Add(hit_Backward_Medium_01);
        backward_Medium_Damage.Add(hit_Backward_Medium_02);
        left_Medium_Damage.Add(hit_Left_Medium_01);
        left_Medium_Damage.Add(hit_Left_Medium_02);
        right_Medium_Damage.Add(hit_Right_Medium_01);
        right_Medium_Damage.Add(hit_Right_Medium_02);

        //ping damage animations
        forward_Ping_Damage.Add(hit_Forward_Ping_01);
        forward_Ping_Damage.Add(hit_Forward_Ping_02);
        backward_Ping_Damage.Add(hit_Backward_Ping_01);
        backward_Ping_Damage.Add(hit_Backward_Ping_02);
        left_Ping_Damage.Add(hit_Left_Ping_01);
        left_Ping_Damage.Add(hit_Left_Ping_02);
        right_Ping_Damage.Add(hit_Right_Ping_01);
        right_Ping_Damage.Add(hit_Right_Ping_02);
    }

    public string GetRandomAnimationFromList(List<string> animationList)
    {
        List<string> finalList = new List<string>();

        foreach (var item in animationList)
        {
            finalList.Add(item);
        }

        finalList.Remove(lastDamageAnimationPlayed);

        for (int i = finalList.Count - 1; i > -1; i--)
        {
            if (finalList[i] == null)
            {
                finalList.RemoveAt(i);
            }
        }

        int randomValue = Random.Range(0, finalList.Count);
        return finalList[randomValue];
    }

    public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        float snappedHorizontal;
        float snappedVertical;

        //HORIZONTAL MOVEMENT
        if (horizontalMovement > 0 && horizontalMovement <= 0.5f)
        {
            snappedHorizontal = 0.5f;
        }
        else if (horizontalMovement > 0.5f && horizontalMovement <= 1f)
        {
            snappedHorizontal = 1f;
        }
        else if (horizontalMovement < 0 && horizontalMovement >= -0.5f)
        {
            snappedHorizontal = -0.5f;
        }
        else if (horizontalMovement < -0.5f && horizontalMovement >= -1f)
        {
            snappedHorizontal = -1;
        }
        else
        {
            snappedHorizontal = 0;
        }

        //VERTICAL MOVEMENT
        if (verticalMovement > 0 && verticalMovement <= 0.5f)
        {
            snappedVertical = 0.5f;
        }
        else if (verticalMovement > 0.5f && verticalMovement <= 1f)
        {
            snappedVertical = 1f;
        }
        else if (verticalMovement < 0 && verticalMovement >= -0.5f)
        {
            snappedVertical = -0.5f;
        }
        else if (verticalMovement < -0.5f && verticalMovement >= -1f)
        {
            snappedVertical = -1;
        }
        else
        {
            snappedVertical = 0;
        }

        if (isSprinting)
        {
            snappedVertical = 2;
        }

        character.animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        character.animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
    }

    public virtual void PlayTargetActionAnimation(
        string targetAnimation,
        bool isPerformingAction,
        bool applyRootMotion = true,
        bool canRotate = false,
        bool canMove = false,
        bool canRun =true,
        bool canRoll = false)
    {
        character.characterAnimatorManager.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);

        character.isPerformingAction = isPerformingAction;
        character.characterLocomotionManager.canRotate = canRotate;
        character.characterLocomotionManager.canMove = canMove;
        character.characterLocomotionManager.canRun = canRun;
        character.characterLocomotionManager.canRoll = canRoll;

        character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    }

 public virtual void PlayTargetActionAnimationInstantly(
        string targetAnimation,
        bool isPerformingAction,
        bool applyRootMotion = true,
        bool canRotate = false,
        bool canMove = false,
        bool canRun =true,
        bool canRoll = false)
    {
        character.characterAnimatorManager.applyRootMotion = applyRootMotion;
        character.animator.Play(targetAnimation);

        character.isPerformingAction = isPerformingAction;
        character.characterLocomotionManager.canRotate = canRotate;
        character.characterLocomotionManager.canMove = canMove;
        character.characterLocomotionManager.canRun = canRun;
        character.characterLocomotionManager.canRoll = canRoll;

        character.characterNetworkManager.NotifyTheServerOfInstantActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    }

    public virtual void PlayTargetAttackActionAnimation(
        WeaponItem weapon,
        AttackType attackType,
        string targetAnimation,
        bool isPerformingAction,
        bool applyRootMotion = true,
        bool canRotate = false,
        bool canMove = false,
        bool canRoll = false)
    {
        character.characterCombatManager.currentAttackType = attackType;
        character.characterCombatManager.lastAttackAnimationPerformed = targetAnimation;
        UpdateAnimatorController(weapon.weaponAnimator);
        this.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        character.isPerformingAction = isPerformingAction;
        character.characterLocomotionManager.canRotate = canRotate;
        character.characterLocomotionManager.canMove = canMove;
        character.characterNetworkManager.isAttacking.Value = true;
        character.characterLocomotionManager.canRoll = canRoll;

        character.characterNetworkManager.NotifyTheServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    }

    public void UpdateAnimatorController(AnimatorOverrideController weaponController)
    {
        character.animator.runtimeAnimatorController = weaponController;
    }



}
