using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float minSpawnRadius = 25f;
    [SerializeField] private float maxSpawnRadius = 40f;

    private Dictionary<int, List<GameObject>> enemiesByCost = new();
    private List<int> costsInOrder = new();

    private int credits = 0;

    [SerializeField] private int creditsPerWave = 3;
    [SerializeField] private int startingCredits = 3;
    /// <summary>
    /// Waves x Cost = Unlocked enemy, basically. Couldn't think of a better name I'm sorry.
    /// </summary>
    [SerializeField] private int costWaveMultiplier = 2;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private Vector3 boxSize = new(1.5f, 1, 1.5f);

    [SerializeField] private float secondsBeforeNextWave = 2.5f;

    private List<Object> bosses;

    private CameraFollower cam;
    
    /// <summary>
    /// Quickly set this up
    /// </summary>
    private void Start()
    {
        Object[] enemies = Resources.LoadAll("Enemies/Fodder", typeof(GameObject));
        bosses = Resources.LoadAll("Enemies/Bosses", typeof(GameObject)).ToList();

        foreach (GameObject enemy in enemies)
        {
            EnemyScript script = enemy.GetComponent<EnemyScript>();
            if (!enemiesByCost.ContainsKey(script.Cost))
            {
                enemiesByCost.Add(script.Cost, new List<GameObject>());
                costsInOrder.Add(script.Cost);
            }

            enemiesByCost[script.Cost].Add(enemy);
        }

        costsInOrder.Sort();
        costsInOrder.Reverse();

        cam = FindFirstObjectByType<CameraFollower>();

        GameInformation.Wave = 1;
    }
    public void StartSpawningEnemies()
    {
        StartCoroutine(EnemySpawn());
    }
    
    /// <summary>
    /// The while loops scare me. Should spawn enemies at runtime when none remain
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnemySpawn()
    {
        //Guys I swear all this math makes sense you're going to have to trust me
        while (true)
        {
            yield return new WaitForSeconds(secondsBeforeNextWave);
            GameInformation.TotalEnemies = 0;

            if(GameInformation.Wave % 5 != 0)
            {
                credits = startingCredits + (creditsPerWave * GameInformation.Wave);
                credits *= GameInformation.NumPlayers;

                int highestCost = costsInOrder[0];
                while (credits > 0)
                {
                    if (highestCost > credits)
                    {
                        highestCost = credits;
                    }

                    if (highestCost > 1 && highestCost > GameInformation.Wave / (float)costWaveMultiplier)
                    {
                        //print("Occurs?");
                        highestCost--;
                        continue;
                    }

                    List<int> allowedCosts = new();
                    foreach (int cost in costsInOrder)
                    {
                        if (cost <= highestCost)
                        {
                            allowedCosts.Add(cost);
                        }
                    }

                    //Okay lets run a quick physics check and get a position. Who's ready for TRIPLE WHILE LOOPS
                    Vector3? position = null;

                    while (position == null)
                    {
                        Vector3 desiredPos = Quaternion.Euler(0, Random.Range(0f, 360f), 0) * Vector3.forward * Random.Range(minSpawnRadius, maxSpawnRadius);
                        //desiredPos += Vector3.up * 1.5f;
                        desiredPos += new Vector3(cam.Average.x, 1.5f, cam.Average.z);
                        bool gotHit = Physics.CheckBox(desiredPos, boxSize / 2f, Quaternion.identity, groundLayers);
                        //Physics check
                        //I'm not sure if I need the wait for fixed update...but I don't want to crash the game
                        yield return new WaitForFixedUpdate();
                        if (!gotHit)
                        {
                            position = desiredPos;
                            break;
                        }
                    }

                    //Now we spawn enemies
                    List<GameObject> enemyPool = enemiesByCost[allowedCosts[Random.Range(0, allowedCosts.Count)]];
                    GameObject enemy = enemyPool[Random.Range(0, enemyPool.Count)];

                    //position += 3f * enemy.GetComponent<EnemyScript>().Size * Vector3.up / 4f;

                    Instantiate(enemy, (Vector3)position, Quaternion.identity);
                    credits -= enemy.GetComponent<EnemyScript>().Cost;
                    //print("I got this far");
                    yield return null;

                    GameInformation.TotalEnemies++;
                }
            }
            else
            {
                GameObject boss = GenerateBoss();

                //int bossCredits = Mathf.CeilToInt(GameInformation.Wave / 15f) - 1; //TODO: Levelup systems nyeheh

                Vector3? position = null;

                while (position == null)
                {
                    Vector3 desiredPos = Quaternion.Euler(0, Random.Range(0f, 360f), 0) * Vector3.forward * Random.Range(minSpawnRadius, maxSpawnRadius);
                    //desiredPos += Vector3.up * 1.5f;
                    desiredPos += new Vector3(cam.Average.x, 1.5f, cam.Average.z);
                    bool gotHit = Physics.CheckBox(desiredPos, boxSize / 2f, Quaternion.identity, groundLayers);
                    //Physics check
                    //I'm not sure if I need the wait for fixed update...but I don't want to crash the game
                    yield return new WaitForFixedUpdate();
                    if (!gotHit)
                    {
                        position = desiredPos;
                        break;
                    }
                }

                //position += Vector3.up * boss.GetComponent<EnemyScript>().Size / 2f;
                //Now we spawn boss
                Instantiate(boss, (Vector3)position, Quaternion.identity);
                GameInformation.TotalEnemies++;
            }

            GameInformation.EnemiesRemaining = GameInformation.TotalEnemies;
            FindFirstObjectByType<EnemyWaveBar>().ApplyEnemyCount();

            while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            {
                yield return null;
            }

            if (GameInformation.Wave % 5 == 0)
            {
                FindFirstObjectByType<UpgradeSystem>().StartUpgrades();
            }

            GameInformation.Wave++;
            yield return null;
        }
    }

    private GameObject GenerateBoss()
    {
        if (bosses.Count == 0) {
            bosses = Resources.LoadAll("Enemies/Bosses", typeof(GameObject)).ToList();
        }

        GameObject boss = (GameObject)bosses[0];
        bosses.RemoveAt(0);

        return boss;
    }
}
