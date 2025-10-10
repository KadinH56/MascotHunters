using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private string[] weapons = new string[2];

    public string[] Weapons { get => weapons; set => weapons = value; }

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

            WeaponStat stat = new();
            stat.Weapon = weapons[i];
            stat.Level = transform.Find(weapons[i]).GetComponent<WeaponBase>().WeaponLevel;

            if (transform.Find(weapons[i]).GetComponent<WeaponBase>().LevelupDescriptions.Count > 0)
            {
                stat.NextLevelDescription = transform.Find(weapons[i]).GetComponent<WeaponBase>().LevelupDescriptions[stat.Level];
            }
            else
            {
                stat.NextLevelDescription = "Get a " + stat.Weapon;
            }
        }

        return stats;
    }
}
