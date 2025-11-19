using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/// <summary>
/// Prüft ob ein Spieler gerade angreift und der Boss dodgen sollte
/// </summary>
public class IsPlayerAttacking : Conditional
{
    public SharedGameObject player1;
    public SharedGameObject player2;

    public SharedFloat detectionRange = 5f;
    
    public SharedFloat dodgeChance = 70f;

    private static float lastDodgeTime;
    public SharedFloat dodgeCooldown = 2f;

    public override TaskStatus OnUpdate()
    {
        // Cooldown Check
        if (Time.time - lastDodgeTime < dodgeCooldown.Value)
        {
            return TaskStatus.Failure;
        }

        // Prüfe beide Spieler
        if (IsAttacking(player1.Value) || IsAttacking(player2.Value))
        {
            // Zufalls-Chance für Dodge
            if (Random.Range(0f, 100f) <= dodgeChance.Value)
            {
                lastDodgeTime = Time.time;
                return TaskStatus.Success;
            }
        }

        return TaskStatus.Failure;
    }

    private bool IsAttacking(GameObject player)
    {
        if (player == null)
            return false;

        // Prüfe Distanz
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance > detectionRange.Value)
            return false;

        // Prüfe ob Spieler am Angreifen ist (über Animator)
        Animator playerAnimator = player.GetComponent<Animator>();
        if (playerAnimator != null)
        {
            // Passe diese Parameter an deine Spieler-Animationen an
            AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
            
            // Prüfe ob in einer Attack-Animation
            return stateInfo.IsName("Attack") || 
                   stateInfo.IsName("LightAttack") || 
                   stateInfo.IsName("HeavyAttack") ||
                   stateInfo.IsName("ComboAttack") ||
                   playerAnimator.GetBool("IsAttacking");
        }

        return false;
    }
}
