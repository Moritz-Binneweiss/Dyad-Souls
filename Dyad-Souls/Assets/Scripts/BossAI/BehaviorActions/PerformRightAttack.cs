using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/// <summary>
/// Schneller Angriff mit der rechten Hand
/// </summary>
[TaskDescription("Führt einen schnellen Angriff mit der rechten Hand aus. Wenig Schaden, kurze Cooldown.")]
public class PerformRightAttack : BossAttackBase
{
    [UnityEngine.Tooltip("Schaden des Angriffs")]
    public SharedFloat damage = 20f;

    [UnityEngine.Tooltip("Reichweite des Angriffs")]
    public SharedFloat attackRange = 3f;

    public PerformRightAttack()
    {
        // Defaults für Right Hand Attack
        animationTrigger = new SharedString { Value = "AttackRight" };
        attackDuration = new SharedFloat { Value = 1.0f };
        cooldownTime = new SharedFloat { Value = 1.5f };
        attackChance = new SharedFloat { Value = 100f };
    }

    protected override void OnAttackStart()
    {
        Debug.Log("Boss: Right Hand Attack gestartet!");
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
        Debug.Log("Boss: Right Hand Attack abgeschlossen!");
    }
}
