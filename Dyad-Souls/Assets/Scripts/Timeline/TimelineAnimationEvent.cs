using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Nutze dieses Skript um die Timeline von einem Animation Event aufzurufen.
/// 1. Füge dieses Skript zum Player hinzu
/// 2. Weise die Timeline zu
/// 3. Erstelle ein Animation Event in deiner Attack Animation
/// 4. Rufe PlaySlashTimeline() im Event auf
/// </summary>
public class TimelineAnimationEvent : MonoBehaviour
{
    [Header("Timeline Reference")]
    [SerializeField]
    private PlayableDirector slashTimeline;

    /// <summary>
    /// Rufe diese Funktion als Animation Event auf
    /// </summary>
    public void PlaySlashTimeline()
    {
        if (slashTimeline != null)
        {
            slashTimeline.time = 0; // Von vorne starten
            slashTimeline.Play();
        }
        else
        {
            Debug.LogWarning("Slash Timeline ist nicht zugewiesen!");
        }
    }

    /// <summary>
    /// Optional: Timeline stoppen
    /// </summary>
    public void StopSlashTimeline()
    {
        if (slashTimeline != null)
        {
            slashTimeline.Stop();
        }
    }
}
