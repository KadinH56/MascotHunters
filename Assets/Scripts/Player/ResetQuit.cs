using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ResetQuit : MonoBehaviour
{
    private PlayerInput pInput;
    private InputAction restart;
    private InputAction pause;

    private InputAction start1P;
    private InputAction start2P;

    private PauseMenu pm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pInput = GetComponent<PlayerInput>();
        restart = pInput.currentActionMap.FindAction("Restart");
        pause = pInput.currentActionMap.FindAction("Pause");
        pm = GetComponent<PauseMenu>();

        restart.started += Reset_started;
        pause.started += Pause_started;

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            start1P = pInput.currentActionMap.FindAction("Start Player 1");
            start2P = pInput.currentActionMap.FindAction("Start Player 2");

            start1P.started += Start1P_started;
            start2P.started += Start2P_started;
        }
    }

    private void Pause_started(InputAction.CallbackContext obj)
    {
        pm.Paused();
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

    private void Reset_started(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(0);
    }

    private void OnDestroy()
    {
        restart.started -= Reset_started;

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            start1P.started -= Start1P_started;
            start2P.started -= Start2P_started;
        }
    }
}
