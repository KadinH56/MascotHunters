using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    public void StartSpawningEnemies()
    {
        StartCoroutine(EnemySpawn());
    }

    private IEnumerator EnemySpawn()
    {
        yield return new WaitForSeconds(5f);
        Instantiate(enemy, Vector3.zero, Quaternion.identity);
    }
}
