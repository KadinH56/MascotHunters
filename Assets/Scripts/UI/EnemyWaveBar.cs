using UnityEngine;
using UnityEngine.UI;

public class EnemyWaveBar : MonoBehaviour
{
    [SerializeField] private Image image;
    public void ApplyEnemyCount()
    {
        if(GameInformation.TotalEnemies > 0)
        {
            image.fillAmount = 0 + (float)GameInformation.EnemiesRemaining / GameInformation.TotalEnemies;
            //print(GameInformation.EnemiesRemaining.ToString() + " " + GameInformation.TotalEnemies.ToString());
            return;
        }

        image.fillAmount = 1;
    }
}
