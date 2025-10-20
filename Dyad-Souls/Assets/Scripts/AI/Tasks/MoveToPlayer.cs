using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class MoveToPlayer : Action
{
    public SharedTransform player;
    public SharedFloat stoppingDistance = 2f;

    private NavMeshAgent agent;
    private Animator animator;

    public override void OnStart()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent != null && player.Value != null)
        {
            agent.isStopped = false;
            agent.SetDestination(player.Value.position);

            // Setze isRunning auf true wenn der Agent zu laufen beginnt
            if (animator != null)
                animator.SetBool("isRunning", true);
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (player.Value == null || agent == null)
        {
            // Stoppe Animation wenn Fehler auftritt
            if (animator != null)
                animator.SetBool("isRunning", false);
            return TaskStatus.Failure;
        }

        // Lauf weiter zum Ziel
        agent.SetDestination(player.Value.position);

        // Prüfe Entfernung
        float distance = Vector3.Distance(transform.position, player.Value.position);
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

    public override void OnEnd()
    {
        if (agent != null)
            agent.isStopped = true;

        // Stoppe Lauf-Animation wenn die Task beendet wird
        if (animator != null)
            animator.SetBool("isRunning", false);
    }
}
