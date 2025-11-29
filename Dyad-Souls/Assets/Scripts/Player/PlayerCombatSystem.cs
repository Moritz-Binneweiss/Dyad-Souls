using UnityEngine;

public class PlayerCombatSystem : MonoBehaviour
{
    [Header("Components")]
    private PlayerManager player;
    private Animator animator;

    [Header("Weapon Setup")]
    public GameObject swordPrefab;
    public Transform rightHandBone;
    private GameObject currentSword;
    private Collider weaponCollider;
    private WeaponDamage weaponDamage;

    [Header("Combat Settings")]
    [SerializeField]
    private string heavyAttackAnimation = "HeavyAttack";

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
        animator = GetComponent<Animator>();

        if (rightHandBone == null)
            rightHandBone = FindRightHandBone(transform);
    }

    private void Start()
    {
        EquipSword();
    }

    private Transform FindRightHandBone(Transform root)
    {
        foreach (Transform child in root.GetComponentsInChildren<Transform>())
        {
            if (child.name.Contains("RightHand") || child.name.Contains("Right_Hand"))
                return child;
        }
        return null;
    }

    private void EquipSword()
    {
        if (swordPrefab == null || rightHandBone == null)
            return;

        currentSword = Instantiate(swordPrefab, rightHandBone);
        currentSword.transform.localPosition = Vector3.zero;
        currentSword.transform.localRotation = Quaternion.identity;

        weaponCollider = currentSword.GetComponentInChildren<Collider>();
        if (weaponCollider != null)
            weaponCollider.enabled = false;

        weaponDamage = currentSword.GetComponentInChildren<WeaponDamage>();
    }

    public void PerformAttack()
    {
        if (animator != null)
        {
            if (player.playerStaminaSystem != null)
            {
                if (
                    !player.playerStaminaSystem.ConsumeStamina(
                        player.playerStaminaSystem.GetLightAttackCost()
                    )
                )
                    return;
            }

            if (weaponDamage != null)
                weaponDamage.SetLightAttackDamage();

            animator.SetTrigger("Attack");
        }
    }

    public void PerformHeavyAttack()
    {
        if (animator != null && !string.IsNullOrEmpty(heavyAttackAnimation))
        {
            if (player.playerStaminaSystem != null)
            {
                if (
                    !player.playerStaminaSystem.ConsumeStamina(
                        player.playerStaminaSystem.GetHeavyAttackCost()
                    )
                )
                    return;
            }

            if (weaponDamage != null)
                weaponDamage.SetHeavyAttackDamage();

            animator.CrossFade(heavyAttackAnimation, 0.2f, 0);
        }
    }

    public void EnableWeaponCollider()
    {
        if (weaponCollider != null)
            weaponCollider.enabled = true;
    }

    public void DisableWeaponCollider()
    {
        if (weaponCollider != null)
            weaponCollider.enabled = false;
    }

    public void PerformDodge()
    {
        if (animator != null && player != null && player.playerInputManager != null)
        {
            float moveAmount = player.playerInputManager.moveAmount;

            if (moveAmount > 0.1f)
            {
                if (player.playerStaminaSystem != null)
                {
                    if (
                        !player.playerStaminaSystem.ConsumeStamina(
                            player.playerStaminaSystem.GetDodgeRollCost()
                        )
                    )
                        return;
                }
                animator.SetTrigger("Dodge");
            }
            else
            {
                if (player.playerStaminaSystem != null)
                {
                    if (
                        !player.playerStaminaSystem.ConsumeStamina(
                            player.playerStaminaSystem.GetDodgeBackstepCost()
                        )
                    )
                        return;
                }
                animator.SetTrigger("DodgeBackstep");
            }
        }
    }

    public void PerformSpecialAttack()
    {
        if (animator != null)
        {
            if (player.playerStaminaSystem != null)
            {
                if (
                    !player.playerStaminaSystem.ConsumeStamina(
                        player.playerStaminaSystem.GetSpecialAttackCost()
                    )
                )
                    return;
            }

            if (weaponDamage != null)
                weaponDamage.SetSpecialAttackDamage();

            animator.SetTrigger("SpecialAttack");
        }
    }
}
