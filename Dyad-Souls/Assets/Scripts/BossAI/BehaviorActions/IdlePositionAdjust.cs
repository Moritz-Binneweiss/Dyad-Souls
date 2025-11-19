using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

/// <summary>
/// Kleine Position-Anpassung / Idle-Movement um den Boss lebendiger wirken zu lassen
/// </summary>
public class IdlePositionAdjust : Action
{
    [Tooltip("Dauer der Idle-Anpassung")]
    public SharedFloat idleDuration = 2f;

    [Tooltip("Maximale Bewegungsreichweite")]
    public SharedFloat moveRadius = 2f;

    private float timer;
    private NavMeshAgent agent;

    public override void OnStart()
    {
        timer = 0f;
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            // Zufällige Position in der Nähe
            Vector3 randomDirection = Random.insideUnitSphere * moveRadius.Value;
            randomDirection += transform.position;
            randomDirection.y = transform.position.y; // Gleiche Höhe beibehalten

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, moveRadius.Value, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
    }

    public override TaskStatus OnUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= idleDuration.Value)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
