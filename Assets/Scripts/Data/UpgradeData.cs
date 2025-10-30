using UnityEngine;
[CreateAssetMenu(fileName = "Upgrade", menuName = "UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public enum UPGRADE_TYPE 
    {
        STAT,
        WEAPON
    };

    [SerializeField] private UPGRADE_TYPE upgrade;

    [SerializeField] private string upgradeName;

    [SerializeField] private string[] descriptions;

    [SerializeField] private Sprite sprite;

    public UPGRADE_TYPE Upgrade { get => upgrade; }
    public string UpgradeName { get => upgradeName; }
    public string[] Descriptions { get => descriptions; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
}
