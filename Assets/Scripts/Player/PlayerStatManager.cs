using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatManager : MonoBehaviour
{
    public enum PLAYER_STATS
    {
        HEALTH,
        DAMAGE,
        MOVEMENT
    }

    [SerializeField] private Stats playerStats;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject mainMenu;

    [SerializeField, MinMaxRangeSlider(0f, 1f)] private float percentOfHealthRegainedOnRessurect = 0.5f;

    /// <summary>
    /// Used to modify player stats or utilize the values
    /// </summary>
    public Stats PlayerStats { get => playerStats; set => playerStats = value; }
    public Image HealthBar { get => healthBar; set => healthBar = value; }

    private void Start()
    {
        //OnAlive();
    }
    /// <summary>
    /// Take damage
    /// </summary>
    /// <param name="damage">Amount of damage</param>
    public void TakeDamage(int damage)
    {
        playerStats.Health -= damage;
        UpdateHealthBar();

        if (playerStats.Health <= 0)
        {
            gameObject.SetActive(false);
            //mainMenu.SetActive(true);
        }
    }

    public void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float fill = Mathf.Clamp01((float)playerStats.Health / playerStats.MaxHealth);
            healthBar.fillAmount = fill;
        }
    }

    public void OnAlive(bool fullRevive = true)
    {
        gameObject.SetActive(true);
        if (fullRevive)
        {
            playerStats.Health = playerStats.MaxHealth;
        } 
        else
        {
            playerStats.Health = Mathf.RoundToInt(playerStats.MaxHealth * percentOfHealthRegainedOnRessurect);
        }


            UpdateHealthBar();
    }

    public void TempPowerupIEnumerator(PLAYER_STATS statBuff, float duration, float level)
    {
        switch (statBuff)
        {
            case (PLAYER_STATS.MOVEMENT):
                StartCoroutine(TempSpeedBuff(duration, level));
                break;
        }
    }

    private IEnumerator TempSpeedBuff(float duration, float level)
    {
        PlayerStats.TempMoveModifierMultiplicative *= level;
        yield return new WaitForSeconds(duration);
        PlayerStats.TempMoveModifierMultiplicative /= level;
    }
}
