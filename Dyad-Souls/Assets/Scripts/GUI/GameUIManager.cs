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
    private GameObject victoryUI;

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
        // Disable UI actions immediately to prevent blocking player input
        isPaused = false;
        inputActions.UI.Disable();
    }

    private void Start()
    {
        // Enable only Pause action for pause functionality
        // Do NOT enable entire Player action map
        inputActions.Player.Pause.Enable();
    }

    private void OnEnable()
    {
        // Subscribe to pause and cancel actions
        inputActions.Player.Pause.performed += OnPausePerformed;
        inputActions.UI.Cancel.performed += OnCancelPerformed;
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
        SelectFirstInteractableElement(pauseUI);
    }

    private void SelectFirstInteractableElement(GameObject uiPanel)
    {
        if (uiPanel != null && EventSystem.current != null)
        {
            Selectable firstSelectable = uiPanel.GetComponentInChildren<Selectable>();
            if (firstSelectable != null && firstSelectable.interactable)
            {
                firstSelectable.Select();
                EventSystem.current.SetSelectedGameObject(firstSelectable.gameObject);
            }
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
        SelectFirstInteractableElement(gameOverUI);
    }

    public void RestartGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        // Cleanup input actions before scene load
        if (inputActions != null)
        {
            inputActions.Player.Pause.performed -= OnPausePerformed;
            inputActions.UI.Cancel.performed -= OnCancelPerformed;
            inputActions.Disable();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        // Cleanup input actions before scene load
        if (inputActions != null)
        {
            inputActions.Player.Pause.performed -= OnPausePerformed;
            inputActions.UI.Cancel.performed -= OnCancelPerformed;
            inputActions.Disable();
        }

        SceneManager.LoadScene("MainMenu");
    }

    public void ShowVictory()
    {
        if (victoryUI != null)
        {
            this.victoryUI.SetActive(true);
            VictoryUI victoryUI = this.victoryUI.GetComponent<VictoryUI>();
            if (victoryUI != null)
            {
                StartCoroutine(victoryUI.ShowVictorySequence());
            }
        }
    }

    public Slider GetPlayerOneStaminaSlider() => playerOneStaminaSlider;

    public Slider GetPlayerTwoStaminaSlider() => playerTwoStaminaSlider;
}
