using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScoreController : MonoBehaviour
{
    private readonly char[] ROTATING_CHARACTERS = new char[] 
    { 
        '.',
        'A',
        'B',
        'C',
        'D',
        'E',
        'F',
        'G',
        'H',
        'I',
        'J',
        'K',
        'L',
        'M',
        'N',
        'O',
        'P',
        'Q',
        'R',
        'S',
        'T',
        'U',
        'V',
        'W',
        'X',
        'Y',
        'Z',
    };

    private int verticalPosition = 0;
    private int initialNumber = 0;

    [SerializeField] private float waitTime = 3f;
    private void Start()
    {
        SaveSystem.LoadGame();

        if (SaveSystem.GetNewScorePosition(GameInformation.Wave) != -1)
        {

            return;
        }

        StartCoroutine(MoveToNextScreen());
    }

    private IEnumerator MoveToNextScreen()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(0);
    }
}
