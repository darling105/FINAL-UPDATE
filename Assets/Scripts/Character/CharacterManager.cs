using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterManager : NetworkBehaviour
{
    

    [Header("Status")]
    public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public CharacterStatsManager characterStatsManager;
    [HideInInspector] public CharacterEffectsManager characterEffectsManager;
    [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
    [HideInInspector] public CharacterCombatManager characterCombatManager;
    [HideInInspector] public CharacterSoundFXManager characterSoundFXManager;
    [HideInInspector] public CharacterLocomotionManager characterLocomotionManager;
    [HideInInspector] public CharacterNetworkManager characterNetworkManager;
    [HideInInspector] public CharacterUIManager characterUIManager;
    [HideInInspector] public Animator animator;

    [Header("Character Group")]
    public CharacterGroup characterGroup;

    [Header("Flags")]
    public bool isPerformingAction = false;

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        characterStatsManager = GetComponent<CharacterStatsManager>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        characterCombatManager = GetComponent<CharacterCombatManager>();
        characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
        characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
        characterNetworkManager = GetComponent<CharacterNetworkManager>();
        characterUIManager = GetComponent<CharacterUIManager>();
    }

    protected virtual void Start()
    {
        IgnoreMyOwnColliders();
    }

    protected virtual void Update()
    {
        animator.SetBool("isGrounded", characterLocomotionManager.isGrounded);

        if (IsOwner)
        {
            characterNetworkManager.networkPosition.Value = transform.position;
            characterNetworkManager.networkRotation.Value = transform.rotation;
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position,
             characterNetworkManager.networkPosition.Value,
             ref characterNetworkManager.networkPositionVelocity, characterNetworkManager.networkPositionSmoothTime);

            transform.rotation = Quaternion.Slerp(transform.rotation,
            characterNetworkManager.networkRotation.Value,
             characterNetworkManager.networkRotationSmoothTime);
        }
    }

    protected virtual void FixedUpdate()
    {

    }

    protected virtual void LateUpdate()
    {

    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        animator.SetBool("isMoving", characterNetworkManager.isMoving.Value);
        characterNetworkManager.OnIsActiveChanged(false, characterNetworkManager.isActive.Value);

        isDead.OnValueChanged += characterNetworkManager.OnIsDeadChanged;
        characterNetworkManager.isMoving.OnValueChanged += characterNetworkManager.OnIsMovingChanged;
        characterNetworkManager.isActive.OnValueChanged += characterNetworkManager.OnIsActiveChanged;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        isDead.OnValueChanged -= characterNetworkManager.OnIsDeadChanged;
        characterNetworkManager.isMoving.OnValueChanged -= characterNetworkManager.OnIsMovingChanged;
        characterNetworkManager.isActive.OnValueChanged -= characterNetworkManager.OnIsActiveChanged;
    }

    public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {
        if (IsOwner)
        {
            characterNetworkManager.currentHealth.Value = 0;
            isDead.Value = true;

            if (!manuallySelectDeathAnimation && !characterNetworkManager.isBeingCriticallyDamaged.Value)
            {
                characterAnimatorManager.PlayTargetActionAnimation("Death_01", true);
            }
        }


        yield return new WaitForSeconds(5);
    }

    protected virtual void IgnoreMyOwnColliders()
    {
        Collider characterControllerCollider = GetComponent<Collider>();
        Collider[] damageableCharacterColliders = GetComponentsInChildren<Collider>();

        List<Collider> ignoreColliders = new List<Collider>();

        foreach (var collider in damageableCharacterColliders)
        {
            ignoreColliders.Add(collider);
        }

        ignoreColliders.Add(characterControllerCollider);

        foreach (var collider in ignoreColliders)
        {
            foreach (var otherCollider in ignoreColliders)
            {
                Physics.IgnoreCollision(collider, otherCollider, true);
            }
        }
    }

    public void OnIsBlockingChanged(bool isBlocking)
    {
        animator.SetBool("isBlocking", isBlocking);
    }

}
