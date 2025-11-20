using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Steuert die Bewegungs-Animation basierend auf NavMeshAgent Velocity
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class BossMovementAnimator : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;

    [Header("Animation Parameters")]
    [SerializeField] private string walkSpeedParameter = "WalkSpeed";
    [SerializeField] private string isWalkingParameter = "IsWalking";

    [Header("Settings")]
    [SerializeField] private float walkThreshold = 0.1f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (animator == null || agent == null)
            return;

        // Berechne aktuelle Geschwindigkeit
        float currentSpeed = agent.velocity.magnitude;
        float normalizedSpeed = currentSpeed / agent.speed;

        // Setze Animation Parameter
        animator.SetFloat(walkSpeedParameter, normalizedSpeed);
        animator.SetBool(isWalkingParameter, currentSpeed > walkThreshold);
    }
}
