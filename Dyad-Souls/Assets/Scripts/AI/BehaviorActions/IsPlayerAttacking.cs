using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class IsPlayerAttacking : Conditional
{
    public SharedGameObject player1;
    public SharedGameObject player2;

    public SharedFloat detectionRange = 5f;

    public override TaskStatus OnUpdate()
    {
        if (IsAttacking(player1.Value) || IsAttacking(player2.Value))
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }

    private bool IsAttacking(GameObject player)
    {
        if (player == null)
            return false;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance > detectionRange.Value)
            return false;

        Animator playerAnimator = player.GetComponent<Animator>();
        if (playerAnimator != null)
        {
            AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

            return stateInfo.IsName("Attack")
                || stateInfo.IsName("LightAttack")
                || stateInfo.IsName("HeavyAttack")
                || stateInfo.IsName("ComboAttack")
                || playerAnimator.GetBool("IsAttacking");
        }

        return false;
    }
}
