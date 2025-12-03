using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectionUIManager : MonoBehaviour
{
    [Header("Ready Texts")]
    [SerializeField]
    private GameObject gamepadReadyText;

    [SerializeField]
    private GameObject keyboardReadyText;

    [Header("Player Selection")]
    [SerializeField]
    private RectTransform gamepadIcon;

    [SerializeField]
    private RectTransform keyboardIcon;

    [Header("Selection Settings")]
    [SerializeField]
    private float positionOffset;

    [SerializeField]
    private float moveSpeed;

    private Vector3 gamepadStartPosition;
    private Vector3 keyboardStartPosition;
    private int gamepadIconPosition = 0;
    private int keyboardIconPosition = 0;
    private bool gamepadReady = false;
    private bool keyboardReady = false;
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        ResetLobbyState();
    }

    private void Start()
    {
        SelectFirstInteractableElement();
        if (gamepadIcon)
            gamepadStartPosition = gamepadIcon.localPosition;
        if (keyboardIcon)
            keyboardStartPosition = keyboardIcon.localPosition;
    }

    private void ResetLobbyState()
    {
        gamepadIconPosition = 0;
        keyboardIconPosition = 0;
        gamepadReady = false;
        keyboardReady = false;

        if (gamepadReadyText != null)
            gamepadReadyText.SetActive(false);
        if (keyboardReadyText != null)
            keyboardReadyText.SetActive(false);
    }

    private void Update()
    {
        MoveIconsToTargetPositions();
        Ready();
    }

    private void OnEnable()
    {
        inputActions.UI.Cancel.performed += OnCancelPerformed;
        inputActions.UI.Navigate.performed += OnNavigate;
        inputActions.UI.Submit.performed += OnSubmitPerformed;
        inputActions.UI.Enable();
    }

    private void OnDisable()
    {
        inputActions.UI.Cancel.performed -= OnCancelPerformed;
        inputActions.UI.Navigate.performed -= OnNavigate;
        inputActions.UI.Submit.performed -= OnSubmitPerformed;
        inputActions.UI.Disable();
    }

    private void OnDestroy() => inputActions?.Dispose();

    private void OnCancelPerformed(InputAction.CallbackContext context) => BackToMainMenu();

    private void OnSubmitPerformed(InputAction.CallbackContext context)
    {
        string deviceName = context.control.device.name.ToLower();

        if (IsGamepad(deviceName))
        {
            if (gamepadIconPosition != 0)
            {
                gamepadReady = !gamepadReady;
                if (gamepadReadyText)
                    gamepadReadyText.SetActive(gamepadReady);
            }
        }
        else if (deviceName.Contains("keyboard"))
        {
            if (keyboardIconPosition != 0)
            {
                keyboardReady = !keyboardReady;
                if (keyboardReadyText)
                    keyboardReadyText.SetActive(keyboardReady);
            }
        }
    }

    private void OnNavigate(InputAction.CallbackContext context)
    {
        Vector2 navigation = context.ReadValue<Vector2>();
        if (Mathf.Abs(navigation.x) < 0.5f)
            return;

        int direction = navigation.x > 0 ? 1 : -1;
        string deviceName = context.control.device.name.ToLower();

        if (IsGamepad(deviceName))
        {
            MoveIcon(ref gamepadIconPosition, direction);
            if (gamepadReady)
            {
                gamepadReady = false;
                if (gamepadReadyText)
                    gamepadReadyText.SetActive(false);
            }
        }
        else if (deviceName.Contains("keyboard"))
        {
            MoveIcon(ref keyboardIconPosition, direction);
            if (keyboardReady)
            {
                keyboardReady = false;
                if (keyboardReadyText)
                    keyboardReadyText.SetActive(false);
            }
        }
    }

    private bool IsGamepad(string deviceName) =>
        deviceName.Contains("gamepad")
        || deviceName.Contains("controller")
        || deviceName.Contains("dualshock")
        || deviceName.Contains("xbox");

    private void MoveIcon(ref int position, int direction) =>
        position = Mathf.Clamp(position + direction, -1, 1);

    private void MoveIconsToTargetPositions()
    {
        UpdateIconPosition(gamepadIcon, gamepadStartPosition, gamepadIconPosition);
        UpdateIconPosition(keyboardIcon, keyboardStartPosition, keyboardIconPosition);
    }

    private void UpdateIconPosition(RectTransform icon, Vector3 startPos, int posIndex)
    {
        if (!icon)
            return;

        float xOffset = posIndex * positionOffset;
        Vector3 target = startPos + new Vector3(xOffset, 0f, 0f);
        icon.localPosition = Vector3.Lerp(icon.localPosition, target, Time.deltaTime * moveSpeed);
    }

    private void SelectFirstInteractableElement()
    {
        if (EventSystem.current == null)
            return;

        Selectable firstSelectable = GetComponentInChildren<Selectable>();
        if (firstSelectable?.interactable == true)
            firstSelectable.Select();
    }

    private void Ready()
    {
        if (gamepadReady && keyboardReady && gamepadIconPosition != keyboardIconPosition)
        {
            SavePlayerConfiguration();
            SceneManager.LoadScene("Arena");
        }
    }

    private void SavePlayerConfiguration()
    {
        PlayerPrefs.DeleteKey("GamepadControls");
        PlayerPrefs.DeleteKey("KeyboardControls");

        if (gamepadIconPosition == -1)
            PlayerPrefs.SetString("GamepadControls", "Player1");
        else if (gamepadIconPosition == 1)
            PlayerPrefs.SetString("GamepadControls", "Player2");

        if (keyboardIconPosition == -1)
            PlayerPrefs.SetString("KeyboardControls", "Player1");
        else if (keyboardIconPosition == 1)
            PlayerPrefs.SetString("KeyboardControls", "Player2");

        PlayerPrefs.Save();
    }

    public int GetGamepadPlayerPosition() => gamepadIconPosition;

    public int GetKeyboardPlayerPosition() => keyboardIconPosition;

    public bool IsGamepadPlayerOne() => gamepadIconPosition == -1;

    public bool IsKeyboardPlayerOne() => keyboardIconPosition == -1;

    public void BackToMainMenu() => SceneManager.LoadScene("MainMenu");
}
