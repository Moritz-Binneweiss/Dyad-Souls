using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskDescription("Prüft ob ein Spieler in Angriffsreichweite ist und gibt den nächsten zurück")]
[TaskCategory("Combat")]
public class IsWithinAttackRange : Conditional
{
    public SharedGameObject playerOne;
    public SharedGameObject playerTwo;
    public SharedGameObject returnedPlayer;
    public AttackType attackType = AttackType.Normal;

    private EnemyCombatSystem combatSystem;

    public enum AttackType
    {
        Normal,
        Heavy,
        Range,
    }

    public override void OnAwake()
    {
        combatSystem = GetComponent<EnemyCombatSystem>();
    }

    public override TaskStatus OnUpdate()
    {
        if (combatSystem == null)
        {
            return TaskStatus.Failure;
        }

        returnedPlayer.Value = null;

        // Check if we have at least one player assigned
        if (playerOne.Value == null && playerTwo.Value == null)
        {
            return TaskStatus.Failure;
        }

        GameObject closestPlayer = null;
        float closestDistance = float.MaxValue;
        float requiredRange = GetRequiredRange();

        // Check Player One
        if (playerOne.Value != null)
        {
            float distance = Vector3.Distance(
                transform.position,
                playerOne.Value.transform.position
            );
            if (distance <= requiredRange && distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = playerOne.Value;
            }
        }

        // Check Player Two
        if (playerTwo.Value != null)
        {
            float distance = Vector3.Distance(
                transform.position,
                playerTwo.Value.transform.position
            );
            if (distance <= requiredRange && distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = playerTwo.Value;
            }
        }

        if (closestPlayer != null)
        {
            returnedPlayer.Value = closestPlayer;
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }

    private float GetRequiredRange()
    {
        switch (attackType)
        {
            case AttackType.Normal:
                return combatSystem.GetAttackRange();
            case AttackType.Heavy:
                return combatSystem.GetHeavyAttackRange();
            case AttackType.Range:
                return combatSystem.GetRangeAttackRange();
            default:
                return combatSystem.GetAttackRange();
        }
    }
}
