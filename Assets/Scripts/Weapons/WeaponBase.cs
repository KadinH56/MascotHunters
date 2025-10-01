using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] protected int baseDamage = 1;
    [SerializeField] protected PlayerMovement pMovement;

    protected Stats playerStats;
    private void Awake()
    {
        playerStats = transform.parent.GetComponent<PlayerStatManager>().PlayerStats;
    }

    /// <summary>
    /// Gets the weapon's damage
    /// </summary>
    /// <returns></returns>
    public int CalculateDamage()
    {
        int damage = baseDamage;
        damage = baseDamage + playerStats.DamageModifierAdditive;

        damage = Mathf.RoundToInt(damage * playerStats.DamageModifierMultiplicitive);
        return damage; //Mathf.RoundToInt((baseDamage + playerStats.DamageModifierAdditive) * playerStats.DamageModifierMultiplicitive);
    }
}
