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

    [Header("Combat Settings")]
    [SerializeField]
    string lightAttackAnimation = "MainLightAttack";

    [Header("Dodge Settings")]
    [SerializeField]
    string dodgeAnimation = "Backstep";

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
    }

    public void PerformLightAttack()
    {
        if (animator != null && !string.IsNullOrEmpty(lightAttackAnimation))
        {
            animator.CrossFade(lightAttackAnimation, 0.2f, 0);
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
        if (animator != null && !string.IsNullOrEmpty(dodgeAnimation))
        {
            animator.CrossFade(dodgeAnimation, 0.1f, 0);
        }
    }
}
