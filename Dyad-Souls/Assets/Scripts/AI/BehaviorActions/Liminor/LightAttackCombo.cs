using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class LightAttackCombo : Action
{
    public SharedGameObject target;

    public string animationName = "LightAttackCombo";

    public SharedFloat attackDuration = 7f;

    private EnemyDamage enemyDamage;
    private float timer;
    private bool attackStarted;

    public override void OnAwake()
    {
        enemyDamage = GetComponent<EnemyDamage>();
    }

    public override void OnStart()
    {
        timer = 0f;
        attackStarted = false;

        if (target.Value != null && enemyDamage != null)
        {
            float distance = Vector3.Distance(transform.position, target.Value.transform.position);

            if (distance <= enemyDamage.GetHeavyAttackRange())
            {
                attackStarted = true;

                Animator animator = GetComponent<Animator>();
                if (animator != null)
                    animator.CrossFade(animationName, 0.15f); // Faster blend for combo
            }
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (!attackStarted)
            return TaskStatus.Failure;

        timer += Time.deltaTime;

        if (timer < attackDuration.Value)
            return TaskStatus.Running;

        return TaskStatus.Success;
    }
}
