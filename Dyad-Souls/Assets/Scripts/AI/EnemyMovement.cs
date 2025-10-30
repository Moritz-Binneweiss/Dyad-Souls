using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private float timer;
    private float waitTimer;
    private bool isWaiting;

    [Header("Wandering Settings")]
    [SerializeField]
    private float wanderRadius = 20f;

    [SerializeField]
    private float wanderTimer = 5f;

    [SerializeField]
    private float waitTimeAfterArrival = 3f;

    [SerializeField]
    private float movementThreshold = 0.1f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        timer = wanderTimer;
        SetRandomDestination();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTimeAfterArrival)
            {
                isWaiting = false;
                SetRandomDestination();
                timer = 0;
            }
        }
        else if (ShouldSetNewDestination())
        {
            if (HasReachedDestination())
            {
                isWaiting = true;
                waitTimer = 0;
            }
            else
            {
                SetRandomDestination();
                timer = 0;
            }
        }

        UpdateAnimator();
    }

    bool ShouldSetNewDestination()
    {
        return timer >= wanderTimer || HasReachedDestination();
    }

    bool HasReachedDestination()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }

    void UpdateAnimator()
    {
        if (animator == null)
            return;

        bool isMoving = agent.velocity.magnitude > movementThreshold;
        animator.SetBool("isRunning", isMoving);
    }

    void SetRandomDestination()
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * wanderRadius;

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}
