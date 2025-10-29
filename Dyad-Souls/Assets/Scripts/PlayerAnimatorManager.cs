using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
    PlayerManager player;

    protected virtual void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    public void UpdateAnimatorMovementParameter(float horizontalValue, float verticalValue)
    {
        if (player == null || player.animator == null)
        {
            Debug.LogWarning("Player or animator is null in UpdateAnimatorMovementParameter");
            return;
        }

        player.animator.SetFloat("Horizontal", horizontalValue, 0.1f, Time.deltaTime);
        player.animator.SetFloat("Vertical", verticalValue, 0.1f, Time.deltaTime);
    }

    public virtual void PlayTargetAttackAnimation(
        string targetAnimation,
        bool isPerformingAction,
        bool applyRootMotion = true,
        bool canRotate = false,
        bool canMove = false
    )
    {
        if (player == null)
        {
            Debug.LogError("Player is null in PlayTargetAttackAnimation!");
            return;
        }

        if (player.animator == null)
        {
            Debug.LogError("Animator is null on player: " + player.gameObject.name);
            return;
        }

        if (string.IsNullOrEmpty(targetAnimation))
        {
            Debug.LogError("Target animation name is null or empty!");
            return;
        }

        Debug.Log(
            $"Attempting to play animation/state: '{targetAnimation}' on {player.gameObject.name}"
        );

        try
        {
            player.animator.CrossFade(targetAnimation, 0.2f, 0);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(
                $"Failed to play animation '{targetAnimation}' on {player.gameObject.name}: {ex.Message}"
            );
        }
    }
}
