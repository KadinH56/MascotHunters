using UnityEngine;
using System.IO;

public class HighScore
{
    public string[] initials;
    public int[] score;
}

public static class SaveSystem
{
    private static HighScore highScores;

    public static HighScore HighScores { get => highScores; }

    public static int GetNewScorePosition(int score)
    {
        for (int i = 0; i < highScores.score.Length; i++)
        {
            if (highScores.score[i] < score)
            {
                return i;
            }
        }

        return -1;
    }

    public static void SetNewHighScore(int position, int score, string initials)
    {
        //HighScore originalHighScores = highScores.Clone();
        HighScore newHighScores = new HighScore()
        {
            score = new int[10],
            initials = new string[10]
        };

        bool inserted = false;
        for (int i = 0; i < highScores.score.Length; i++)
        {
            int pos = i;
            if (inserted)
            {
                pos--;
            }

            if(i == position)
            {
                newHighScores.score[i] = score;
                newHighScores.initials[i] = initials;
                inserted = true;
                continue;
            }

            newHighScores.score[i] = highScores.score[pos];
            newHighScores.initials[i] = highScores.initials[pos];
            Debug.Log(highScores.score[i]);
            Debug.Log(highScores.initials[i]);
            Debug.Log(pos);
        }

        highScores = newHighScores;
    }
    public static void SaveGame()
    {
        string saveFilePath = Application.persistentDataPath + "/HighScores.json";

        string savedData = JsonUtility.ToJson(highScores);
        File.WriteAllText(saveFilePath, savedData);
    }

    /// <summary>
    /// Loads the game
    /// </summary>
    public static void LoadGame()
    {
        string saveFilePath = Application.persistentDataPath + "/HighScores.json";

        if (File.Exists(saveFilePath))
        {
            string loadedData = File.ReadAllText(saveFilePath);
            highScores = JsonUtility.FromJson<HighScore>(loadedData);

            //data.LoadData();
        }
        else
        {
            HighScore temp = new();

            temp.score = new int[10];
            temp.initials = new string[10];

            for (int i = 0; i < 10; i++)
            {
                temp.score[i] = 0;
                temp.initials[i] = "...";
            }

            highScores = temp;

            SaveGame();
        }

    }
}
