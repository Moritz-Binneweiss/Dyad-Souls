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
    string heavyAttackAnimation = "HeavyAttack";

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
        animator = GetComponent<Animator>();

        // Find right hand bone if not assigned
        if (rightHandBone == null)
        {
            rightHandBone = FindRightHandBone(transform);
        }
    }

    private void Start()
    {
        EquipSword();
    }

    private Transform FindRightHandBone(Transform root)
    {
        // Recursively search for "RightHand" bone
        foreach (Transform child in root.GetComponentsInChildren<Transform>())
        {
            if (child.name.Contains("RightHand") || child.name.Contains("Right_Hand"))
            {
                return child;
            }
        }
        return null;
    }

    private void EquipSword()
    {
        if (swordPrefab == null || rightHandBone == null)
        {
            Debug.LogWarning("Sword prefab or right hand bone not assigned!");
            return;
        }

        currentSword = Instantiate(swordPrefab, rightHandBone);
        currentSword.transform.localPosition = Vector3.zero;
        currentSword.transform.localRotation = Quaternion.identity;

        weaponCollider = currentSword.GetComponentInChildren<Collider>();
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
        else
        {
            Debug.LogError("No collider found on sword or its children!");
        }

        weaponDamage = currentSword.GetComponentInChildren<WeaponDamage>();
        if (weaponDamage == null)
        {
            Debug.LogWarning("No WeaponDamage component found on sword or its children!");
        }
    }

    public void PerformAttack()
    {
        if (animator != null)
        {
            // Setze Damage für Light Attack
            if (weaponDamage != null)
            {
                weaponDamage.SetLightAttackDamage();
            }

            animator.SetTrigger("Attack");
        }
    }

    public void PerformHeavyAttack()
    {
        if (animator != null && !string.IsNullOrEmpty(heavyAttackAnimation))
        {
            // Setze Damage für Heavy Attack (100)
            if (weaponDamage != null)
            {
                weaponDamage.SetHeavyAttackDamage();
            }

            animator.CrossFade(heavyAttackAnimation, 0.2f, 0);
        }
    }

    public void EnableWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
        }
    }

    public void DisableWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
    }

    public void PerformDodge()
    {
        if (animator != null && player != null && player.playerInputManager != null)
        {
            // Check if player is moving
            float moveAmount = player.playerInputManager.moveAmount;

            if (moveAmount > 0.1f)
            {
                // Player is moving -> Dodge Roll
                animator.SetTrigger("Dodge");
            }
            else
            {
                // Player is standing still -> Backstep
                animator.SetTrigger("DodgeBackstep");
            }
        }
    }

    public void PerformSpecialAttack()
    {
        if (animator != null)
        {
            // Setze Damage für Special Attack (150)
            if (weaponDamage != null)
            {
                weaponDamage.SetSpecialAttackDamage();
            }

            animator.SetTrigger("SpecialAttack");
        }
    }
}
