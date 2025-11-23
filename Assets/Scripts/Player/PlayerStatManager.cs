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
    [SerializeField] private PlayerInput player;
    [SerializeField] private WeaponManager weapon;


    [SerializeField] private float percentOfHealthRegainedOnRessurect = 0.5f;
    private string scheme;
    private InputDevice[] device;

    /// <summary>
    /// Used to modify player stats or utilize the values
    /// </summary>
    public Stats PlayerStats { get => playerStats; set => playerStats = value; }
    public Image HealthBar { get => healthBar; set => healthBar = value; }

    [SerializeField] private Sprite p2hpBar;

    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Animator animator;
    [SerializeField] private bool isDead;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;

    [SerializeField] private GameObject weaponManager;

    private void Start()
    {
        if(GetComponent<PlayerMovement>().PlayerID == 1)
        {
            healthBar.sprite = p2hpBar;
        }
    }

    /// <summary>
    /// Take damage
    /// </summary>
    /// <param name="damage">Amount of damage</param>
    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }
        StartCoroutine(HitFX());
        SFX.SpawnClip(hurtSound, transform.position);

        playerStats.Health -= damage;
        UpdateHealthBar();

        if (playerStats.Health <= 0 && !isDead)
        {
            StartCoroutine(HandleDeath());
            
            //mainMenu.SetActive(true);
        }
    }

    private IEnumerator HandleDeath()
    {
        isDead = true;
        SFX.SpawnClip(deathSound, transform.position);

        animator.SetBool("IsDead", true);
        weaponManager.SetActive(false);
        player.enabled = false;
        weapon.enabled = false;
        yield return new WaitForSeconds(4);
        gameObject.SetActive(false);
    }

    public void Healing(int health)
    {
        playerStats.Health += health;
        if (playerStats.Health > playerStats.MaxHealth)
        {
            playerStats.Health = playerStats.MaxHealth;
        }

        UpdateHealthBar();
    }

    public void SetControls(string scheme, InputDevice[] device)
    {
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
        isDead = false;
        player.enabled = true;
        weapon.enabled = true;
        animator.SetBool("IsDead", false);
        spriteRenderer.material.SetFloat("_HitFlash", 0);

        weaponManager.SetActive(true);

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

    private IEnumerator HitFX()
    {
        spriteRenderer.material.SetFloat("_HitFlash", 1);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material.SetFloat("_HitFlash", 0);
    }

}
