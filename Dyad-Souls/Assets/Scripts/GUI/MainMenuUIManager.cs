using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void Start() => SelectFirstInteractableElement();

    private void OnEnable()
    {
        inputActions.UI.Enable();
    }

    private void OnDisable()
    {
        inputActions.UI.Disable();
    }

    private void OnDestroy()
    {
        inputActions?.Dispose();
    }

    private void SelectFirstInteractableElement()
    {
        if (EventSystem.current != null)
        {
            Selectable firstSelectable = GetComponentInChildren<Selectable>();
            if (firstSelectable != null && firstSelectable.interactable)
                firstSelectable.Select();
        }
    }

    public void StartGame() => SceneManager.LoadScene("CharacterSelection");

    public void QuitGame() => Application.Quit();
}
