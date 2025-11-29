using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class AttackLeftHand : Action
{
    public SharedGameObject target;

    public string animationName = "AttackLeftHand";

    public SharedFloat attackDuration = 1.5f;

    public SharedFloat attackRange = 3f;

    public SharedFloat damage = 20f;

    private float timer;
    private bool attackStarted;

    public override void OnStart()
    {
        timer = 0f;
        attackStarted = false;

        if (target.Value != null)
        {
            float distance = Vector3.Distance(transform.position, target.Value.transform.position);

            if (distance <= attackRange.Value)
            {
                attackStarted = true;

                Animator animator = GetComponent<Animator>();
                if (animator != null)
                    animator.Play(animationName);
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
