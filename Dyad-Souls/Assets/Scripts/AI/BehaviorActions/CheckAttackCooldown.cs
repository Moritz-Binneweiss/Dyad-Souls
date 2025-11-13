using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Boss")]
[TaskDescription("Überprüft ob der Boss eine Attacke ausführen kann (Cooldown System)")]
public class CheckAttackCooldown : Conditional
{
    
    public SharedGameObject enemyGameObject;
    
    
    public SharedFloat minimumCooldown = 1f;
    
    private EnemyManager enemyManager;
    
    public override void OnAwake()
    {
        // Finde Enemy Manager
        if (enemyGameObject.Value != null)
        {
            enemyManager = enemyGameObject.Value.GetComponent<EnemyManager>();
        }
        else
        {
            enemyManager = GetComponent<EnemyManager>();
        }
    }
    
    public override TaskStatus OnUpdate()
    {
        if (enemyManager == null)
        {
            Debug.LogError("CheckAttackCooldown: EnemyManager nicht gefunden!");
            return TaskStatus.Failure;
        }
        
        // Prüfe ob Boss angreifen kann
        bool canAttack = enemyManager.CanAttack();
        
        if (canAttack)
        {
            return TaskStatus.Success;
        }
        else
        {
            // Optional: Debug Info über verbleibende Cooldown-Zeit
            float remainingCooldown = enemyManager.GetRemainingCooldown();
            //Debug.Log($"Attack on cooldown, remaining: {remainingCooldown:F1}s");
            return TaskStatus.Failure;
        }
    }
}