using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Bewegt den Boss kontinuierlich zum näheren der beiden Spieler
/// </summary>
[TaskDescription("Bewegt den Boss zum näheren Spieler. Wechselt automatisch das Ziel.")]
public class MoveToClosestPlayer : Action
{
    [UnityEngine.Tooltip("Referenz zu Spieler 1")]
    [RequiredField]
    public SharedGameObject player1;

    [UnityEngine.Tooltip("Referenz zu Spieler 2")]
    [RequiredField]
    public SharedGameObject player2;

    [UnityEngine.Tooltip("Mindestdistanz zum Ziel")]
    public SharedFloat stoppingDistance = 2f;

    [UnityEngine.Tooltip("Wie oft pro Sekunde soll das Ziel neu berechnet werden?")]
    public float updateTargetInterval = 0.2f;

    [UnityEngine.Tooltip("Soll der Animator beim Movement aktiviert werden?")]
    public bool useAnimator = true;

    [UnityEngine.Tooltip("Name des Bool-Parameters im Animator (z.B. 'isRunning')")]
    public SharedString animatorBoolParameter = "isRunning";

    [UnityEngine.Tooltip("Optionales Output: Aktuelles Ziel")]
    public SharedGameObject currentTarget;

    [UnityEngine.Tooltip("Timeout: Nach wie vielen Sekunden gibt die Action Success zurück?")]
    public SharedFloat timeout = 0.5f;

    private NavMeshAgent agent;
    private Animator animator;
    private float nextUpdateTime;
    private GameObject lastTarget;
    private bool isWithinRange = false;
    private float startTime;

    public override void OnStart()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        startTime = Time.time;

        if (agent == null)
        {
            Debug.LogError("MoveToClosestPlayer: Kein NavMeshAgent gefunden!");
            return;
        }

        if (player1.Value == null || player2.Value == null)
        {
            Debug.LogError("MoveToClosestPlayer: Player-Referenzen fehlen!");
            return;
        }

        agent.stoppingDistance = stoppingDistance.Value;
        nextUpdateTime = 0f;

        // Aktiviere Walk-Animation
        if (useAnimator && animator != null && !string.IsNullOrEmpty(animatorBoolParameter.Value))
        {
            animator.SetBool(animatorBoolParameter.Value, true);
        }

        // Initiales Ziel setzen
        UpdateClosestTarget();
    }

    public override TaskStatus OnUpdate()
    {
        if (agent == null || player1.Value == null || player2.Value == null)
        {
            return TaskStatus.Failure;
        }

        // Timeout Check - gibt Success zurück damit der Tree weiterläuft
        if (Time.time - startTime >= timeout.Value)
        {
            return TaskStatus.Success;
        }

        // Aktualisiere das Ziel periodisch
        if (Time.time >= nextUpdateTime)
        {
            UpdateClosestTarget();
            nextUpdateTime = Time.time + updateTargetInterval;
        }

        // Prüfe Distanz zum Ziel
        if (currentTarget.Value != null)
        {
            float distanceToTarget = Vector3.Distance(
                transform.position,
                currentTarget.Value.transform.position
            );
            bool wasWithinRange = isWithinRange;
            isWithinRange = distanceToTarget <= agent.stoppingDistance;

            // Setze Ziel nur wenn wir zu weit weg sind
            if (!isWithinRange)
            {
                agent.SetDestination(currentTarget.Value.transform.position);
            }
            else
            {
                // Stoppe den Agent wenn wir in Reichweite sind
                if (agent.hasPath)
                {
                    agent.ResetPath();
                }
            }

            // Update Animation basierend auf Movement
            if (
                useAnimator
                && animator != null
                && !string.IsNullOrEmpty(animatorBoolParameter.Value)
            )
            {
                // Prüfe ob der Boss sich tatsächlich bewegt
                bool isMoving = agent.velocity.magnitude > 0.1f;
                animator.SetBool(animatorBoolParameter.Value, isMoving);
            }
        }

        // Läuft bis zum Timeout
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        // Deaktiviere Walk-Animation
        if (useAnimator && animator != null && !string.IsNullOrEmpty(animatorBoolParameter.Value))
        {
            animator.SetBool(animatorBoolParameter.Value, false);
        }

        if (agent != null && agent.isOnNavMesh)
        {
            agent.ResetPath();
        }
    }

    private void UpdateClosestTarget()
    {
        float distanceToPlayer1 = Vector3.Distance(
            transform.position,
            player1.Value.transform.position
        );
        float distanceToPlayer2 = Vector3.Distance(
            transform.position,
            player2.Value.transform.position
        );

        GameObject newTarget =
            (distanceToPlayer1 < distanceToPlayer2) ? player1.Value : player2.Value;

        // Nur loggen wenn sich das Ziel ändert
        if (newTarget != lastTarget)
        {
            lastTarget = newTarget;
        }

        currentTarget.Value = newTarget;
    }
}
