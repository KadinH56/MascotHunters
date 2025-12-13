using UnityEngine;
using UnityEngine.InputSystem;

public class ForceQuit : MonoBehaviour
{
    private static ForceQuit instance;
    [SerializeField] private PlayerInput pInput;

    private InputAction forceQuit;
    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            pInput.currentActionMap.Enable();
            forceQuit = pInput.currentActionMap.FindAction("ForceQuit");
            forceQuit.started += ForceQuit_started;
            return;
        }

        Destroy(gameObject);
    }

    private void ForceQuit_started(InputAction.CallbackContext obj)
    {
        Application.Quit();
    }
}
