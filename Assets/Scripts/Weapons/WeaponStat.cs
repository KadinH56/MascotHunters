using System.Runtime.CompilerServices;
using UnityEngine;

public class WeaponStat
{
    private string weapon;
    private int level;
    private string nextLevelDescription;

    public string Weapon { get => weapon; set => weapon = value; }
    public int Level { get => level; set => level = value; }
    public string NextLevelDescription { get => nextLevelDescription; set => nextLevelDescription = value; }
}
