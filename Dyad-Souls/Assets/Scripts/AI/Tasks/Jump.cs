

using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Jump : Action
{
    public SharedTransform player;          // wird im Tree zugewiesen
    public SharedFloat jumpHeight = 4f;     // wie hoch der Sprung geht
    public SharedFloat jumpSpeed = 6f;      // horizontale Geschwindigkeit
    public SharedFloat windupTime = 0.5f;   // kleine Vorbereitung
    public SharedFloat landTime = 0.4f;     // Zeit nach der Landung

    private Animator animator;
    private float elapsed;
    private Vector3 startPos;
    private Vector3 targetPos;
    private bool isJumping;

    public override void OnStart()
    {
        animator = GetComponent<Animator>();
        startPos = transform.position;

        if (player.Value != null)
        {
            Vector3 dir = (player.Value.position - transform.position).normalized;
            targetPos = transform.position + dir * 5f; // Sprungdistanz (5 Meter)
        }
        else
        {
            targetPos = transform.position + transform.forward * 5f;
        }

        elapsed = 0f;
        isJumping = false;

        if (animator != null)
            animator.SetTrigger("jumpAttack");
    }

    public override TaskStatus OnUpdate()
    {
        elapsed += Time.deltaTime;

        // 1. Windup (vorbereitungszeit)
        if (elapsed < windupTime.Value)
            return TaskStatus.Running;

        // 2. Sprungphase
        if (!isJumping)
        {
            isJumping = true;
            startPos = transform.position;
        }

        float t = (elapsed - windupTime.Value) * (jumpSpeed.Value / 10f);
        if (t > 1f) t = 1f;

        // Parabolische Bewegung (einfach)
        Vector3 pos = Vector3.Lerp(startPos, targetPos, t);
        pos.y += Mathf.Sin(t * Mathf.PI) * jumpHeight.Value;
        transform.position = pos;

        // Ende erreicht?
        if (t >= 1f)
        {
            if (elapsed >= windupTime.Value + landTime.Value)
                return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
