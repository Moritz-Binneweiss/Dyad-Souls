using UnityEngine;
using UnityEngine.Playables;

/// Triggers a timeline from an animation event.
public class TimelineAnimationEvent : MonoBehaviour
{
    [Header("Timeline Reference")]
    [SerializeField]
    private PlayableDirector slashTimeline;

    /// Call this function as an animation event.
    public void PlaySlashTimeline()
    {
        if (slashTimeline != null)
        {
            slashTimeline.time = 0;
            slashTimeline.Play();
        }
        else
        {
            Debug.LogWarning("Slash Timeline ist nicht zugewiesen!");
        }
    }

    public void StopSlashTimeline()
    {
        if (slashTimeline != null)
        {
            slashTimeline.Stop();
        }
    }
}
