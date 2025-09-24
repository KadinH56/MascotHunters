using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    [SerializeField] private Stats playerStats;

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

        if(playerStats.Health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnAlive()
    {
        gameObject.SetActive(true);
        playerStats.Health = playerStats.MaxHealth;
    }
}
