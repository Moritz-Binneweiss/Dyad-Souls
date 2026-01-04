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
    private GameObject trailEffect;
    private GameObject waterfowlEffect;
    private GameObject windSlashEffect;

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

        // Find and disable Trail effect
        Transform trailTransform = currentSword.transform.Find("Trail");
        if (trailTransform != null)
        {
            trailEffect = trailTransform.gameObject;
            trailEffect.SetActive(false);
        }

        // Find and disable WaterfowlDance effect
        Transform waterfowlDanceTransform = currentSword.transform.Find("WaterfowlEffect");
        if (waterfowlDanceTransform != null)
        {
            waterfowlEffect = waterfowlDanceTransform.gameObject;
            waterfowlEffect.SetActive(false);
        }

        // Find and disable WindSlash effect
        Transform windSlashTransform = currentSword.transform.Find("WindSlash");
        if (windSlashTransform != null)
        {
            windSlashEffect = windSlashTransform.gameObject;
            windSlashEffect.SetActive(false);
        }
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
        if (animator != null)
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

            animator.SetTrigger("HeavyAttack");
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

    // Animation Event Methods for Trail Effect
    public void SlashEffectOn()
    {
        if (trailEffect != null)
            trailEffect.SetActive(true);
    }

    public void SlashEffectOff()
    {
        if (trailEffect != null)
            trailEffect.SetActive(false);
    }

    // Animation Event Methods for WindSlash Effect
    public void WindSlashesOn()
    {
        if (windSlashEffect != null)
            windSlashEffect.SetActive(true);
    }

    public void WindSlashesOff()
    {
        if (windSlashEffect != null)
            windSlashEffect.SetActive(false);
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
