using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private string[] weapons = new string[2];
    [SerializeField] private WeaponBase[] weaponScripts;

    public string[] Weapons { get => weapons; set => weapons = value; }
    public WeaponBase[] WeaponScripts { get => weaponScripts; set => weaponScripts = value; }

    private void Start()
    {
        weapons[0] = "Bat";
    }

    public void WeaponUpgrade(string newWeapon)
    {
        if (weapons[0] == newWeapon || weapons[1] == newWeapon)
        {
            transform.Find(newWeapon).GetComponent<WeaponBase>().LevelUpWeapon();
            return;
        }

        if (weapons[0] == null)
        {
            weapons[0] = newWeapon;
            transform.Find(newWeapon).gameObject.SetActive(true);
            return;
        }

        if (weapons[1] == null)
        {
            weapons[1] = newWeapon;
            transform.Find(newWeapon).gameObject.SetActive(true);
            return;
        }
    }

    //[ContextMenu("Gimme a hammah")]
    //public void WeaponUpgrade2()
    //{
    //    WeaponUpgrade("Hammer");
    //}

    public WeaponStat[] GetWeapons()
    {
        WeaponStat[] stats = new WeaponStat[2];

        for(int i = 0; i < weapons.Length; i++)
        {
            if(weapons[i] == null)
            {
                stats[i] = null;
                continue;
            }

            WeaponStat stat = new()
            {
                Weapon = weapons[i],
                Level = transform.Find(weapons[i]).GetComponent<WeaponBase>().WeaponLevel
            };

            if (transform.Find(weapons[i]).GetComponent<WeaponBase>().LevelupDescriptions.Count > 0)
            {
                stat.NextLevelDescription = transform.Find(weapons[i]).GetComponent<WeaponBase>().LevelupDescriptions[stat.Level];
            }
            else
            {
                stat.NextLevelDescription = "Get a " + stat.Weapon;
            }

            stats[i] = stat;
        }

        return stats;
    }

    public int GetLevelNext(string weapon)
    {
        if(transform.Find(weapon) != null && transform.Find(weapon).gameObject.activeSelf)
        {
            return transform.Find(weapon).GetComponent<WeaponBase>().WeaponLevel;
        }

        return 0;
    }
}
