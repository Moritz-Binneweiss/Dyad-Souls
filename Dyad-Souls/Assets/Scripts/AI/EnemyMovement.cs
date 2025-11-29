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
        if (IsPlayerNearby())
            return;

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

    bool ShouldSetNewDestination() => timer >= wanderTimer || HasReachedDestination();

    bool HasReachedDestination() =>
        !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;

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

    private bool IsPlayerNearby()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            if (player != null)
            {
                PlayerManager playerManager = player.GetComponent<PlayerManager>();
                if (playerManager != null && playerManager.IsDead())
                    continue;

                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance <= 15f)
                    return true;
            }
        }

        return false;
    }
}
