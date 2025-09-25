using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{

    [SerializeField] private GameObject button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void LevelMove()
    {
        if (button.tag == "1Player")
        {
            GameInformation.NumPlayers = 1;
            SceneManager.LoadSceneAsync(1);
        }

        else
        {
            GameInformation.NumPlayers = 2;
            SceneManager.LoadSceneAsync(1);
        }
    }
}
