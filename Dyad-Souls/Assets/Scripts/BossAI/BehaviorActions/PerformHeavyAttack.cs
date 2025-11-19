using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/// <summary>
/// Langsamer schwerer Angriff - hoher Schaden
/// </summary>
[TaskDescription("Führt einen langsamen Heavy Attack aus. Hoher Schaden, lange Cooldown.")]
public class PerformHeavyAttack : BossAttackBase
{
    [UnityEngine.Tooltip("Schaden des Angriffs")]
    public SharedFloat damage = 50f;

    [UnityEngine.Tooltip("Reichweite des Angriffs")]
    public SharedFloat attackRange = 4f;

    public PerformHeavyAttack()
    {
        // Defaults für Heavy Attack
        animationTrigger = new SharedString { Value = "HeavyAttack" };
        attackDuration = new SharedFloat { Value = 2.5f };
        cooldownTime = new SharedFloat { Value = 4f };
        attackChance = new SharedFloat { Value = 40f }; // 40% Chance
    }

    protected override void OnAttackStart()
    {
        Debug.Log("Boss: Heavy Attack gestartet!");
    }

    protected override void OnAttackUpdate(float elapsedTime)
    {
        // Damage-Window später in der Animation (Heavy Attacks brauchen länger)
        if (elapsedTime >= 1.2f && elapsedTime <= 1.5f)
        {
            // Damage-Detection
        }
    }

    protected override void OnAttackComplete()
    {
        Debug.Log("Boss: Heavy Attack abgeschlossen!");
    }
}
