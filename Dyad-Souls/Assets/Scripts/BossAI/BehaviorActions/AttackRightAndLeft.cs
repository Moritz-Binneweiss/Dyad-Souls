using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/// <summary>
/// Beide Hände Combo Attacke
/// </summary>
public class AttackRightAndLeft : Action
{
    public SharedGameObject target;

    public string animationName = "RightAndLeftAttack";

    public SharedFloat attackDuration = 2.5f;

    public SharedFloat attackRange = 3f;

    public SharedFloat damage = 30f;

    private float timer;
    private bool attackStarted;

    public override void OnStart()
    {
        timer = 0f;
        attackStarted = false;

        // Prüfe ob Spieler in Reichweite ist
        if (target.Value != null)
        {
            float distance = Vector3.Distance(transform.position, target.Value.transform.position);
            
            if (distance <= attackRange.Value)
            {
                attackStarted = true;
                
                Animator animator = GetComponent<Animator>();
                if (animator != null)
                 {
                     animator.Play(animationName);
                }

                Debug.Log($"Right And Left Attack gestartet: {animationName}");
            }
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (!attackStarted)
        {
            return TaskStatus.Failure;
        }

        timer += Time.deltaTime;

        // Attacke läuft noch
        if (timer < attackDuration.Value)
        {
            return TaskStatus.Running;
        }

        // Attacke abgeschlossen
        // HIER: Damage anwenden, Effekte spawnen, etc.
        Debug.Log($"Right And Left Attack abgeschlossen: {animationName}");
        
        return TaskStatus.Success;
    }
}
