using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/// <summary>
/// Combo-Angriff mit beiden Händen
/// </summary>
[TaskDescription("Führt einen Combo-Angriff mit links und rechts aus. Mittlerer Schaden, mittlere Cooldown.")]
public class PerformComboAttack : BossAttackBase
{
    [UnityEngine.Tooltip("Schaden des Angriffs")]
    public SharedFloat damage = 40f;

    [UnityEngine.Tooltip("Reichweite des Angriffs")]
    public SharedFloat attackRange = 3.5f;

    public PerformComboAttack()
    {
        // Defaults für Combo Attack
        animationTrigger = new SharedString { Value = "AttackLeftAndRight" };
        attackDuration = new SharedFloat { Value = 2.0f };
        cooldownTime = new SharedFloat { Value = 3f };
        attackChance = new SharedFloat { Value = 50f }; // 50% Chance
    }

    protected override void OnAttackStart()
    {
        Debug.Log("Boss: Combo Attack (Left+Right) gestartet!");
    }

    protected override void OnAttackUpdate(float elapsedTime)
    {
        // Zwei Damage-Windows für beide Schläge
        // Erster Schlag (links)
        if (elapsedTime >= 0.4f && elapsedTime <= 0.7f)
        {
            // First hit damage
        }
        // Zweiter Schlag (rechts)
        else if (elapsedTime >= 1.2f && elapsedTime <= 1.5f)
        {
            // Second hit damage
        }
    }

    protected override void OnAttackComplete()
    {
        Debug.Log("Boss: Combo Attack abgeschlossen!");
    }
}
