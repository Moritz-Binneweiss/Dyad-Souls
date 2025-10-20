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
    }

    protected virtual void Update() { }

    protected virtual void LateUpdate() { }
}
