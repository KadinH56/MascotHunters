using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.UI;

public class UpgradeSystem : MonoBehaviour
{
    private bool unSelected;
    private PlayerMovement currentPlayer;

    string[] upgrades = new string[3];

    [SerializeField] private GameObject UpgradeMenu;

    [SerializeField] private Image[] upgradeImages;

    //Oh Dead God the serializedFields
    [SerializeField] private Sprite blueCow;
    [SerializeField] private Sprite steroids;
    [SerializeField] private Sprite medkit;
    [SerializeField] private Sprite ladder;
    [SerializeField] private Sprite  corkGun;
    [SerializeField] private Sprite hammer;

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
            upgrades = new string[3];
            if (firstUpgrade)
            {
                upgrades = new[]
                {
                    "Ladder",
                    "CorkGun",
                    "Hammer"
                };

                for (int j = 0; j < upgrades.Length; j++)
                {
                    switch (upgrades[j])
                    {
                        case "Ladder":
                            upgradeImages[j].sprite = ladder;
                            break;
                        case "Hammer":
                            upgradeImages[j].sprite = hammer;
                            break;
                        case "CorkGun":
                            upgradeImages[j].sprite = corkGun;
                            break;
                    }
                }
            }
            else
            {
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

                    //print(upgrades[j]);
                    switch (upgrades[j])
                    {
                        case "Ladder":
                            upgradeImages[j].sprite = ladder;
                            break;
                        case "Hammer":
                            upgradeImages[j].sprite = hammer;
                            break;
                        case "CorkGun":
                            upgradeImages[j].sprite = corkGun;
                            break;
                        case "Health":
                            upgradeImages[j].sprite = medkit;
                            break;
                        case "Damage":
                            upgradeImages[j].sprite = steroids;
                            break;
                        case "Speed":
                            upgradeImages[j].sprite = blueCow;
                            break;
                    }
                }
            }

            currentPlayer = players[i];
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
        SelectWeapon(2);
    }

    private void SelectB_started(InputAction.CallbackContext obj)
    {
        SelectWeapon(1);
    }

    private void SelectA_started(InputAction.CallbackContext obj)
    {
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
        WeaponManager manager = player.WeaponManager;

        WeaponStat[] weaponStats = manager.GetWeapons();

        if (weaponStats.Contains(null))
        {
            weapons.Add("CorkGun");
            weapons.Add("Hammer");
            weapons.Add("Ladder");
            return weapons;
        }

        foreach (WeaponStat weap in weaponStats)
        {
            if(weap.Level > 1)
            {
                continue;
            }
            weapons.Add(weap.Weapon);
        }

        return weapons;
    }
}
