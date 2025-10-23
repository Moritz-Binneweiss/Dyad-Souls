using UnityEngine;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager player;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
    {
        weaponAction.AttemptToPerformAction(player, weaponPerformingAction);
    }
}
