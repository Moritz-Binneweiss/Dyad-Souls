using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PhaseTransition : MonoBehaviour
{
    [Header("Video Settings")]
    [SerializeField]
    private VideoPlayer videoPlayer;

    [SerializeField]
    private VideoClip phaseTransitionVideo;

    [Header("Fade Settings")]
    [SerializeField]
    private Image fadeImage;

    [SerializeField]
    private float fadeDuration = 1f;

    [SerializeField]
    private float pauseDurationBeforeFade = 0.3f;

    [Header("Render Texture Settings")]
    [SerializeField]
    private RenderTexture videoRenderTexture;

    [SerializeField]
    private RawImage videoDisplay;

    private bool isPlaying = false;

    private void Awake()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        // Setup video player for fullscreen display via RenderTexture
        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false;
            videoPlayer.isLooping = false;
            videoPlayer.renderMode = VideoRenderMode.RenderTexture;
            videoPlayer.targetTexture = videoRenderTexture;
        }

        // Setup video display
        if (videoDisplay != null)
        {
            videoDisplay.texture = videoRenderTexture;
            videoDisplay.gameObject.SetActive(false);
        }

        // Make sure fade image starts transparent
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
            fadeImage.gameObject.SetActive(false);
        }
    }

    public void PlayPhaseTransition(System.Action onComplete = null)
    {
        if (isPlaying)
            return;

        StartCoroutine(PhaseTransitionSequence(onComplete));
    }

    private IEnumerator PhaseTransitionSequence(System.Action onComplete)
    {
        isPlaying = true;

        // 1. Immediately pause time (before any delays)
        Time.timeScale = 0f;

        // 2. Briefly freeze moment
        yield return new WaitForSecondsRealtime(pauseDurationBeforeFade);

        // 3. Fade to black
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            yield return StartCoroutine(FadeToBlack());
        }

        // 4. Prepare video while screen is black
        if (videoPlayer != null && phaseTransitionVideo != null)
        {
            videoPlayer.clip = phaseTransitionVideo;
            videoPlayer.Prepare();

            // Wait for video to be prepared
            while (!videoPlayer.isPrepared)
            {
                yield return null;
            }
        }

        // 5. Start playing video before showing display (prevents preview frame)
        if (videoPlayer != null)
        {
            videoPlayer.Play();
            // Wait a frame for video to actually start
            yield return null;
        }

        // 6. Now show video display and fade from black
        if (videoDisplay != null)
        {
            videoDisplay.gameObject.SetActive(true);
        }

        if (fadeImage != null)
        {
            yield return StartCoroutine(FadeFromBlack());
        }

        // 7. Wait for video to finish
        if (videoPlayer != null)
        {
            // Wait for video to finish
            while (videoPlayer.isPlaying)
            {
                yield return null;
            }

            // Extra safety: wait a frame after video ends
            yield return null;
        }

        // 8. Smoothly fade to black after video
        if (fadeImage != null)
        {
            yield return StartCoroutine(FadeToBlack());
        }

        if (videoDisplay != null)
        {
            videoDisplay.gameObject.SetActive(false);
        }

        // 9. Fade back to gameplay (reveal Phase 2)
        if (fadeImage != null)
        {
            yield return StartCoroutine(FadeFromBlack());
            fadeImage.gameObject.SetActive(false);
        }

        // 10. Resume time
        Time.timeScale = 1f;

        isPlaying = false;

        // 11. Callback for game manager to continue phase transition
        onComplete?.Invoke();
    }

    private IEnumerator FadeToBlack()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime; // Use unscaled time since timeScale is 0
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);

            Color c = fadeImage.color;
            c.a = alpha;
            fadeImage.color = c;

            yield return null;
        }

        // Ensure fully black
        Color finalColor = fadeImage.color;
        finalColor.a = 1f;
        fadeImage.color = finalColor;
    }

    private IEnumerator FadeFromBlack()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);

            Color c = fadeImage.color;
            c.a = alpha;
            fadeImage.color = c;

            yield return null;
        }

        // Ensure fully transparent
        Color finalColor = fadeImage.color;
        finalColor.a = 0f;
        fadeImage.color = finalColor;
    }

    public bool IsPlaying() => isPlaying;
}
