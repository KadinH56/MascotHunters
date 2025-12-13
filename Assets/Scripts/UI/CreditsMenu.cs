using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CreditsMenu : MonoBehaviour
{
    [SerializeField] private GameObject tent;
    [SerializeField] private GameObject credits;
    [SerializeField] private PlayerInput cInput;

    private string targetScene = "TitleScreen";
    private UnityEngine.SceneManagement.Scene currentScene;

    private InputAction creditsOpenAction;

    private bool creditsOpen;

    private void Start()
    {
        cInput.enabled = true;
        creditsOpen = false;
        creditsOpenAction = cInput.currentActionMap.FindAction("Credits");

        creditsOpenAction.started += CreditsOpenAction_started;
    }

    private void CreditsOpenAction_started(InputAction.CallbackContext obj)
    {
        currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == targetScene)
        {
            Credits();
        }   
    }

    public void Credits()
    {
        if (creditsOpen == false)
        {
            tent.SetActive(false);
            credits.SetActive(true);
            creditsOpen = true;
        }
        else
        {
            tent.SetActive(true);
            credits.SetActive(false);
            creditsOpen = false;
        }
    }
}
