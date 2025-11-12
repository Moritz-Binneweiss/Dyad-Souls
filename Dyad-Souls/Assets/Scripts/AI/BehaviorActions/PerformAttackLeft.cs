using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskDescription("Führt einen normalen Angriff aus (20 Damage)")]
[TaskCategory("Combat")]
public class PerformAttackLeft : Action
{
    private EnemyCombatSystem combatSystem;
    private bool attackStarted;

    public override void OnAwake()
    {
        combatSystem = GetComponent<EnemyCombatSystem>();
    }

    public override void OnStart()
    {
        attackStarted = false;
    }

    public override TaskStatus OnUpdate()
    {
        if (combatSystem == null)
        {
            Debug.LogError("EnemyCombatSystem component not found!");
            return TaskStatus.Failure;
        }

        // Wenn noch kein Angriff läuft → starten
        if (!attackStarted)
        {
            combatSystem.PerformAttackLeft();  // startet Animation + Damage
            attackStarted = true;
        }

        // Solange das CombatSystem sagt "Ich greife noch an" → Running
        if (combatSystem.IsAttacking())
        {
            return TaskStatus.Running;
        }

        // Wenn Attack vorbei ist → Success
        return TaskStatus.Success;
    }
}
