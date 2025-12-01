using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ResetQuit : MonoBehaviour
{
    private PlayerInput pInput;

    private InputAction start1P;
    private InputAction start2P;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        pInput = GetComponent<PlayerInput>();

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

    private void OnDestroy()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            start1P.started -= Start1P_started;
            start2P.started -= Start2P_started;
        }
    }

    [ContextMenu("Increase Time")]
    public void IncreaseTime()
    {
        Time.timeScale *= 1.5f;
    }
}
