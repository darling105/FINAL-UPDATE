using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Unity.Netcode;

public class AICharacterCombatManager : CharacterCombatManager
{
    protected AICharacterManager aiCharacter;
    [Header("Action Recovery")]
    public float actionRecoveryTimer = 0;

    [Header("Pivot")]
    public bool enabledPivot = true;

    [Header("Target Information")]
    public float distanceFromTarget;
    public float viewAbleAngle;
    public Vector3 targetsDirection;


    [Header("Detection")]
    [SerializeField] float detectionRadius = 15;
    public float minimumFOV = -35;
    public float maximumFOV = 35;

    [Header("Attack Rotation Speed")]
    public float attackRotationSpeed = 25;

    [Header("Stance")]
    public float maxStance = 100;
    public float currentStance;
    [SerializeField] float stanceRecoveryPersecond = 15;
    [SerializeField] bool ignoreStanceBreak = false;

    [Header("Stance Timer")]
    [SerializeField] float stanceRegenerationTimer = 0;
    private float stanceTickTimer = 0;
    [SerializeField] float defaultTimeUntilStanceRegenerationBegins = 15;

    protected override void Awake()
    {
        base.Awake();

        aiCharacter = GetComponent<AICharacterManager>();
        lockOnTransform = GetComponentInChildren<LockOnTransform>().transform;
    }

    private void FixedUpdate()
    {
        HandleStanceBreak();
    }

    public void AwardShadesOnDeath(PlayerManager player)
    {
        if (player.characterGroup == CharacterGroup.Team02)
            return;

        // if(NetworkManager.Singleton.IsHost)
        // {

        // }

        player.playerStatsManager.AddShades(aiCharacter.characterStatsManager.shadesDropOnDeath);

    }

    private void HandleStanceBreak()
    {
        if (!aiCharacter.IsOwner)
            return;

        if (aiCharacter.isDead.Value)
            return;

        if (stanceRegenerationTimer > 0)
        {
            stanceRegenerationTimer -= Time.deltaTime;
        }
        else
        {
            stanceRegenerationTimer = 0;

            if (currentStance < maxStance)
            {
                stanceTickTimer += Time.deltaTime;
                if (stanceTickTimer >= 1)
                {
                    stanceTickTimer = 0;
                    currentStance += stanceRecoveryPersecond;
                }
            }
            else
            {
                currentStance = maxStance;
            }
        }

        if (currentStance <= 0)
        {
            DamageIntensity previousDamageIntensity = WorldUtilityManager.instance.GetDamageIntensityBasedOnPoiseDamage(previousPoiseDamageTaken);

            if (previousDamageIntensity == DamageIntensity.Heavy)
            {
                currentStance = 1;
                return;
            }

            currentStance = maxStance;

            if (ignoreStanceBreak)
                return;

            aiCharacter.characterAnimatorManager.PlayTargetActionAnimationInstantly("Stance_Break_01", true);
        }
    }

    public void DamageStance(int stanceDamage)
    {
        stanceRegenerationTimer = defaultTimeUntilStanceRegenerationBegins;

        currentStance -= stanceDamage;


    }

    public void FindATargetViaLineOfSight(AICharacterManager aiCharacter)
    {
        if (currentTarget != null)
            return;

        Collider[] collider = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, WorldUtilityManager.instance.GetCharacterLayers());

        for (int i = 0; i < collider.Length; i++)
        {
            CharacterManager targetCharacter = collider[i].GetComponent<CharacterManager>();


            if (targetCharacter == null)
                continue;

            if (targetCharacter == aiCharacter)
                continue;

            if (targetCharacter.isDead.Value)
                continue;

            if (WorldUtilityManager.instance.CanIDamageThisTarget(aiCharacter.characterGroup, targetCharacter.characterGroup))
            {
                Vector3 targetsDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                float angleOfPotentialTarget = Vector3.Angle(targetsDirection, aiCharacter.transform.forward);

                if (angleOfPotentialTarget > minimumFOV && angleOfPotentialTarget < maximumFOV)
                {
                    if (Physics.Linecast(aiCharacter.characterCombatManager.lockOnTransform.position,
                    targetCharacter.characterCombatManager.lockOnTransform.position,
                    WorldUtilityManager.instance.GetEnviroLayers()))
                    {
                        Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position, Color.red);
                    }
                    else
                    {
                        targetsDirection = targetCharacter.transform.position - transform.position;
                        viewAbleAngle = WorldUtilityManager.instance.GetAngleOfTarget(transform, targetsDirection);
                        aiCharacter.characterCombatManager.SetTarget(targetCharacter);

                        if (enabledPivot)
                            PivotTowardsTarget(aiCharacter);
                    }
                }
            }
        }
    }

    public virtual void PivotTowardsTarget(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction)
            return;

        if (viewAbleAngle >= 20 && viewAbleAngle <= 60)
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Right_Turn", true);
        else if (viewAbleAngle <= -20 && viewAbleAngle >= -60)
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Left_Turn", true);
        else if (viewAbleAngle >= 61 && viewAbleAngle <= 110)
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Right_Turn", true);
        else if (viewAbleAngle <= -61 && viewAbleAngle >= -110)
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Left_Turn", true);
        else if (viewAbleAngle >= 110 && viewAbleAngle <= 145)
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Right_Turn", true);
        else if (viewAbleAngle <= -110 && viewAbleAngle >= -145)
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Left_Turn", true);
        else if (viewAbleAngle >= 146 && viewAbleAngle <= 180)
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Right_Turn", true);
        else if (viewAbleAngle <= -146 && viewAbleAngle >= -180)
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Left_Turn", true);
    }

    public void RotateTowardsAgent(AICharacterManager aiCharacter)
    {
        if (aiCharacter.aiCharacterNetworkManager.isMoving.Value)
        {
            aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
        }
    }

    public void RotateTowardsTargetWhilstAttacking(AICharacterManager aiCharacter)
    {
        if (currentTarget != null)
            return;

        if (!aiCharacter.characterLocomotionManager.canRotate)
            return;

        if (aiCharacter.isPerformingAction)
            return;

        Vector3 targetDirection = currentTarget.transform.position - aiCharacter.transform.position;
        targetDirection.y = 0;
        targetDirection.Normalize();

        if (targetDirection == Vector3.zero)
            targetDirection = aiCharacter.transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, attackRotationSpeed * Time.deltaTime);
    }

    public void HandleActionRecovery(AICharacterManager aiCharacter)
    {
        if (actionRecoveryTimer > 0)
        {
            if (!aiCharacter.isPerformingAction)
            {
                actionRecoveryTimer -= Time.deltaTime;
            }
        }
    }

}
