using System.Collections;
using UnityEngine;

public class SFX : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    private static GameObject instance;
    private void Start()
    {
        StartCoroutine(PlaySound());
    }

    private IEnumerator PlaySound()
    {
        source.Play();
        while (source.isPlaying)
        {
            yield return null;
        }
        Destroy(gameObject);
    }

    public static void SpawnClip(AudioClip clip, Vector3 position)
    {
        GameObject sourceObject = Instantiate(instance, position, Quaternion.identity);
        sourceObject.GetComponent<AudioSource>().clip = clip;
    }

    public static void SetPrefab(GameObject prefab)
    {
        instance = prefab;
    }
}
