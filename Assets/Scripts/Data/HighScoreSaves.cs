using UnityEngine;
using System.IO;

public class HighScore
{
    public string initials;
    public int score;
}

public static class SaveSystem
{
    private static HighScore[] highScores = new HighScore[10];

    public static int GetNewScorePosition(int score)
    {
        for (int i = 0; i < highScores.Length; i++)
        {
            if (highScores[i].score == score)
            {
                return i;
            }
        }

        return -1;
    }

    public static void SetNewHighScore(int position, int score, string initials)
    {
        HighScore newHighScore = new()
        {
            initials = initials,
            score = score
        };

        HighScore[] originalHighScores = (HighScore[])highScores.Clone();

        bool inserted = false;
        for (int i = 0; i < originalHighScores.Length; i++)
        {
            int pos = i;
            if (inserted)
            {
                pos--;
            }

            if(i == position)
            {
                highScores[i] = newHighScore;
                inserted = true;
                continue;
            }

            highScores[i] = originalHighScores[pos];
        }
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
            highScores = JsonUtility.FromJson<HighScore[]>(loadedData);

            //data.LoadData();
        }
    }
}
