using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class BossMovementAnimator : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;

    [Header("Animation Parameters")]
    [SerializeField]
    private string walkSpeedParameter = "WalkSpeed";

    [SerializeField]
    private string isWalkingParameter = "IsWalking";

    [Header("Settings")]
    [SerializeField]
    private float walkThreshold = 0.1f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (animator == null || agent == null)
            return;

        float currentSpeed = agent.velocity.magnitude;
        float normalizedSpeed = currentSpeed / agent.speed;

        animator.SetFloat(walkSpeedParameter, normalizedSpeed);
        animator.SetBool(isWalkingParameter, currentSpeed > walkThreshold);
    }
}
