using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class PerformDodge : Action
{
    public SharedGameObject target;

    public string dodgeAnimationName = "Dodge";

    public SharedFloat dodgeDuration = 0.8f;

    public SharedFloat dodgeDistance = 3f;

    public enum DodgeDirection
    {
        Backward,
        Left,
        Right,
        Random,
    }

    public DodgeDirection dodgeDirection = DodgeDirection.Random;

    private float timer;
    private NavMeshAgent agent;
    private Vector3 dodgeTarget;

    public override void OnStart()
    {
        timer = 0f;
        agent = GetComponent<NavMeshAgent>();

        Animator animator = GetComponent<Animator>();
        if (animator != null)
            animator.Play(dodgeAnimationName);

        Vector3 directionFromPlayer = Vector3.zero;
        if (target.Value != null)
            directionFromPlayer = (transform.position - target.Value.transform.position).normalized;

        Vector3 dodgeDir = directionFromPlayer;

        DodgeDirection actualDirection = dodgeDirection;
        if (dodgeDirection == DodgeDirection.Random)
            actualDirection = (DodgeDirection)Random.Range(0, 3);

        switch (actualDirection)
        {
            case DodgeDirection.Backward:
                dodgeDir = directionFromPlayer; // Weg vom Spieler
                break;
            case DodgeDirection.Left:
                dodgeDir = Vector3.Cross(directionFromPlayer, Vector3.up);
                break;
            case DodgeDirection.Right:
                dodgeDir = Vector3.Cross(Vector3.up, directionFromPlayer);
                break;
        }

        // Setze Dodge-Ziel
        dodgeTarget = transform.position + dodgeDir * dodgeDistance.Value;

        // Bewege mit NavMesh
        if (agent != null)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(dodgeTarget, out hit, dodgeDistance.Value, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
    }

    public override TaskStatus OnUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= dodgeDuration.Value)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        if (agent != null && agent.isOnNavMesh)
        {
            agent.ResetPath();
        }
    }
}
