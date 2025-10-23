using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class MoveToPlayer : Action
{
    public SharedTransform player;

    public SharedTransform playerTwo;
    public SharedFloat stoppingDistance = 2f;

    private NavMeshAgent agent;
    private Animator animator;

    public override void OnStart()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        Transform targetPlayer = GetClosestPlayer();
        if (agent != null && targetPlayer != null)
        {
            agent.isStopped = false;
            agent.SetDestination(targetPlayer.position);

            // Setze isRunning auf true wenn der Agent zu laufen beginnt
            if (animator != null)
                animator.SetBool("isRunning", true);
        }
    }

    public override TaskStatus OnUpdate()
    {
        Transform targetPlayer = GetClosestPlayer();
        if (targetPlayer == null || agent == null)
        {
            // Stoppe Animation wenn Fehler auftritt
            if (animator != null)
                animator.SetBool("isRunning", false);
            return TaskStatus.Failure;
        }

        // Lauf weiter zum nächstgelegenen Spieler
        agent.SetDestination(targetPlayer.position);

        // Prüfe Entfernung
        float distance = Vector3.Distance(transform.position, targetPlayer.position);
        if (distance <= stoppingDistance.Value)
        {
            agent.isStopped = true;
            // Stoppe Lauf-Animation wenn Ziel erreicht
            if (animator != null)
                animator.SetBool("isRunning", false);
            return TaskStatus.Success; // Ziel erreicht
        }

        // Stelle sicher, dass die Lauf-Animation läuft
        if (animator != null)
            animator.SetBool("isRunning", true);

        return TaskStatus.Running;
    }

    private Transform GetClosestPlayer()
    {
        // Prüfe welche Spieler verfügbar sind
        if (player.Value == null && playerTwo.Value == null)
            return null;
        
        if (player.Value == null)
            return playerTwo.Value;
        
        if (playerTwo.Value == null)
            return player.Value;

        // Beide Spieler sind verfügbar - wähle den nächstgelegenen
        float distanceToPlayer = Vector3.Distance(transform.position, player.Value.position);
        float distanceToPlayerTwo = Vector3.Distance(transform.position, playerTwo.Value.position);

        return distanceToPlayer <= distanceToPlayerTwo ? player.Value : playerTwo.Value;
    }

    public override void OnEnd()
    {
        if (agent != null)
            agent.isStopped = true;

        // Stoppe Lauf-Animation wenn die Task beendet wird
        if (animator != null)
            animator.SetBool("isRunning", false);
    }
}
