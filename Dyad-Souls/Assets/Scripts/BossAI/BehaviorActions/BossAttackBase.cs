using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/// <summary>
/// Basis-Klasse für Boss-Angriffe mit Animation und Cooldown
/// </summary>
public abstract class BossAttackBase : Action
{
    [UnityEngine.Tooltip("Name des Animation Triggers im Animator")]
    [RequiredField]
    public SharedString animationTrigger;

    [UnityEngine.Tooltip("Dauer der Angriffs-Animation in Sekunden")]
    public SharedFloat attackDuration = 1.5f;

    [UnityEngine.Tooltip("Cooldown in Sekunden bevor dieser Angriff wieder verwendet werden kann")]
    public SharedFloat cooldownTime = 2f;

    [UnityEngine.Tooltip("Chance (0-100), dass dieser Angriff gewählt wird")]
    public SharedFloat attackChance = 100f;

    [UnityEngine.Tooltip("Ziel des Angriffs")]
    public SharedGameObject target;

    [UnityEngine.Tooltip("Soll der Boss zum Ziel schauen?")]
    public bool faceTarget = true;

    protected Animator animator;
    protected float attackStartTime;
    private static System.Collections.Generic.Dictionary<string, float> lastAttackTimes = new System.Collections.Generic.Dictionary<string, float>();

    public override void OnStart()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError($"{GetType().Name}: Kein Animator gefunden!");
            return;
        }

        // Prüfe Cooldown
        string attackKey = GetType().Name;
        if (lastAttackTimes.ContainsKey(attackKey))
        {
            float timeSinceLastAttack = Time.time - lastAttackTimes[attackKey];
            if (timeSinceLastAttack < cooldownTime.Value)
            {
                // Noch im Cooldown
                return;
            }
        }

        // Prüfe Chance
        float roll = Random.Range(0f, 100f);
        if (roll > attackChance.Value)
        {
            // Angriff nicht ausgewählt
            return;
        }

        // Schaue zum Ziel
        if (faceTarget && target != null && target.Value != null)
        {
            Vector3 direction = (target.Value.transform.position - transform.position).normalized;
            direction.y = 0; // Nur horizontal drehen
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }

        // Triggere Animation
        if (!string.IsNullOrEmpty(animationTrigger.Value))
        {
            animator.SetTrigger(animationTrigger.Value);
            Debug.Log($"{GetType().Name}: Triggere Animation '{animationTrigger.Value}'");
        }

        attackStartTime = Time.time;
        lastAttackTimes[attackKey] = Time.time;

        OnAttackStart();
    }

    public override TaskStatus OnUpdate()
    {
        if (animator == null)
        {
            return TaskStatus.Failure;
        }

        // Warte bis Animation fertig ist
        float elapsedTime = Time.time - attackStartTime;
        if (elapsedTime < attackDuration.Value)
        {
            OnAttackUpdate(elapsedTime);
            return TaskStatus.Running;
        }

        OnAttackComplete();
        return TaskStatus.Success;
    }

    // Override diese Methoden für spezifisches Verhalten
    protected virtual void OnAttackStart() { }
    protected virtual void OnAttackUpdate(float elapsedTime) { }
    protected virtual void OnAttackComplete() { }
}
