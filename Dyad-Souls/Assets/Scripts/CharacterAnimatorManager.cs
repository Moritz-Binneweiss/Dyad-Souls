using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public void UpdateAnimatorMovementParameter(float horizontalValue, float verticalValue)
    {
        if (character == null || character.animator == null)
        {
            Debug.LogWarning("Character or animator is null in UpdateAnimatorMovementParameter");
            return;
        }

        character.animator.SetFloat("Horizontal", horizontalValue, 0.1f, Time.deltaTime);
        character.animator.SetFloat("Vertical", verticalValue, 0.1f, Time.deltaTime);
    }

    public virtual void PlayTargetAttackAnimation(
        string targetAnimation,
        bool isPerformingAction,
        bool applyRootMotion = true,
        bool canRotate = false,
        bool canMove = false)
    {
        // Add safety checks
        if (character == null)
        {
            Debug.LogError("Character is null in PlayTargetAttackAnimation!");
            return;
        }

        if (character.animator == null)
        {
            Debug.LogError("Animator is null on character: " + character.gameObject.name);
            return;
        }

        if (string.IsNullOrEmpty(targetAnimation))
        {
            Debug.LogError("Target animation name is null or empty!");
            return;
        }

        // Log available animations for debugging
        LogAvailableAnimations();
        
        Debug.Log($"Attempting to play animation/state: '{targetAnimation}' on {character.gameObject.name}");

        try
        {
            // Explicitly specify layer 0 (base layer) to avoid layer index issues
            character.animator.CrossFade(targetAnimation, 0.2f, 0);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to play animation '{targetAnimation}' on {character.gameObject.name}: {ex.Message}");
        }
    }

    private bool HasAnimation(string animationName)
    {
        if (character.animator == null || character.animator.runtimeAnimatorController == null)
            return false;

        // For now, let's be more permissive and let Unity handle the validation
        // This avoids false negatives when checking state names vs clip names
        // Unity's CrossFade will handle invalid names gracefully
        
        // Still log available animations for debugging
        LogAvailableAnimations();
        return true;
    }

    private void LogAvailableAnimations()
    {
        if (character.animator == null || character.animator.runtimeAnimatorController == null)
        {
            Debug.Log("No animator or animator controller found.");
            return;
        }

        Debug.Log($"Available animation clips in {character.gameObject.name}:");
        foreach (var clip in character.animator.runtimeAnimatorController.animationClips)
        {
            Debug.Log($"- Clip: {clip.name}");
        }
    }
}
