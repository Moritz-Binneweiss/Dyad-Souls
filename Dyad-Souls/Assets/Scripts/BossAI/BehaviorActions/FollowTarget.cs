using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

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
            return TaskStatus.Running;

        updateTimer += Time.deltaTime;

        if (updateTimer >= updateInterval.Value)
        {
            updateTimer = 0f;

            float distance = Vector3.Distance(transform.position, target.Value.transform.position);

            if (distance > stoppingDistance.Value)
                agent.SetDestination(target.Value.transform.position);
            else
                agent.ResetPath();
        }

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
