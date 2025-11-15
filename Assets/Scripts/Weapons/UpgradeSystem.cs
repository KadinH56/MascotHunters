using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UpgradeSystem : MonoBehaviour
{
    private bool unSelected;
    private PlayerMovement currentPlayer;

    string[] upgrades = new string[3];

    [SerializeField] private GameObject UpgradeMenu;

    [SerializeField] private Image[] upgradeImages;

    [SerializeField] private UpgradeData[] upgradeData;

    [SerializeField] private TMP_Text nameText;

    [SerializeField] private TMP_Text descriptionText;

    private Dictionary<string, UpgradeData> upgradeNames = new();

    private string[] descriptions = new string[3];
    private string[] names = new string[3];

    private int buttonHit = 0;
    private void Start()
    {
        foreach(UpgradeData upgrade in upgradeData)
        {
            upgradeNames.Add(upgrade.UpgradeName, upgrade);
        }
    }

    public void StartUpgrades(bool firstUpgrade = false)
    {
        StartCoroutine(Upgrade(firstUpgrade));
    }
    public IEnumerator Upgrade(bool firstUpgrade)
    {
        yield return null;
        PlayerMovement[] players = new PlayerMovement[2];
        foreach (PlayerMovement player in FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None))
        {
            players[player.PlayerID] = player; //Thats enough player for one line
        }
        UpgradeMenu.SetActive(true);

        Time.timeScale = 0.0f;
        for (int i = 0; i < GameInformation.NumPlayers; i++)
        {
            buttonHit = -1;
            currentPlayer = players[i];

            descriptionText.gameObject.SetActive(false);
            nameText.gameObject.SetActive(false);

            upgrades = new string[3];
            //if (firstUpgrade)
            //{
            //    upgrades = new[]
            //    {
            //        "Ladder",
            //        "CorkGun",
            //        "Hammer"
            //    };

            //    for (int j = 0; j < upgrades.Length; j++)
            //    {
            //        switch (upgrades[j])
            //        {
            //            case "Ladder":
            //                upgradeImages[j].sprite = upgradeNames["Spinning Ladder"].Sprite;
            //                break;
            //            case "Hammer":
            //                upgradeImages[j].sprite = upgradeNames["Hammer"].Sprite;
            //                break;
            //            case "CorkGun":
            //                upgradeImages[j].sprite = upgradeNames["Cork Gun"].Sprite;
            //                break;
            //        }
            //    }
            //}
            //else
            //{
                List<string> pool = new()
                {
                    "Health",
                    "Damage",
                    "Speed"
                };

                pool.AddRange(GetPossiblePlayerWeaponUpgrades(players[i]));

                for (int j = 0; j < upgrades.Length; j++)
                {
                    upgrades[j] = pool[Random.Range(0, pool.Count)];
                    pool.Remove(upgrades[j]);

                    UpgradeData data = null;
                    int level = 0;

                    //print(upgrades[j]);
                    switch (upgrades[j])
                    {
                        case "Ladder":
                            data = upgradeNames["Spinning Ladder"];
                            level = currentPlayer.WeaponManager.GetLevelNext("Ladder");
                            break;
                        case "Hammer":
                            data = upgradeNames["Hammer"];
                            level = currentPlayer.WeaponManager.GetLevelNext("Hammer");
                            break;
                        case "CorkGun":
                            data = upgradeNames["Cork Gun"];
                            level = currentPlayer.WeaponManager.GetLevelNext("CorkGun");
                            break;
                        case "Bat":
                            data = upgradeNames["Baseball Bat"];
                            level = currentPlayer.WeaponManager.GetLevelNext("Bat");
                            break;
                        case "Health":
                            data = upgradeNames["Medkit"];
                            break;
                        case "Damage":
                            data = upgradeNames["Steroids"];
                            break;
                        case "Speed":
                            data = upgradeNames["BlueCow"];
                            break;
                    }
                    if(data == null)
                    {
                        throw new System.Exception("You forgot to set something, didn't you?");
                    }
                    upgradeImages[j].sprite = data.Sprite;
                    names[j] = data.UpgradeName;
                    descriptions[j] = data.Descriptions[level];
                }
            //}

            PlayerInput pInput = currentPlayer.GetComponent<PlayerInput>();
            InputAction selectA = pInput.currentActionMap.FindAction("Selection1");
            InputAction selectB = pInput.currentActionMap.FindAction("Selection2");
            InputAction selectC = pInput.currentActionMap.FindAction("Selection3");

            selectA.started += SelectA_started;
            selectB.started += SelectB_started;
            selectC.started += SelectC_started;

            unSelected = true;
            while (unSelected)
            {
                yield return null;
            }

            selectA.started -= SelectA_started;
            selectB.started -= SelectB_started;
            selectC.started -= SelectC_started;
        }

        UpgradeMenu.SetActive(false);

        Time.timeScale = 1.0f;
    }

    //Theres...probably a better way to do this
    //Darn you unity input system
    private void SelectC_started(InputAction.CallbackContext obj)
    {
        if(buttonHit != 2)
        {
            descriptionText.text = descriptions[2];
            nameText.text = names[2];
            buttonHit = 2;
            descriptionText.gameObject.SetActive(true);
            nameText.gameObject.SetActive(true);
            return;
        }
        SelectWeapon(2);
    }

    private void SelectB_started(InputAction.CallbackContext obj)
    {
        if (buttonHit != 1)
        {
            descriptionText.text = descriptions[1];
            nameText.text = names[1];
            buttonHit = 1;
            descriptionText.gameObject.SetActive(true);
            nameText.gameObject.SetActive(true);
            return;
        }
        SelectWeapon(1);
    }

    private void SelectA_started(InputAction.CallbackContext obj)
    {
        if (buttonHit != 0)
        {
            descriptionText.text = descriptions[0];
            nameText.text = names[0];
            buttonHit = 0;
            descriptionText.gameObject.SetActive(true);
            nameText.gameObject.SetActive(true);
            return;
        }
        SelectWeapon(0);
    }

    private void SelectWeapon(int id)
    {
        unSelected = false;

        PlayerStatManager stats = currentPlayer.GetComponent<PlayerStatManager>();

        if (upgrades[id] == "Health")
        {
            float currentHealth = stats.PlayerStats.Health / stats.PlayerStats.HealthModifierMultiplicitive;
            stats.PlayerStats.HealthModifierMultiplicitive += 0.25f;
            currentHealth *= stats.PlayerStats.HealthModifierMultiplicitive;

            stats.PlayerStats.Health = Mathf.RoundToInt(currentHealth);
            return;
        }

        if (upgrades[id] == "Damage")
        {
            stats.PlayerStats.DamageModifierMultiplicitive += 0.25f;
            return;
        }

        if (upgrades[id] == "Speed")
        {
            stats.PlayerStats.MovementModifierMultiplicitive += 0.25f;
            return;
        }

        currentPlayer.WeaponManager.WeaponUpgrade(upgrades[id]);
    }

    private List<string> GetPossiblePlayerWeaponUpgrades(PlayerMovement player)
    {
        List<string> weapons = new();
        List<string> allWeapons = new()
        {
            "Hammer",
            "Ladder",
            "CorkGun",
            "Bat"
        };
        List<string> evaluatedWeapons = new();
        WeaponManager manager = player.WeaponManager;

        WeaponStat[] weaponStats = manager.GetWeapons();

        foreach (WeaponStat stat in weaponStats)
        {
            if(stat == null)
            {
                continue;
            }

            if(stat.Level < 3)
            {
                weapons.Add(stat.Weapon);
            }
            evaluatedWeapons.Add(stat.Weapon);
        }

        if (weaponStats.Contains(null))
        {
            foreach(string weap in allWeapons)
            {
                if (!evaluatedWeapons.Contains(weap))
                {
                    weapons.Add(weap);
                }
            }
        }

        return weapons;
    }
}
