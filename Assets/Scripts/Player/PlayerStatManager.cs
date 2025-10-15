using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
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

    [SerializeField] private float percentOfHealthRegainedOnRessurect = 0.5f;
    private string scheme;
    private InputDevice[] device;

    /// <summary>
    /// Used to modify player stats or utilize the values
    /// </summary>
    public Stats PlayerStats { get => playerStats; set => playerStats = value; }
    public Image HealthBar { get => healthBar; set => healthBar = value; }

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

    public void SetControls(string scheme, InputDevice[] device)
    {
        print(scheme);
        print(device);
        this.scheme = scheme;
        this.device = device;
        ApplyControls();
    }

    private void ApplyControls()
    {
        GetComponent<PlayerInput>().SwitchCurrentControlScheme(scheme, device);
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
        ApplyControls();
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
