using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameUI;

    [SerializeField]
    private GameObject pauseUI;

    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private TextMeshProUGUI waveCountdownText;

    [SerializeField]
    private GameObject enemyDefeatedText;

    [Header("Player Stamina UI References")]
    [SerializeField]
    private Slider playerOneStaminaSlider;

    [SerializeField]
    private Slider playerTwoStaminaSlider;

    private InputSystem_Actions inputActions;
    private bool isPaused;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Pause.performed += OnPausePerformed;
        inputActions.UI.Cancel.performed += OnCancelPerformed;
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Pause.performed -= OnPausePerformed;
        inputActions.UI.Cancel.performed -= OnCancelPerformed;
        inputActions.Player.Disable();
        inputActions.UI.Disable();
    }

    private void OnDestroy()
    {
        inputActions?.Dispose();
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        if (isPaused)
            ContinueGame();
        else
            PauseGame();
    }

    private void OnCancelPerformed(InputAction.CallbackContext context)
    {
        if (isPaused)
            ContinueGame();
    }

    public void PauseGame()
    {
        gameUI.SetActive(false);
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        inputActions.UI.Enable();
        SelectFirstInteractableElement();
    }

    private void SelectFirstInteractableElement()
    {
        if (pauseUI != null && EventSystem.current != null)
        {
            Selectable firstSelectable = pauseUI.GetComponentInChildren<Selectable>();
            if (firstSelectable != null && firstSelectable.interactable)
                firstSelectable.Select();
        }
    }

    public void ContinueGame()
    {
        pauseUI.SetActive(false);
        gameUI.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
        inputActions.UI.Disable();
    }

    public void GameOver()
    {
        gameUI.SetActive(false);
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        inputActions.UI.Enable();
        SelectFirstInteractableElement();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowWaveCountdown(int nextWave, int seconds)
    {
        if (waveCountdownText != null)
        {
            waveCountdownText.text = $"Wave {nextWave} starting in {seconds}...";
            waveCountdownText.gameObject.SetActive(true);
        }
    }

    public void HideWaveCountdown()
    {
        if (waveCountdownText != null)
        {
            waveCountdownText.gameObject.SetActive(false);
        }
    }

    public void ShowEnemyDefeated()
    {
        if (enemyDefeatedText != null)
        {
            enemyDefeatedText.SetActive(true);
        }
    }

    public void HideEnemyDefeated()
    {
        if (enemyDefeatedText != null)
            enemyDefeatedText.SetActive(false);
    }

    public Slider GetPlayerOneStaminaSlider() => playerOneStaminaSlider;

    public Slider GetPlayerTwoStaminaSlider() => playerTwoStaminaSlider;
}
