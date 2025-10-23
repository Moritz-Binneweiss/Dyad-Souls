using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Determines if the current agent is within distance of either of two specified players. Returns the closest player if within range.")]
    [TaskCategory("Movement")]
    [TaskIcon("62dc1c328b5c4eb45a90ec7a75cfb747", "0e2ffa7c5e610214eb6d5c71613bbdec")]
    public class WithinDistanceTwoPlayer : Conditional
    {
        [Tooltip("The first player to check distance to")]
        public SharedGameObject m_PlayerOne;
        
        [Tooltip("The second player to check distance to")]
        public SharedGameObject m_PlayerTwo;
        
        [Tooltip("The distance that at least one player needs to be within")]
        public SharedFloat m_Magnitude = 5f;
        
        [Tooltip("If true, the player must be within line of sight to be considered within distance")]
        public SharedBool m_LineOfSight = true;
        
        [Tooltip("The LayerMask of the objects to ignore when performing the line of sight check")]
        public LayerMask m_IgnoreLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
        
        [Tooltip("The raycast offset relative to the pivot position")]
        public SharedVector3 m_Offset = Vector3.zero;
        
        [Tooltip("The target raycast offset relative to the pivot position")]
        public SharedVector3 m_TargetOffset = Vector3.zero;
        
        [Tooltip("Should a debug ray be drawn to the scene view?")]
        public SharedBool m_DrawDebugRay = false;
        
        [Tooltip("The player object that will be set when a player is found within distance (the closest one)")]
        public SharedGameObject m_ReturnedPlayer;
        
        [Tooltip("Should the 2D version be used?")]
        public bool m_UsePhysics2D = false;

        private float m_SqrMagnitude; // distance * distance, optimization so we don't have to take the square root

        public override void OnStart()
        {
            m_SqrMagnitude = m_Magnitude.Value * m_Magnitude.Value;
        }

        /// <summary>
        /// Returns success if either player is within distance of the current object. 
        /// Sets m_ReturnedPlayer to the closest player within range.
        /// </summary>
        public override TaskStatus OnUpdate()
        {
            m_ReturnedPlayer.Value = null;
            
            // Check if we have at least one player assigned
            if (m_PlayerOne.Value == null && m_PlayerTwo.Value == null)
            {
                return TaskStatus.Failure;
            }

            GameObject closestPlayer = null;
            float closestDistanceSqr = float.MaxValue;

            // Check Player One
            if (m_PlayerOne.Value != null && IsWithinDistance(m_PlayerOne.Value))
            {
                float distanceSqr = GetSquareDistance(m_PlayerOne.Value);
                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestPlayer = m_PlayerOne.Value;
                }
            }

            // Check Player Two
            if (m_PlayerTwo.Value != null && IsWithinDistance(m_PlayerTwo.Value))
            {
                float distanceSqr = GetSquareDistance(m_PlayerTwo.Value);
                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestPlayer = m_PlayerTwo.Value;
                }
            }

            if (closestPlayer != null)
            {
                m_ReturnedPlayer.Value = closestPlayer;
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }

        /// <summary>
        /// Is the target player within distance?
        /// </summary>
        private bool IsWithinDistance(GameObject player)
        {
            if (player == null) return false;

            var direction = player.transform.position - (transform.position + m_Offset.Value);
            
            // Check if the square magnitude is less than what is specified
            if (Vector3.SqrMagnitude(direction) < m_SqrMagnitude)
            {
                // The magnitude is less. If lineOfSight is true do one more check
                if (m_LineOfSight.Value)
                {
                    var hitTransform = MovementUtility.LineOfSight(transform, m_Offset.Value, player, m_TargetOffset.Value, m_UsePhysics2D, m_IgnoreLayerMask);
                    if (hitTransform != null && MovementUtility.IsAncestor(hitTransform, player.transform))
                    {
                        // The player has a magnitude less than the specified magnitude and is within sight
                        return true;
                    }
                }
                else
                {
                    // The player has a magnitude less than the specified magnitude
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get the square distance to the target player (for comparison purposes)
        /// </summary>
        private float GetSquareDistance(GameObject player)
        {
            if (player == null) return float.MaxValue;
            
            var direction = player.transform.position - (transform.position + m_Offset.Value);
            return Vector3.SqrMagnitude(direction);
        }

        public override void OnReset()
        {
            m_PlayerOne = null;
            m_PlayerTwo = null;
            m_Magnitude = 5f;
            m_LineOfSight = true;
            m_IgnoreLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
            m_Offset = Vector3.zero;
            m_TargetOffset = Vector3.zero;
            m_DrawDebugRay = false;
            m_ReturnedPlayer = null;
            m_UsePhysics2D = false;
        }

        // Draw the detection radius
        public override void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (Owner == null || m_Magnitude == null || Owner.transform == null)
            {
                return;
            }
            
            var oldColor = UnityEditor.Handles.color;
            UnityEditor.Handles.color = Color.cyan;
            UnityEditor.Handles.DrawWireDisc(Owner.transform.position, m_UsePhysics2D ? Owner.transform.forward : Owner.transform.up, m_Magnitude.Value);
            
            // Draw lines to players if they are assigned and within range
            if (m_PlayerOne != null && m_PlayerOne.Value != null && m_PlayerOne.Value.transform != null)
            {
                UnityEditor.Handles.color = IsWithinDistance(m_PlayerOne.Value) ? Color.green : Color.red;
                UnityEditor.Handles.DrawLine(Owner.transform.position, m_PlayerOne.Value.transform.position);
            }
            
            if (m_PlayerTwo != null && m_PlayerTwo.Value != null && m_PlayerTwo.Value.transform != null)
            {
                UnityEditor.Handles.color = IsWithinDistance(m_PlayerTwo.Value) ? Color.green : Color.red;
                UnityEditor.Handles.DrawLine(Owner.transform.position, m_PlayerTwo.Value.transform.position);
            }
            
            UnityEditor.Handles.color = oldColor;
#endif
        }
    }
}
