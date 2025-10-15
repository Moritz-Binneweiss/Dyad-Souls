using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void Start()
    {
        // Automatische Selektion für Controller/Tastatur-Navigation beim Start
        SelectFirstInteractableElement();
    }

    private void OnEnable()
    {
        inputActions.UI.Cancel.performed += OnCancelPerformed;
        inputActions.UI.Enable();
    }

    private void OnDisable()
    {
        inputActions.UI.Cancel.performed -= OnCancelPerformed;
        inputActions.UI.Disable();
    }

    private void OnDestroy()
    {
        inputActions?.Dispose();
    }

    private void OnCancelPerformed(InputAction.CallbackContext context)
    {
        BackToMainMenu();
    }

    private void SelectFirstInteractableElement()
    {
        if (EventSystem.current != null)
        {
            // Finde das erste interaktive UI-Element (Button, Slider, etc.)
            Selectable firstSelectable = GetComponentInChildren<Selectable>();
            if (firstSelectable != null && firstSelectable.interactable)
            {
                firstSelectable.Select();
            }
        }
    }

    public void Ready()
    {
        SceneManager.LoadScene("Arena");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
