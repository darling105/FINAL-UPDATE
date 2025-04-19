using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/Actions/Attack")]
public class AICharacterAttackAction : ScriptableObject
{
    [Header("Attack")]
    [SerializeField] private string attackAnimation;
    [SerializeField] bool isParryable = true;
    

    [Header("Combo Action")]
    public AICharacterAttackAction comboAction;

    [Header("Action Values")]
    public int attackWeight = 50;
    [SerializeField] AttackType attackType;

    public float actionRecoveryTime = 1.5f;
    public float minimumAttackAngle = -35;
    public float maximumAttackAngle = 35;
    public float minimumAttackDistance = 0;
    public float maximumAttackDistance = 2;

    public void AttempToPerformAction(AICharacterManager aiCharacter)
    {
        aiCharacter.characterAnimatorManager.PlayTargetActionAnimation(attackAnimation, true);
        aiCharacter.aiCharacterNetworkManager.isParryable.Value = isParryable;
    }
}
