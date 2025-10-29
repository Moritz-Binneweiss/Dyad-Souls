using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    PlayerManager player;

    [Header("Right Hand Weapon")]
    public WeaponModelInstantiationSlot rightHandSlot;
    public GameObject rightHandWeaponModel;

    [SerializeField]
    WeaponManager rightWeaponManager;

    protected virtual void Awake()
    {
        player = GetComponent<PlayerManager>();
        InitializeWeaponSlot();
    }

    protected virtual void Start()
    {
        LoadRightWeapon();
    }

    private void InitializeWeaponSlot()
    {
        WeaponModelInstantiationSlot[] weaponSlots =
            GetComponentsInChildren<WeaponModelInstantiationSlot>();

        foreach (var weaponSlot in weaponSlots)
        {
            if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
            {
                rightHandSlot = weaponSlot;
                break;
            }
        }
    }

    public void LoadRightWeapon()
    {
        if (player.playerInventoryManager.currentRightHandWeapon != null)
        {
            rightHandSlot.UnloadWeaponModel();

            rightHandWeaponModel = Instantiate(
                player.playerInventoryManager.currentRightHandWeapon.weaponModel
            );
            rightHandSlot.LoadWeaponModel(rightHandWeaponModel);
            rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
            rightWeaponManager.SetWeaponDamage(
                player,
                player.playerInventoryManager.currentRightHandWeapon
            );
        }
    }
}
