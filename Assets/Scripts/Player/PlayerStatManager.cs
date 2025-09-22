using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    [SerializeField] private Stats playerStats;

    /// <summary>
    /// Used to modify player stats or utilize the values
    /// </summary>
    public Stats PlayerStats { get => playerStats; set => playerStats = value; }
}
