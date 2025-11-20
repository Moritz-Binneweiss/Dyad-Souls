using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/// <summary>
/// TEMPLATE für deine eigenen Attacken
/// Kopiere dieses Script und passe es an deine Bedürfnisse an
/// </summary>
public class AttackTemplate : Action
{
    public SharedGameObject target;

    public string animationName = "Attack";

    public SharedFloat attackDuration = 1.5f;

    public SharedFloat attackRange = 3f;

    public SharedFloat damage = 20f;

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
                
                // HIER: Spiele Animation ab
                // Animator animator = GetComponent<Animator>();
                // if (animator != null)
                // {
                //     animator.Play(animationName);
                // }

                Debug.Log($"Attacke gestartet: {animationName}");
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
        Debug.Log($"Attacke abgeschlossen: {animationName}");
        
        return TaskStatus.Success;
    }
}
