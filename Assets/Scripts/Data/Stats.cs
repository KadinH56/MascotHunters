using System;
using UnityEngine;

/// <summary>
/// A General data class to hold stats
/// Can hold stats for both players & enemies
/// </summary>
[Serializable]
public class Stats
{
    //Health

    //This should only be changed in editor
    [SerializeField] private int baseMaxHealth;

    //These three should only be changed in code
    //Health here is a weird one because it's a seperate value not attatched to base, but still neccesary
    private int health;

    private int healthModifierAdditive = 0;
    private float healthModifierMultiplicitive = 1.0f;

    //Damage
    //TODO: talk with designers about this. Damage gets a little nuanced and it's not as easy as "plugin this number"

    //Movement
    [SerializeField] private float baseMovement;

    private float movementModifierAdditive = 0f;
    private float movementModifierMultiplicitive = 1.0f;

    public int Health { get => health; set => health = value; }
    public int HealthModifierAdditive { get => healthModifierAdditive; set => healthModifierAdditive = value; }
    public float HealthModifierMultiplicitive { get => healthModifierMultiplicitive; set => healthModifierMultiplicitive = value; }

    /// <summary>
    /// Readonly. To Modify -> Change baseMaxHealth in inspector or use HealthModifierAdditive or HealthModifierMultiplicitive in code.
    /// Do NOT change baseMaxHealth directly
    /// </summary>
    public int MaxHealth { get => Mathf.RoundToInt((baseMaxHealth + HealthModifierAdditive) * HealthModifierMultiplicitive); }


    public float MovementModifierAdditive { get => movementModifierAdditive; set => movementModifierAdditive = value; }
    public float MovementModifierMultiplicitive { get => movementModifierMultiplicitive; set => movementModifierMultiplicitive = value; }
    /// <summary>
    /// Readonly. To Modify -> Change baseMovement in inspector or use MovementModifierAdditive or MovementModifierMultiplicitive in code.
    /// Do NOT change baseMovement directly
    /// </summary>
    public float Movement { get => (baseMovement + MovementModifierAdditive) * MovementModifierMultiplicitive; }
}
