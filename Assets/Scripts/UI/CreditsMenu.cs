using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using static UnityEngine.Timeline.DirectorControlPlayable;

public class CreditsMenu : MonoBehaviour
{
    [SerializeField] private GameObject tent;
    [SerializeField] private GameObject credits;
    [SerializeField] private PlayerInput cInput;

    private string targetSceneName = "TitleScreen";

    private InputAction creditsOpenAction;

    private bool creditsOpen = false;

    private void Start()
    {
        creditsOpen = false;
        creditsOpenAction = cInput.currentActionMap.FindAction("Credits");
        Time.timeScale = 1.0f;

        creditsOpenAction.started += CreditsOpenAction_started;
    }

    private void CreditsOpenAction_started(InputAction.CallbackContext obj)
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if(currentScene.name == targetSceneName)
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
            Debug.Log("Credits open!");
        }
        else
        {
            tent.SetActive(true);
            credits.SetActive(false);
            creditsOpen = false;
            Debug.Log("Credits closed!");
        }
    }
}
