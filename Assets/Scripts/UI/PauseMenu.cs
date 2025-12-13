using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pause;
    [SerializeField] private PlayerInput pInput;

    private InputAction pauseAction;
    private InputAction resumeAction;

    private bool paused = false;

    private void Start()
    {
        paused = false;
        pauseAction = pInput.currentActionMap.FindAction("Pause");
        resumeAction = pInput.currentActionMap.FindAction("Credits");
        InputDevice[] devices = new InputDevice[1];
        devices[0] = InputSystem.devices[0];

        pInput.SwitchCurrentControlScheme("UI", devices);
        pauseAction.started += PauseAction_started;
        resumeAction.started += ResumeAction_started;
        Time.timeScale = 1.0f;
    }

    private void OnDestroy()
    {
        pauseAction.started -= PauseAction_started;
        resumeAction.started -= ResumeAction_started;
    }

    private void ResumeAction_started(InputAction.CallbackContext obj)
    {
        if (paused)
        {
            Resume();
        }
    }

    private void PauseAction_started(InputAction.CallbackContext obj)
    {
        if (paused)
        {
            QuitGame();
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
        pauseAction.started -= PauseAction_started;
        resumeAction.started -= ResumeAction_started;
        SceneManager.LoadScene(0);
    }
}
