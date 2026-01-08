using System.Collections;
using TMPro;
using UnityEngine;

public class VictoryUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI victoryText;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private float fadeInDuration = 2f;

    [SerializeField]
    private float displayDuration = 3f;

    [SerializeField]
    private float fadeOutDuration = 2f;

    private void Start()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }

    public IEnumerator ShowVictorySequence()
    {
        // Fade in
        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            if (canvasGroup != null)
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
            yield return null;
        }

        if (canvasGroup != null)
            canvasGroup.alpha = 1f;

        // Display
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            if (canvasGroup != null)
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeOutDuration);
            yield return null;
        }

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        gameObject.SetActive(false);
    }
}
