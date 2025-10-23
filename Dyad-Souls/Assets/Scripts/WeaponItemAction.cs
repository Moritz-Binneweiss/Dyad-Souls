using UnityEngine;


[CreateAssetMenu(menuName = "Character Action/Weapon Action/ Test Action")]
public class WeaponItemAction : ScriptableObject
{
    public int actionID;
    public virtual void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {

    }
}
