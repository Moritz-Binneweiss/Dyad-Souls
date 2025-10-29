using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    PlayerManager player;

    protected virtual void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    public void PerformLightAttack()
    {
        if (player.playerInventoryManager.currentRightHandWeapon == null)
        {
            Debug.LogWarning("No weapon equipped!");
            return;
        }

        WeaponItem weapon = player.playerInventoryManager.currentRightHandWeapon;

        if (player.playerAnimatorManager == null)
        {
            Debug.LogError("PlayerAnimatorManager is null!");
            return;
        }

        player.playerAnimatorManager.PlayTargetAttackAnimation(weapon.lightAttackAnimation, true);
    }
}
