using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "A.I/States/Combat Stance")]
public class CombatStanceState : AIState
{
    [Header("Attacks")]
    public List<AICharacterAttackAction> aiCharacterAttacks;
    [SerializeField] List<AICharacterAttackAction> potentialAttacks;
    [SerializeField] AICharacterAttackAction choosenAttack;
    [SerializeField] AICharacterAttackAction previousAttack;
    protected bool hasAttack = false;

    [Header("Combo")]
    [SerializeField] protected bool canPerformCombo = false;
    [SerializeField] protected int chanceToPerformCombo = 25;
    protected bool hasRolledForComboChance = false;

    [Header("Pivot")]
    [SerializeField] public bool enablePivot;

    [Header("Engagement Distance")]
    [SerializeField] public float maximumEngagementDistance = 5;

    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction)
            return this;

        if (!aiCharacter.navMeshAgent.enabled)
            aiCharacter.navMeshAgent.enabled = true;

        if (enablePivot)
        {
            if (!aiCharacter.aiCharacterNetworkManager.isMoving.Value)
            {
                if (aiCharacter.aiCharacterCombatManager.viewAbleAngle < -30 || aiCharacter.aiCharacterCombatManager.viewAbleAngle > 30)
                    aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
            }
        }

        aiCharacter.aiCharacterCombatManager.RotateTowardsAgent(aiCharacter);

        if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
            return SwitchState(aiCharacter, aiCharacter.idle);

        if (!hasAttack)
        {
            GetNewAttack(aiCharacter);
        }
        else
        {
            aiCharacter.attack.currentAttack = choosenAttack;

            return SwitchState(aiCharacter, aiCharacter.attack);
        }

        if (aiCharacter.aiCharacterCombatManager.distanceFromTarget > maximumEngagementDistance)
            return SwitchState(aiCharacter, aiCharacter.pursueTarget);

        NavMeshPath path = new NavMeshPath();
        aiCharacter.navMeshAgent.CalculatePath(aiCharacter.characterCombatManager.currentTarget.transform.position, path);
        aiCharacter.navMeshAgent.SetPath(path);

        return this;

    }

    protected virtual void GetNewAttack(AICharacterManager aiCharacter)
    {
        potentialAttacks = new List<AICharacterAttackAction>();

        foreach (var potentialAttack in aiCharacterAttacks)
        {
            if (potentialAttack.minimumAttackDistance > aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                continue;
            if (potentialAttack.maximumAttackDistance < aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                continue;
            if (potentialAttack.minimumAttackAngle > aiCharacter.aiCharacterCombatManager.viewAbleAngle)
                continue;
            if (potentialAttack.maximumAttackAngle < aiCharacter.aiCharacterCombatManager.viewAbleAngle)
                continue;

            potentialAttacks.Add(potentialAttack);
        }

        if (potentialAttacks.Count <= 0)
            return;

        var totalWeight = 0;

        foreach (var attack in potentialAttacks)
        {
            totalWeight += attack.attackWeight;
        }

        var randomWeightValue = Random.Range(1, totalWeight + 1);
        var processedWeight = 0;

        foreach (var attack in potentialAttacks)
        {
            processedWeight += attack.attackWeight;

            if (randomWeightValue <= processedWeight)
            {
                choosenAttack = attack;
                previousAttack = choosenAttack;
                hasAttack = true;
                return;
            }
        }
    }

    protected virtual bool RollForOutComeChance(int outcomeChance)
    {
        bool outComeWillBePerformed = false;

        int randomPercentage = Random.Range(0, 100);

        if (randomPercentage < outcomeChance)
        {
            outComeWillBePerformed = true;
        }

        return outComeWillBePerformed;
    }

    protected override void ResetStateFlags(AICharacterManager aiCharacter)
    {
        base.ResetStateFlags(aiCharacter);

        hasAttack = false;
        hasRolledForComboChance = false;
    }

}
