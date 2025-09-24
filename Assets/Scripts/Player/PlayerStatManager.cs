using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatManager : MonoBehaviour
{
    [SerializeField] private Stats playerStats;
    [SerializeField] private Image healthBar;

    /// <summary>
    /// Used to modify player stats or utilize the values
    /// </summary>
    public Stats PlayerStats { get => playerStats; set => playerStats = value; }

    private void Start()
    {
        OnAlive();
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
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float fill = Mathf.Clamp01((float)playerStats.Health / playerStats.MaxHealth);
            healthBar.fillAmount = fill;
        }
    }

    public void OnAlive()
    {
        gameObject.SetActive(true);
        playerStats.Health = playerStats.MaxHealth;
    }
}
