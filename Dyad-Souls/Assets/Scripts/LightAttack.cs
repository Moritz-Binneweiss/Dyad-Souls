using UnityEngine;

[CreateAssetMenu(menuName = "Character Action/Weapon Action/ Light Attack")]
public class LightAttack : WeaponItemAction
{
    [SerializeField]
    string lightAttackAnimationName = "MainLightAttack";

    public override void AttemptToPerformAction(
        PlayerManager playerPerformingAction,
        WeaponItem weaponPerformingAction
    )
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        PerformLightAttackAction(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformLightAttackAction(
        PlayerManager playerPerformingAction,
        WeaponItem weaponPerformingAction
    )
    {
        // Check if playerAnimatorManager exists before using it
        if (playerPerformingAction.playerAnimatorManager == null)
        {
            Debug.LogError("PlayerAnimatorManager is null on " + playerPerformingAction.gameObject.name);
            return;
        }

        // Debug: Log the animation name we're trying to play
        Debug.Log($"Attempting to play animation: {lightAttackAnimationName}");

        // Attempt to play the attack animation
        playerPerformingAction.playerAnimatorManager.PlayTargetAttackAnimation(
            lightAttackAnimationName,
            true
        );
    }
}
