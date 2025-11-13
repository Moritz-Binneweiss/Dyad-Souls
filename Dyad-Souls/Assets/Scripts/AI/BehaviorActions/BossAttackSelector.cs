using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Boss")]
[TaskDescription("Wählt die beste Attacke basierend auf Boss-Phase und Kontext (Elden Ring Style)")]
public class BossAttackSelector : Action
{
    [Header("Input Variables")]
    public SharedString chosenAttack;
    public SharedFloat playerDistance;
    public SharedBool isPlayerBehind;
    public SharedString lastAttack;
    
    [Header("Enemy Settings")]
    public SharedGameObject enemyGameObject;
    
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
            Debug.LogError("BossAttackSelector: EnemyManager nicht gefunden!");
            return TaskStatus.Failure;
        }
        
        // Sammle Kontext-Informationen
        float distance = playerDistance != null ? playerDistance.Value : 5f;
        bool behind = isPlayerBehind != null ? isPlayerBehind.Value : false;
        string last = lastAttack != null ? lastAttack.Value : "";
        
        // Lass den Enemy Manager die beste Attacke wählen
        string selectedAttack = enemyManager.GetBestAttackForSituation(distance, behind, last);
        
        // Setze die gewählte Attacke
        chosenAttack.Value = selectedAttack;
        
        // Update last attack für nächstes Mal
        if (lastAttack != null)
        {
            lastAttack.Value = selectedAttack;
        }
        
        Debug.Log($"BossAttackSelector: Chose {selectedAttack} (Distance: {distance:F1}, Behind: {behind}, Phase: {enemyManager.GetCurrentPhase()})");
        
        return TaskStatus.Success;
    }
}