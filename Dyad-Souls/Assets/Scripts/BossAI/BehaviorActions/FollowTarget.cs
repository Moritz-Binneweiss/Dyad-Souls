using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Folgt dem aktuellen Ziel permanent und hält eine bestimmte Distanz
/// </summary>
public class FollowTarget : Action
{
    public SharedGameObject target;

    public SharedFloat stoppingDistance = 3f;

    public SharedFloat updateInterval = 0.2f;

    private NavMeshAgent agent;
    private float updateTimer;

    public override void OnStart()
    {
        agent = GetComponent<NavMeshAgent>();
        updateTimer = 0f;
    }

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null || agent == null)
        {
            return TaskStatus.Running; // Bleibt aktiv, auch wenn kein Ziel
        }

        updateTimer += Time.deltaTime;

        // Update nur alle X Sekunden (Performance)
        if (updateTimer >= updateInterval.Value)
        {
            updateTimer = 0f;

            float distance = Vector3.Distance(transform.position, target.Value.transform.position);

            // Nur bewegen wenn zu weit weg
            if (distance > stoppingDistance.Value)
            {
                agent.SetDestination(target.Value.transform.position);
            }
            else
            {
                // Stoppen wenn nah genug
                agent.ResetPath();
            }
        }

        // Läuft immer weiter (nie Success/Failure)
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        if (agent != null)
        {
            agent.ResetPath();
        }
    }
}
