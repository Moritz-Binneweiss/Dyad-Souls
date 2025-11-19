using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/// <summary>
/// Schneller Angriff mit der linken Hand
/// </summary>
[TaskDescription("Führt einen schnellen Angriff mit der linken Hand aus. Wenig Schaden, kurze Cooldown.")]
public class PerformLeftAttack : BossAttackBase
{
    [UnityEngine.Tooltip("Schaden des Angriffs")]
    public SharedFloat damage = 20f;

    [UnityEngine.Tooltip("Reichweite des Angriffs")]
    public SharedFloat attackRange = 3f;

    public PerformLeftAttack()
    {
        // Defaults für Left Hand Attack
        animationTrigger = new SharedString { Value = "AttackLeft" };
        attackDuration = new SharedFloat { Value = 1.0f };
        cooldownTime = new SharedFloat { Value = 1.5f };
        attackChance = new SharedFloat { Value = 100f };
    }

    protected override void OnAttackStart()
    {
        Debug.Log("Boss: Left Hand Attack gestartet!");
    }

    protected override void OnAttackUpdate(float elapsedTime)
    {
        // Damage-Window
        if (elapsedTime >= 0.3f && elapsedTime <= 0.6f)
        {
            // Collision-Detection hier
        }
    }

    protected override void OnAttackComplete()
    {
        Debug.Log("Boss: Left Hand Attack abgeschlossen!");
    }
}
