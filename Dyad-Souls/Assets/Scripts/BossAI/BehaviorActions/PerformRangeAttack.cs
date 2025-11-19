using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/// <summary>
/// Fernkampf-Angriff
/// </summary>
[TaskDescription("Führt einen Range-Angriff aus. Kann aus größerer Distanz eingesetzt werden.")]
public class PerformRangeAttack : BossAttackBase
{
    [UnityEngine.Tooltip("Schaden des Angriffs")]
    public SharedFloat damage = 35f;

    [UnityEngine.Tooltip("Maximale Reichweite für Range Attack")]
    public SharedFloat maxRange = 10f;

    [UnityEngine.Tooltip("Minimale Reichweite (zu nah für Range Attack)")]
    public SharedFloat minRange = 5f;

    public PerformRangeAttack()
    {
        // Defaults für Range Attack
        animationTrigger = new SharedString { Value = "RangeAttack" };
        attackDuration = new SharedFloat { Value = 1.8f };
        cooldownTime = new SharedFloat { Value = 5f };
        attackChance = new SharedFloat { Value = 30f }; // 30% Chance
    }

    public override void OnStart()
    {
        // Prüfe ob Spieler in richtiger Distanz ist (nicht zu nah, nicht zu weit)
        if (target != null && target.Value != null)
        {
            float distance = Vector3.Distance(transform.position, target.Value.transform.position);
            
            if (distance < minRange.Value || distance > maxRange.Value)
            {
                // Spieler ist zu nah oder zu weit für Range Attack
                Debug.Log($"Boss: Range Attack - Spieler nicht in richtiger Distanz ({distance:F1}m)");
                return;
            }
        }

        base.OnStart();
    }

    protected override void OnAttackStart()
    {
        Debug.Log("Boss: Range Attack gestartet!");
        // Hier würdest du das Projektil spawnen
    }

    protected override void OnAttackUpdate(float elapsedTime)
    {
        // Spawn Projektil zu bestimmtem Zeitpunkt
        if (elapsedTime >= 0.8f && elapsedTime <= 0.9f)
        {
            // Spawn projectile hier
            // z.B. Instantiate(projectilePrefab, spawnPoint.position, ...);
        }
    }

    protected override void OnAttackComplete()
    {
        Debug.Log("Boss: Range Attack abgeschlossen!");
    }
}
