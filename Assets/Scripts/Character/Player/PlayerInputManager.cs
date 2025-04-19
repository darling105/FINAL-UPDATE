using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    public PlayerManager player;
    PlayerControls playerControls;

    [Header("PLAYER MOVEMENT INPUT")]
    [SerializeField] Vector2 movementInput;
    public float vertical_Input;
    public float horizontal_Input;
    public float moveAmount;

    [Header("CAMERA MOVEMENT INPUT")]
    [SerializeField] Vector2 camera_Input;
    public float cameraVertical_Input;
    public float cameraHorizontal_Input;

    [Header("LOCK ON INPUT")]
    [SerializeField] bool lockOn_Input;
    [SerializeField] bool lockOn_Left_Input;
    [SerializeField] bool lockOn_Right_Input;
    private Coroutine lockOnCoroutine;

    [Header("PLAYER ACTION INPUT")]
    [SerializeField] bool dodge_Input = false;
    [SerializeField] bool sprint_Input = false;
    [SerializeField] bool jump_Input = false;
    [SerializeField] bool switch_Right_Weapon_Input = false;
    [SerializeField] bool switch_Left_Weapon_Input = false;
    [SerializeField] bool switch_Quick_Slot_Item_Input = false;
    [SerializeField] bool interaction_Input = false;
    [SerializeField] bool use_item_Input = false;

    [Header("BUMPER INPUTS")]
    [SerializeField] bool RB_Input = false;
    [SerializeField] bool LB_Input = false;

    [Header("TRIGGER INPUTS")]
    [SerializeField] bool RT_Input = false;
    [SerializeField] bool Hold_RT_Input = false;
    [SerializeField] bool LT_Input = false;

    [Header("TwO HANDING INPUTS")]
    [SerializeField] bool two_Hand_Input = false;
    [SerializeField] bool two_Hand_Right_Weapon_Input = false;
    [SerializeField] bool two_Hand_Left_Weapon_Input = false;

    [Header("QUED INPUTS")]
    [SerializeField] private bool input_Que_Is_Active = false;
    [SerializeField] float que_Input_Timer = 0;
    [SerializeField] float default_Que_Input_Timer = 0.35f;
    [SerializeField] bool que_RB_Input = false;
    [SerializeField] bool que_RT_Input = false;

    [Header("UI INPUTS")]
    [SerializeField] bool openCharacterMenuInput = false;
    [SerializeField] bool closeMenuInput = false;


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

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        instance.enabled = false;
        SceneManager.activeSceneChanged += OnSceneChange;

        if (playerControls == null)
        {
            playerControls.Disable();
        }
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        {
            instance.enabled = true;

            if (playerControls == null)
            {
                playerControls.Enable();
            }
        }
        else
        {
            instance.enabled = false;

            if (playerControls == null)
            {
                playerControls.Disable();
            }
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            //MOVEMENT
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.Movement.performed += i => camera_Input = i.ReadValue<Vector2>();

            //ACTIONS
            playerControls.PlayerActions.Dodge.performed += i => dodge_Input = true;
            playerControls.PlayerActions.Sprint.performed += i => sprint_Input = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprint_Input = false;
            playerControls.PlayerActions.Jump.performed += i => jump_Input = true;
            playerControls.PlayerActions.SwitchRightWeapon.performed += i => switch_Right_Weapon_Input = true;
            playerControls.PlayerActions.SwitchLeftWeapon.performed += i => switch_Left_Weapon_Input = true;
            playerControls.PlayerActions.SwitchQuickSlotItem.performed += i => switch_Quick_Slot_Item_Input = true;
            playerControls.PlayerActions.Interaction.performed += i => interaction_Input = true;
            playerControls.PlayerActions.Consumables.performed += i => use_item_Input = true;

            //BUMPER
            playerControls.PlayerActions.RB.performed += i => RB_Input = true;
            playerControls.PlayerActions.LB.performed += i => LB_Input = true;
            playerControls.PlayerActions.LB.canceled += i => player.playerNetworkManager.isBlocking.Value = false;

            //TRIGGER
            playerControls.PlayerActions.RT.performed += i => RT_Input = true;
            playerControls.PlayerActions.HoldRT.performed += i => Hold_RT_Input = true;
            playerControls.PlayerActions.HoldRT.canceled += i => Hold_RT_Input = false;
            playerControls.PlayerActions.LT.performed += i => LT_Input = true;

            //TWO HANDING
            playerControls.PlayerActions.TwoHand.performed += i => two_Hand_Input = true;
            playerControls.PlayerActions.TwoHand.canceled += i => two_Hand_Input = false;
            playerControls.PlayerActions.TwoHandRightWeapon.performed += i => two_Hand_Right_Weapon_Input = true;
            playerControls.PlayerActions.TwoHandRightWeapon.canceled += i => two_Hand_Right_Weapon_Input = false;
            playerControls.PlayerActions.TwoHandLeftWeapon.performed += i => two_Hand_Left_Weapon_Input = true;
            playerControls.PlayerActions.TwoHandLeftWeapon.canceled += i => two_Hand_Left_Weapon_Input = false;

            //LOCK ON   
            playerControls.PlayerActions.LockOn.performed += i => lockOn_Input = true;
            playerControls.PlayerActions.SeekLeftLockOnTarget.performed += i => lockOn_Left_Input = true;
            playerControls.PlayerActions.SeekRightLockOnTarget1.performed += i => lockOn_Right_Input = true;

            //QUED INPUTS
            playerControls.PlayerActions.QueRB.performed += i => QueInput(ref que_RB_Input);
            playerControls.PlayerActions.QueRT.performed += i => QueInput(ref que_RT_Input);

            //UI INPUTS
            playerControls.PlayerActions.CloseMenuInput.performed += i => closeMenuInput = true;
            playerControls.PlayerActions.OpenCharacterMenu.performed += i => openCharacterMenuInput = true;

        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (enabled)
        {
            if (focus)
            {
                playerControls.Enable();
            }
            else
            {
                playerControls.Disable();
            }
        }
    }

    private void Update()
    {
        HandleAllInput();
    }

    private void HandleAllInput()
    {
        HandleUseItemInput();
        HandleTwoHandInput();
        HandleLockOnInput();
        HandleLockOnSwitchTargetInput();
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleDodgeInput();
        HandleSprintInput();
        HandleJumpInput();
        HandleRBInput();
        HandleLBInput();
        HandleRTInput();
        HandleLTInput();
        HandleChargeRTInput();
        HandleSwitchRightWeaponInput();
        HandleSwitchLeftWeaponInput();
        HandleSwitchQuickSlotItemInput();
        HandleQuedInputs();
        HandleInteractionInput();
        HandleOpenCharacterMenuInput();
        HandleCloseMenuInput();
    }

    private void HandleUseItemInput()
    {
        if (use_item_Input)
        {
            use_item_Input = false;

            if (PlayerUIManager.instance.menuWindowIsOpen)
                return;

            if (player.playerInventoryManager.currentQuickSlotItem != null)
            {
                player.playerInventoryManager.currentQuickSlotItem.AttemptToUseItem(player);

                player.playerNetworkManager.NotifyServerOfQuickSlotItemActionServerRpc(
                    NetworkManager.Singleton.LocalClientId, player.playerInventoryManager.currentQuickSlotItem.itemID);
            }

        }
    }

    private void HandleTwoHandInput()
    {
        if (PlayerUIManager.instance.menuWindowIsOpen)
            return;
        if (!two_Hand_Input)
            return;

        if (two_Hand_Right_Weapon_Input)
        {
            RB_Input = false;
            two_Hand_Right_Weapon_Input = false;
            player.playerNetworkManager.isBlocking.Value = false;

            if (player.playerNetworkManager.isTwoHandingWeapon.Value)
            {
                player.playerNetworkManager.isTwoHandingWeapon.Value = false;
                return;
            }
            else
            {
                player.playerNetworkManager.isTwoHandingRightWeapon.Value = true;
                return;
            }
        }
        else if (two_Hand_Left_Weapon_Input)
        {
            LB_Input = false;
            two_Hand_Left_Weapon_Input = false;
            player.playerNetworkManager.isBlocking.Value = false;

            if (player.playerNetworkManager.isTwoHandingWeapon.Value)
            {
                player.playerNetworkManager.isTwoHandingWeapon.Value = false;
                return;
            }
            else
            {
                player.playerNetworkManager.isTwoHandingLeftWeapon.Value = true;
                return;
            }
        }
    }

    private void HandleLockOnInput()
    {
        if (player.playerNetworkManager.isLockedOn.Value)
        {
            if (player.playerCombatManager.currentTarget == null)
                return;

            if (player.playerCombatManager.currentTarget.isDead.Value)
            {
                player.playerNetworkManager.isLockedOn.Value = false;
            }

            if (lockOnCoroutine != null)
                StopCoroutine(lockOnCoroutine);

            lockOnCoroutine = StartCoroutine(PlayerCamera.instance.WaitThenFindNewTarget());

        }
        if (lockOn_Input && player.playerNetworkManager.isLockedOn.Value)
        {
            lockOn_Input = false;
            PlayerCamera.instance.ClearLockOnTargets();
            player.playerNetworkManager.isLockedOn.Value = false;
            return;
        }
        if (lockOn_Input && !player.playerNetworkManager.isLockedOn.Value)
        {
            lockOn_Input = false;

            PlayerCamera.instance.HandleLocatingLockOnTargets();

            if (PlayerCamera.instance.nearestLockOnTarget != null)
            {
                player.playerCombatManager.SetTarget(PlayerCamera.instance.nearestLockOnTarget);
                player.playerNetworkManager.isLockedOn.Value = true;
            }
        }
    }

    private void HandleLockOnSwitchTargetInput()
    {
        if (lockOn_Left_Input)
        {
            lockOn_Left_Input = false;

            if (player.playerNetworkManager.isLockedOn.Value)
            {
                PlayerCamera.instance.HandleLocatingLockOnTargets();

                if (PlayerCamera.instance.leftLockOnTarget != null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.instance.leftLockOnTarget);
                }
            }
        }

        if (lockOn_Right_Input)
        {
            lockOn_Right_Input = false;

            if (player.playerNetworkManager.isLockedOn.Value)
            {
                PlayerCamera.instance.HandleLocatingLockOnTargets();

                if (PlayerCamera.instance.rightLockOnTarget != null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.instance.rightLockOnTarget);
                }
            }
        }
    }

    private void HandlePlayerMovementInput()
    {
        if (PlayerUIManager.instance.menuWindowIsOpen)
            return;
        vertical_Input = movementInput.y;
        horizontal_Input = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(vertical_Input) + Mathf.Abs(horizontal_Input));

        if (moveAmount <= 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount > 0.5 && moveAmount <= 1)
        {
            moveAmount = 1;
        }

        if (player == null)
            return;

        if (moveAmount != 0)
        {
            player.playerNetworkManager.isMoving.Value = true;

        }
        else
        {
            player.playerNetworkManager.isMoving.Value = false;
        }

        if (!player.playerNetworkManager.isLockedOn.Value || player.playerNetworkManager.isSprinting.Value)
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
        }
        else
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontal_Input, vertical_Input, player.playerNetworkManager.isSprinting.Value);
        }
    }

    private void HandleCameraMovementInput()
    {
        cameraVertical_Input = camera_Input.y;
        cameraHorizontal_Input = camera_Input.x;
    }

    private void HandleDodgeInput()
    {
        if (dodge_Input)
        {
            if (PlayerUIManager.instance.menuWindowIsOpen)
                return;
            dodge_Input = false;
            player.playerLocomotionManager.AttemptToPerformDodge();
        }
    }

    private void HandleSprintInput()
    {
        if (sprint_Input)
        {
            if (PlayerUIManager.instance.menuWindowIsOpen)
                return;
            player.playerLocomotionManager.HandleSprinting();
        }
        else
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }
    }

    private void HandleJumpInput()
    {
        if (jump_Input)
        {
            jump_Input = false;

            if (PlayerUIManager.instance.menuWindowIsOpen)
                return;

            player.playerLocomotionManager.AttempToPerformJump();
        }
    }

    private void HandleRBInput()
    {
        if (two_Hand_Input)
            return;

        if (RB_Input)
        {
            RB_Input = false;
            if (PlayerUIManager.instance.menuWindowIsOpen)
                return;
            player.playerNetworkManager.SetCharacterActionHand(true);

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_RB_Action, player.playerInventoryManager.currentRightHandWeapon);
        }
    }

    private void HandleLBInput()
    {

        if (two_Hand_Input)
            return;

        if (LB_Input)
        {
            LB_Input = false;
            if (PlayerUIManager.instance.menuWindowIsOpen)
                return;
            player.playerNetworkManager.SetCharacterActionHand(false);

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentLeftHandWeapon.oh_LB_Action, player.playerInventoryManager.currentLeftHandWeapon);

        }
    }

    private void HandleRTInput()
    {

        if (RT_Input)
        {
            RT_Input = false;
            if (PlayerUIManager.instance.menuWindowIsOpen)
                return;
            player.playerNetworkManager.SetCharacterActionHand(true);
            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_RT_Action, player.playerInventoryManager.currentRightHandWeapon);
        }
    }

    private void HandleChargeRTInput()
    {

        if (player.isPerformingAction)
        {
            if (PlayerUIManager.instance.menuWindowIsOpen)
                return;
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                player.playerNetworkManager.isChargingAttack.Value = Hold_RT_Input;
            }
        }
    }

    private void HandleLTInput()
    {

        if (LT_Input)
        {
            LT_Input = false;

            if (PlayerUIManager.instance.menuWindowIsOpen)
                return;

            WeaponItem weaponPerformingAshOfWar = player.playerCombatManager.SelectWeaponToPerformAshOfWar();

            weaponPerformingAshOfWar.ashOfWarAction.AttemptToPerformAction(player);
        }
    }

    private void HandleSwitchRightWeaponInput()
    {
        if (switch_Right_Weapon_Input)
        {
            switch_Right_Weapon_Input = false;

            if (PlayerUIManager.instance.menuWindowIsOpen)
                return;

            if (player.isPerformingAction)
                return;

            if (player.playerCombatManager.isUsingItem)
                return;

            player.playerEquipmentManager.SwitchRightWeapon();
        }
    }

    private void HandleSwitchLeftWeaponInput()
    {
        if (switch_Left_Weapon_Input)
        {
            switch_Left_Weapon_Input = false;

            if (PlayerUIManager.instance.menuWindowIsOpen)
                return;

            if (player.isPerformingAction)
                return;

            if (player.playerCombatManager.isUsingItem)
                return;

            player.playerEquipmentManager.SwitchLeftWeapon();
        }
    }

    private void HandleSwitchQuickSlotItemInput()
    {
        if (switch_Quick_Slot_Item_Input)
        {
            switch_Quick_Slot_Item_Input = false;

            if (PlayerUIManager.instance.menuWindowIsOpen)
                return;

            if (player.isPerformingAction)
                return;

            if (player.playerCombatManager.isUsingItem)
                return;

            player.playerEquipmentManager.SwitchQuickSlotItem();
        }
    }



    private void QueInput(ref bool queInput)
    {
        que_RB_Input = false;
        que_RT_Input = false;

        if (player.isPerformingAction || player.playerNetworkManager.isJumping.Value)
        {
            queInput = true;
            que_Input_Timer = default_Que_Input_Timer;
            input_Que_Is_Active = true;
        }
    }

    private void ProcessQuedInput()
    {
        if (player.isDead.Value)
            return;
        if (que_RB_Input)
            RB_Input = true;
        if (que_RT_Input)
            RT_Input = true;
    }

    private void HandleQuedInputs()
    {
        if (input_Que_Is_Active)
        {
            if (que_Input_Timer > 0)
            {
                que_Input_Timer -= Time.deltaTime;
                ProcessQuedInput();
            }
        }
        else
        {
            que_RB_Input = false;
            que_RT_Input = false;
            input_Que_Is_Active = false;
            que_Input_Timer = 0;
        }
    }

    private void HandleInteractionInput()
    {
        if (interaction_Input)
        {
            interaction_Input = false;
            player.playerInteractionManager.Interact();
        }
    }

    private void HandleOpenCharacterMenuInput()
    {
        if (openCharacterMenuInput)
        {
            openCharacterMenuInput = false;

            PlayerUIManager.instance.playerUICharacterMenuManager.CloseCharacterMenu();
            PlayerUIManager.instance.CloseAllMenuWindows();
            PlayerUIManager.instance.playerUICharacterMenuManager.OpenCharacterMenu();
        }
    }

    private void HandleCloseMenuInput()
    {
        if (closeMenuInput)
        {
            closeMenuInput = false;

            if (PlayerUIManager.instance.menuWindowIsOpen)
            {
                PlayerUIManager.instance.playerUICharacterMenuManager.CloseCharacterMenu();
            }
        }
    }

}
