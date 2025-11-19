using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopBar : MonoBehaviour
{
    [SerializeField] private Image[] fills = new Image[3];
    [SerializeField] private TMP_Text[] waveTexts = new TMP_Text[3];

    [SerializeField] private Color currentWaveColor = Color.gold;

    [SerializeField] private Sprite bradyFill;
    [SerializeField] private Sprite bradyBossFill;
    [SerializeField] private Sprite cynthiaFill;
    [SerializeField] private Sprite cynthiaBossFill;
    [SerializeField] private Sprite karlFill;
    [SerializeField] private Sprite karlBossFill;

    [SerializeField] private TMP_Text enemiesLeftText;

    [SerializeField] private int fillResetTime = 180;

    private int enemiesDead = 0;
    void Start()
    {
        foreach (Image image in fills)
        {
            image.fillAmount = 0;
            image.sprite = bradyFill;
        }

        fills[2].sprite = bradyBossFill;

        for(int i = 0; i < waveTexts.Length; i++)
        {
            waveTexts[i].text = (GameInformation.Wave + i).ToString();
        }
    }

    public void EnemyDied()
    {
        enemiesDead++;
        int waveSelector = (GameInformation.Wave % 3) - 1;
        if(waveSelector == -1)
        {
            waveSelector = 2;
        }

        fills[waveSelector].fillAmount = (float)enemiesDead / GameInformation.TotalEnemies;
        SetEnemiesLeft();
    }

    public void ResetKillCount()
    {
        enemiesDead = 0;
    }

    public void SetEnemiesLeft()
    {
        int enemiesLeft = GameInformation.TotalEnemies - enemiesDead;
        enemiesLeftText.text = ": " + enemiesLeft.ToString();
        
    }

    public void ResetSystem(string boss)
    {
        for (int i = 0; i < waveTexts.Length; i++)
        {
            waveTexts[i].text = (GameInformation.Wave + i).ToString();
        }

        StartCoroutine(AnimatedReset(boss));
    }

    private IEnumerator AnimatedReset(string boss)
    {

        for (int i = fillResetTime; i >= 0; i--)
        {
            foreach(Image image in fills)
            {
                image.fillAmount = (float)i/fillResetTime;
            }
            yield return null;
        }

        yield return null;

        Sprite neccesarySprite = null;
        Sprite neccesaryBossSprite = null;
        switch (boss)
        {
            case "Brady":
                neccesarySprite = bradyFill;
                neccesaryBossSprite = bradyBossFill;
                break;
            case "Cynthia":
                neccesarySprite = cynthiaFill;
                neccesaryBossSprite = cynthiaBossFill;
                break;
            case "Karl":
                neccesarySprite = karlFill;
                neccesaryBossSprite = karlBossFill;
                break;
        }
        foreach (Image image in fills)
        {
            image.sprite = neccesarySprite;
        }
    }
}
