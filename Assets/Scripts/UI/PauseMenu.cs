using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pause;
    [SerializeField] private PlayerInput pInput;

    private InputAction pauseAction;

    private bool paused = false;

    private void Start()
    {
        pauseAction = pInput.currentActionMap.FindAction("Pause");
        pauseAction.started += PauseAction_started;
        Time.timeScale = 1.0f;
    }

    private void PauseAction_started(InputAction.CallbackContext obj)
    {
        if (GameInformation.IsArcadeBuild)
        {
            QuitGame();
            return;
        }

        if (paused)
        {
            Resume();
        }
        else
        {
            Paused();
        }
    }

    public void Paused()
    {
        pause.SetActive(true);
        paused = true;

        Time.timeScale = 0f;
    }
    
    public void Resume()
    {
        pause.SetActive(false);
        paused = false;

        Time.timeScale = 1.0f;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }
}
