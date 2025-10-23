using UnityEngine;
using UnityEngine.AI;

public class BossScript : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Wandering Settings")]
    [SerializeField]
    private float wanderRadius = 20f; // Radius in dem der Boss herumläuft

    [SerializeField]
    private float wanderTimer = 5f; // Zeit bis zum nächsten Ziel

    [Header("Animation Settings")]

    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        timer = wanderTimer;

        // Setze sofort ein erstes Ziel
        SetRandomDestination();
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Wenn Timer abgelaufen ist oder Ziel erreicht wurde, neues Ziel setzen
        if (
            timer >= wanderTimer
            || !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance
        )
        {
            SetRandomDestination();
            timer = 0;
        }

        // Animator Parameter basierend auf Geschwindigkeit setzen
        UpdateAnimator();
    }

    void UpdateAnimator()
    {
        if (animator != null)
        {
            // Setze Walking-Animation basierend auf der Geschwindigkeit
            bool isMoving = agent.velocity.magnitude > 0.1f;
            animator.SetBool("isRunning", isMoving);
        }
    }

    void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}
