using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskDescription("Prüft ob Spieler in Reichweite für einen bestimmten Angriff ist")]
[TaskCategory("Combat")]
public class IsWithinSpecificAttackRange : Conditional
{
    public SharedGameObject playerOne;
    public SharedGameObject playerTwo;
    public SharedGameObject returnedPlayer;
    public string specificAttackType = "Right"; // Right, Left, Heavy, Range

    private EnemyCombatSystem combatSystem;

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

        if (playerOne.Value == null && playerTwo.Value == null)
        {
            return TaskStatus.Failure;
        }

        GameObject closestPlayer = null;
        float closestDistance = float.MaxValue;
        float requiredRange = GetRequiredRangeForAttack(specificAttackType);

        // Check Player One
        if (playerOne.Value != null)
        {
            float distance = Vector3.Distance(transform.position, playerOne.Value.transform.position);
            if (distance <= requiredRange && distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = playerOne.Value;
            }
        }

        // Check Player Two
        if (playerTwo.Value != null)
        {
            float distance = Vector3.Distance(transform.position, playerTwo.Value.transform.position);
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

    private float GetRequiredRangeForAttack(string attackType)
    {
        switch (attackType.ToLower())
        {
            case "heavy":
                return combatSystem.GetHeavyAttackRange();
            case "range":
                return combatSystem.GetRangeAttackRange();
            case "right":
            case "left":
            case "leftright":
            default:
                return combatSystem.GetAttackRange();
        }
    }
}