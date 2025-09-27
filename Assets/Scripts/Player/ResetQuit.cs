using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ResetQuit : MonoBehaviour
{
    private PlayerInput pInput;
    private InputAction restart;
    private InputAction quit;

    private InputAction start1P;
    private InputAction start2P;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pInput = GetComponent<PlayerInput>();
        restart = pInput.currentActionMap.FindAction("Restart");
        quit = pInput.currentActionMap.FindAction("Quit");

        restart.started += Reset_started;
        quit.started += Quit_started;

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            start1P = pInput.currentActionMap.FindAction("Start Player 1");
            start2P = pInput.currentActionMap.FindAction("Start Player 2");

            start1P.started += Start1P_started;
            start2P.started += Start2P_started;
        }
    }

    private void Start2P_started(InputAction.CallbackContext obj)
    {
        GameInformation.NumPlayers = 2;
        SceneManager.LoadSceneAsync(1);
    }

    private void Start1P_started(InputAction.CallbackContext obj)
    {
        GameInformation.NumPlayers = 1;
        SceneManager.LoadSceneAsync(1);
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

    private void OnDestroy()
    {
        quit.started -= Quit_started;
        restart.started -= Reset_started;

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            start1P.started -= Start1P_started;
            start2P.started -= Start2P_started;
        }
    }
}
