using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ResetQuit : MonoBehaviour
{
    private PlayerInput pInput;
    private InputAction restart;
    private InputAction quit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pInput = GetComponent<PlayerInput>();
        restart = pInput.currentActionMap.FindAction("Restart");
        quit = pInput.currentActionMap.FindAction("Quit");

        restart.started += Reset_started;
        quit.started += Quit_started;
    }

    private void Quit_started(InputAction.CallbackContext obj)
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    private void Reset_started(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(0);
    }
}
