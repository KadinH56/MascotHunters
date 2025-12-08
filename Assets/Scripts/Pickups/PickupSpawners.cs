using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PickupSpawners : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnableItems = new();
    [SerializeField] private int maxItems = 2;

    [SerializeField] private Vector2 randomTimeSeconds = new(30, 90);

    private List<GameObject> spawnedItems = new();

    private void Start()
    {
        StartCoroutine(SpawnItem());
    }

    private IEnumerator SpawnItem()
    {
        while (true)
        {
            //If we have the max amount of items, stall this script
            if(spawnedItems.Count >= maxItems)
            {
                foreach (GameObject item in spawnedItems)
                {
                    if(item == null || !item.activeSelf)
                    {
                        spawnedItems.Remove(item);
                    }
                }
                yield return null;
            }
            //Keep this at the top because we don't want to start by spawning in an item
            yield return new WaitForSeconds(Random.Range(randomTimeSeconds.x, randomTimeSeconds.y));

            List<Transform> allowedChildren = new();
            foreach(Transform child in transform.GetComponentInChildren<Transform>())
            {
                if(child.childCount == 0)
                {
                    allowedChildren.Add(child);
                }
            }

            //Safety check
            if(allowedChildren.Count == 0)
            {
                continue;
            }

            //Spawn the item
            spawnedItems.Add(Instantiate(spawnableItems[Random.Range(0, spawnableItems.Count)], allowedChildren[Random.Range(0, allowedChildren.Count)]));
        }
    }
}
