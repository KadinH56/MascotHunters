using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] protected int baseDamage = 1;
    [SerializeField] protected PlayerMovement pMovement;

    [SerializeField] protected int weaponLevel = 1;

    protected Stats playerStats;

    public int WeaponLevel { get => weaponLevel; set => weaponLevel = value; }

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
        return damage;
    }

    public virtual void LevelUpWeapon()
    {
        
    }
}
