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

    private void Start()
    {
        // Automatische Selektion für Controller/Tastatur-Navigation beim Start
        SelectFirstInteractableElement();
    }

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

    //TODO: UI Interaktion mit Controller und Tastatur geht hier  und wo anders noch nicht von Anfang an, erst nach neuem loaden der Szene
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

    public void StartGame()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
