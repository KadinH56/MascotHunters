using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pause;

    public void Paused()
    {
        pause.SetActive(true);

        Time.timeScale = 0f;
    }
    
    public void Resume()
    {
        pause.SetActive(false);

        Time.timeScale = 1.0f;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
