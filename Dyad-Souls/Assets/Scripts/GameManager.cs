using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Slider playerHealthSlider;

    [SerializeField]
    private Slider bossHealthSlider;

    [SerializeField]
    private GameUIManager gameUIManager;

    void Update()
    {
        if (playerHealthSlider.value <= 0)
        {
            gameUIManager.GameOver();
        }

        if (bossHealthSlider.value <= 0)
        {
            gameUIManager.Victory();
        }
    }
}
