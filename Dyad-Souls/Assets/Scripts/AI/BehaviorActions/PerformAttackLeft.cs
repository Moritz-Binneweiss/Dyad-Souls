using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskDescription("Führt einen normalen Angriff aus (20 Damage) - Boss Version")]
[TaskCategory("Boss")]
public class PerformAttackLeft : Action
{
    private EnemyCombatSystem combatSystem;
    private EnemyManager enemyManager;
    private bool attackStarted;

    public override void OnAwake()
    {
        combatSystem = GetComponent<EnemyCombatSystem>();
        enemyManager = GetComponent<EnemyManager>();
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
            // Drehe Boss zum näheren Spieler
            RotateToClosestPlayer();
            
            combatSystem.PerformAttackLeft();  // startet Animation + Damage
            attackStarted = true;
            
            // Starte Cooldown im Enemy Manager
            if (enemyManager != null)
            {
                enemyManager.StartAttackCooldown("Left");
            }
        }

        // Solange das CombatSystem sagt "Ich greife noch an" → Running
        if (combatSystem.IsAttacking())
        {
            return TaskStatus.Running;
        }

        // Wenn Attack vorbei ist → Success
        return TaskStatus.Success;
    }
    
    private void RotateToClosestPlayer()
    {
        // Finde beide Spieler
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
        
        GameObject closestPlayer = null;
        float closestDistance = float.MaxValue;
        
        // Finde näheren Spieler
        foreach (GameObject player in allPlayers)
        {
            if (player != null)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayer = player;
                }
            }
        }
        
        // Drehe zum näheren Spieler
        if (closestPlayer != null)
        {
            Vector3 direction = (closestPlayer.transform.position - transform.position).normalized;
            direction.y = 0; // Nur horizontal drehen
            
            if (direction.magnitude > 0.1f)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}
