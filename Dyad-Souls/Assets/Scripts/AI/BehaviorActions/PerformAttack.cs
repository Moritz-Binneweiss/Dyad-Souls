using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskDescription("Führt einen normalen Angriff aus (20 Damage)")]
[TaskCategory("Combat")]
public class PerformAttack : Action
{
    private EnemyCombatSystem combatSystem;

    public override void OnAwake()
    {
        combatSystem = GetComponent<EnemyCombatSystem>();
    }

    public override TaskStatus OnUpdate()
    {
        if (combatSystem == null)
        {
            Debug.LogError("EnemyCombatSystem component not found!");
            return TaskStatus.Failure;
        }

        if (combatSystem.IsAttacking())
        {
            return TaskStatus.Running;
        }

        combatSystem.PerformAttack();
        return TaskStatus.Success;
    }
}
