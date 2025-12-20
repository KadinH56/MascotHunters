using UnityEngine;
using UnityEngine.InputSystem;

public class ForceQuit : MonoBehaviour
{
    private static ForceQuit instance;
    [SerializeField] private PlayerInput pInput;

    private InputAction forceQuit;
    void Start()
    {
        if(instance == null && !GameInformation.IsArcadeBuild)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            pInput.currentActionMap.Enable();
            forceQuit = pInput.currentActionMap.FindAction("ForceQuit");
            InputDevice[] devices = new InputDevice[1];
            devices[0] = InputSystem.devices[0];

            pInput.SwitchCurrentControlScheme("ForceQuit", devices);
            forceQuit.started += ForceQuit_started;
            return;
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if(instance == this)
        {
            forceQuit.started -= ForceQuit_started;
        }
    }

    private void ForceQuit_started(InputAction.CallbackContext obj)
    {
        Application.Quit();
    }
}
