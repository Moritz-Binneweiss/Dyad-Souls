using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject mainMenu;

    public void StartGame()
    {
        SceneManager.LoadScene("PrepLobby");
    }
}
