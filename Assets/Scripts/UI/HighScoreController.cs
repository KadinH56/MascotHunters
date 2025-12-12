using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HighScoreController : MonoBehaviour
{
    private readonly char[] ROTATING_CHARACTERS = new char[] 
    { 
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
    private int horizontalPosition = 0;

    private bool settingNewScore = false;
    private bool newHighScore = false;

    [SerializeField] private float waitTime = 3f;
    [SerializeField] private float appearTime = 0.5f;

    [SerializeField] private GameObject wordHolder;
    [SerializeField] private TMP_Text selectText;
    [SerializeField] private float waitTimeAfterSelecting = 2f;

    [SerializeField] private int framesBeforeColorChange = 60;
    [SerializeField] private Color colorChange = Color.yellow;

    private Color textColor = Color.white;

    private InputAction select;
    private InputAction move;

    private string initials = "";

    private void Start()
    {
        GameInformation.Wave = 1000;
        wordHolder.SetActive(false);
        SaveSystem.LoadGame();
        StartCoroutine(ShowScores());

        GetComponent<PlayerInput>().currentActionMap.Enable();

        if (GameInformation.IsArcadeBuild)
        {
            InputDevice[] devices =
            {
                InputSystem.GetDevice<Keyboard>()
            };
            GetComponent<PlayerInput>().SwitchCurrentControlScheme("ArcadeA", devices);
            selectText.text = "Roll\r\nto Select";
        }

        select = GetComponent<PlayerInput>().currentActionMap.FindAction("Select High Score");
        move = GetComponent<PlayerInput>().currentActionMap.FindAction("Move");

        select.started += Select_started;
        move.started += Move_started;
    }

    private void OnDestroy()
    {
        select.started -= Select_started;
        move.started -= Move_started;
    }

    private void Move_started(InputAction.CallbackContext obj)
    {
        Vector2 moveValue = obj.ReadValue<Vector2>();

        if(moveValue.x != 0)
        {
            horizontalPosition += Mathf.RoundToInt(moveValue.x);
            horizontalPosition = Mathf.Clamp(horizontalPosition, 0, 2);
        }

        if (moveValue.y != 0)
        {
            int index = Array.IndexOf(ROTATING_CHARACTERS, initials[horizontalPosition]);
            index -= Mathf.RoundToInt(moveValue.y);
            if (index < 0)
            {
                index = ROTATING_CHARACTERS.Length - 1;
            }

            if (index >= ROTATING_CHARACTERS.Length)
            {
                index = 0;
            }

            switch (horizontalPosition)
            {
                case 0:
                    initials = ROTATING_CHARACTERS[index].ToString() + initials[1].ToString() + initials[2].ToString();
                    break;
                case 1:
                    initials = initials[0].ToString() + ROTATING_CHARACTERS[index].ToString() + initials[2].ToString();
                    break;
                case 2:
                    initials = initials[0].ToString() + initials[1].ToString() + ROTATING_CHARACTERS[index].ToString();
                    break;
            }
        }
    }

    private void Select_started(InputAction.CallbackContext obj)
    {
        if(horizontalPosition == 2)
        {
            settingNewScore = false;
        }
        else
        {
            horizontalPosition++;
        }
    }

    private void HandleGameStuff()
    {
        verticalPosition = SaveSystem.GetNewScorePosition(GameInformation.Wave);
        if (verticalPosition != -1)
        {
            newHighScore = true;
            wordHolder.SetActive(true);
            StartCoroutine(EnterInitials());
            return;
        }

        StartCoroutine(MoveToNextScreen());
    }

    private IEnumerator MoveToNextScreen()
    {
        yield return new WaitForSeconds(newHighScore ? waitTimeAfterSelecting : waitTime);
        SceneManager.LoadScene(0);
    }

    private IEnumerator ColorChangingText()
    {
        int colorChangeFrames = framesBeforeColorChange;
        while (settingNewScore)
        {
            colorChangeFrames--;
            if(colorChangeFrames == 0)
            {
                textColor = textColor == Color.white ? colorChange : Color.white;
                colorChangeFrames = framesBeforeColorChange;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator EnterInitials()
    {
        settingNewScore = true;
        yield return null;

        bool inserted = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            if(i == verticalPosition)
            {
                inserted = true;
                continue;
            }

            if (inserted)
            {
                transform.GetChild(i).GetComponent<TMP_Text>().text = SaveSystem.HighScores.initials[i - 1].ToString() + " : " + SaveSystem.HighScores.score[i - 1].ToString();
            }
            
        }

        initials = "AAA";
        TMP_Text gameText = transform.GetChild(verticalPosition).GetComponent<TMP_Text>();
        string endText = " : " + GameInformation.Wave.ToString();
        StartCoroutine(ColorChangingText());
        while (settingNewScore)
        {

            string initialsText;
            switch (horizontalPosition)
            {
                case 0:
                    initialsText = "<U><color=#" + ColorUtility.ToHtmlStringRGB(textColor) + ">" + initials[0].ToString() + "</U></Color>" + initials[1].ToString() + initials[2].ToString();

                    gameText.text = initialsText + endText;
                    break;
                case 1:
                    initialsText = initials[0].ToString() + "<U><color=#" + ColorUtility.ToHtmlStringRGB(textColor) + ">" + initials[1].ToString() + "</U></Color>" + initials[2].ToString();

                    gameText.text = initialsText + endText;
                    break;
                case 2:
                    initialsText = initials[0].ToString() + initials[1].ToString() + "<U><color=#" + ColorUtility.ToHtmlStringRGB(textColor) + ">" + initials[2].ToString() + "</U></Color>";

                    gameText.text = initialsText + endText;
                    break;
            }
            yield return null;
        }

        gameText.text = initials + endText;

        SaveSystem.SetNewHighScore(verticalPosition, GameInformation.Wave, initials);
        SaveSystem.SaveGame();

        StartCoroutine(MoveToNextScreen());
    }

    private IEnumerator ShowScores()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<TMP_Text>().text = SaveSystem.HighScores.initials[i].ToString() + " : " + SaveSystem.HighScores.score[i].ToString();

            transform.GetChild(i).gameObject.SetActive(true);
            yield return new WaitForSeconds(appearTime);
        }

        HandleGameStuff();
    }
}
