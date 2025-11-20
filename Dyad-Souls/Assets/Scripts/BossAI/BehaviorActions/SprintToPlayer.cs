using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Sprintet aggressiv zum Spieler (Gap Close für Far Range)
/// </summary>
public class SprintToPlayer : Action
{
    public SharedGameObject target;

    public SharedFloat sprintSpeedMultiplier = 1.8f;

    public SharedFloat stoppingDistance = 3f;

    private NavMeshAgent agent;
    private float originalSpeed;

    public override void OnStart()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            originalSpeed = agent.speed;
            agent.speed = originalSpeed * sprintSpeedMultiplier.Value;
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null || agent == null)
        {
            return TaskStatus.Failure;
        }

        float distance = Vector3.Distance(transform.position, target.Value.transform.position);

        // Wenn nah genug, Sprint erfolgreich beendet
        if (distance <= stoppingDistance.Value)
        {
            agent.speed = originalSpeed;
            return TaskStatus.Success;
        }

        // Bewege zum Spieler
        agent.SetDestination(target.Value.transform.position);

        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        if (agent != null)
        {
            agent.speed = originalSpeed;
        }
    }
}
