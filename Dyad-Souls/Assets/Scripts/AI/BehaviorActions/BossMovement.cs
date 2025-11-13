using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

[TaskCategory("Boss")]
[TaskDescription("Boss bewegt sich intelligent zum näheren Spieler (Elden Ring Style)")]
public class BossMovement : Action
{
    [Header("Movement Settings")]
    public SharedFloat moveSpeed = 3.5f;
    public SharedFloat stoppingDistance = 4f;
    
    [Header("Player References")]
    public SharedTransform playerOne;
    public SharedTransform playerTwo;
    
    [Header("Debug")]
    public SharedBool debugMode = false;
    
    private NavMeshAgent agent;
    private Animator animator;
    private Transform targetPlayer;
    
    public override void OnAwake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    
    public override void OnStart()
    {
        if (agent == null)
        {
            Debug.LogError("BossMovement: NavMeshAgent nicht gefunden!");
            return;
        }
        
        // Finde näheren Spieler
        targetPlayer = GetClosestPlayer();
        
        if (targetPlayer != null)
        {
            // Stelle sicher, dass Agent aktiv ist
            agent.enabled = true;
            agent.isStopped = false;
            agent.speed = moveSpeed.Value;
            
            // Setze Ziel
            agent.SetDestination(targetPlayer.position);
            
            // Animation
            if (animator != null)
                animator.SetBool("isRunning", true);
            
            if (debugMode.Value)
                Debug.Log($"BossMovement: Moving to {targetPlayer.name}, Distance: {Vector3.Distance(transform.position, targetPlayer.position):F1}");
        }
    }
    
    public override TaskStatus OnUpdate()
    {
        if (agent == null || targetPlayer == null)
        {
            if (animator != null)
                animator.SetBool("isRunning", false);
            return TaskStatus.Failure;
        }
        
        // Update Ziel (falls Spieler sich bewegt hat)
        agent.SetDestination(targetPlayer.position);
        
        // Prüfe Entfernung
        float distance = Vector3.Distance(transform.position, targetPlayer.position);
        
        if (distance <= stoppingDistance.Value)
        {
            // Ziel erreicht - stoppe
            agent.isStopped = true;
            if (animator != null)
                animator.SetBool("isRunning", false);
            
            if (debugMode.Value)
                Debug.Log($"BossMovement: Reached player, Distance: {distance:F1}");
                
            return TaskStatus.Success;
        }
        
        // Stelle sicher, dass Animation läuft
        if (animator != null && agent.velocity.magnitude > 0.1f)
            animator.SetBool("isRunning", true);
        
        return TaskStatus.Running;
    }
    
    public override void OnEnd()
    {
        if (animator != null)
            animator.SetBool("isRunning", false);
            
        if (debugMode.Value)
            Debug.Log("BossMovement: Ended");
    }
    
    private Transform GetClosestPlayer()
    {
        // Fallback: Finde alle Spieler mit Tag
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        if (players.Length == 0)
        {
            Debug.LogWarning("BossMovement: Keine Spieler gefunden!");
            return null;
        }
        
        Transform closest = null;
        float closestDistance = float.MaxValue;
        
        foreach (GameObject player in players)
        {
            if (player != null)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = player.transform;
                }
            }
        }
        
        return closest;
    }
}