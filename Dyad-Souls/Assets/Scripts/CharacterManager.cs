using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [HideInInspector]
    public CharacterController characterController;

    [HideInInspector]
    public Animator animator;

    protected virtual void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // Validate required components
        if (animator == null)
        {
            Debug.LogError($"Animator component not found on {gameObject.name}! Please add an Animator component.", this);
        }
        else if (animator.runtimeAnimatorController == null)
        {
            Debug.LogError($"Animator Controller not assigned to {gameObject.name}! Please assign an Animator Controller.", this);
        }
    }

    protected virtual void Update() { }

    protected virtual void LateUpdate() { }
}
