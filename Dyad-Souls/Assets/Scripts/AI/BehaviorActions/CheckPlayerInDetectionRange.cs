using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Boss")]
[TaskDescription("Prüft ob mindestens ein Spieler im Detektionsradius ist, bevor Attacken ausgeführt werden")]
public class CheckPlayerInDetectionRange : Conditional
{
   
    public float detectionRadius = 20f;
    

    public LayerMask obstacleMask;
    
   
    public bool checkLineOfSight = false;
    
   
    public float raycastHeightOffset = 1.5f;
    
    public SharedGameObject playerOne;
    public SharedGameObject playerTwo;
    public SharedGameObject closestPlayerInRange;
    
    public override TaskStatus OnUpdate()
    {
        // Check if we have at least one player assigned
        if (playerOne.Value == null && playerTwo.Value == null)
        {
            // Fallback: Suche Spieler mit Tag
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length > 0)
            {
                playerOne.Value = players[0];
                if (players.Length > 1)
                {
                    playerTwo.Value = players[1];
                }
            }
            else
            {
                return TaskStatus.Failure;
            }
        }

        GameObject closestPlayer = null;
        float closestDistance = float.MaxValue;
        
        // Check Player One
        if (playerOne.Value != null)
        {
            float distance = Vector3.Distance(transform.position, playerOne.Value.transform.position);
            
            if (distance <= detectionRadius)
            {
                // Optional: Line of Sight Check
                if (!checkLineOfSight || HasLineOfSight(playerOne.Value))
                {
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPlayer = playerOne.Value;
                    }
                }
            }
        }
        
        // Check Player Two
        if (playerTwo.Value != null)
        {
            float distance = Vector3.Distance(transform.position, playerTwo.Value.transform.position);
            
            if (distance <= detectionRadius)
            {
                // Optional: Line of Sight Check
                if (!checkLineOfSight || HasLineOfSight(playerTwo.Value))
                {
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPlayer = playerTwo.Value;
                    }
                }
            }
        }
        
        // Wenn ein Spieler im Radius gefunden wurde
        if (closestPlayer != null)
        {
            closestPlayerInRange.Value = closestPlayer;
            return TaskStatus.Success;
        }
        
        // Kein Spieler im Radius
        closestPlayerInRange.Value = null;
        return TaskStatus.Failure;
    }
    
    /// <summary>
    /// Prüft ob der Boss freie Sicht zum Spieler hat (keine Hindernisse dazwischen)
    /// </summary>
    private bool HasLineOfSight(GameObject player)
    {
        if (player == null) return false;
        
        Vector3 startPos = transform.position + Vector3.up * raycastHeightOffset;
        Vector3 targetPos = player.transform.position + Vector3.up * raycastHeightOffset;
        Vector3 direction = (targetPos - startPos).normalized;
        float distance = Vector3.Distance(startPos, targetPos);
        
        // Raycast zum Spieler
        if (Physics.Raycast(startPos, direction, out RaycastHit hit, distance, obstacleMask))
        {
            // Wenn der Raycast etwas trifft, das NICHT der Spieler ist, keine Sicht
            if (!hit.collider.CompareTag("Player"))
            {
                return false;
            }
        }
        
        return true;
    }
    
    /// <summary>
    /// Zeichnet den Detektionsradius im Editor (Gizmos)
    /// </summary>
    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        // Zeichne Linie zu Spielern, die im Radius sind
        if (Application.isPlaying && closestPlayerInRange != null && closestPlayerInRange.Value != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, closestPlayerInRange.Value.transform.position);
        }
    }
}
