using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.AI.Navigation;
using UnityEngine;

/// <summary>
/// The main procedural generation script
/// </summary>
public class MainProcGen : MonoBehaviour
{
    [SerializeField] private float chunkSize = 50;
    [SerializeField] private int loadedChunkDistance = 2;

    [SerializeField] private int biomes = 3;

    [SerializeField] private Transform parentTransform;

    //Look into this https://auburn.github.io/FastNoiseLite/ for visualizations
    private FastNoiseLite biomeNoise;
    private FastNoiseLite chunkNoise;

    private Dictionary<Vector2Int, GameObject> loadedMap = new Dictionary<Vector2Int, GameObject>();

    public float ChunkSize { get => chunkSize; set => chunkSize = value; }

    /// <summary>
    /// Initialize the fastnoiselites and configures them. Also randomizes the scene
    /// </summary>
    private void Start()
    {
        //We want a set seed. It'd be interesting if it selects a random layout from 256 as a reference to old games
        //But someone can yell at me later for this if they don't like that
        int seed = Random.Range(0, 256);

        //Set up biome noise
        //This is a cellular noise map
        biomeNoise = new FastNoiseLite();
        biomeNoise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        biomeNoise.SetCellularReturnType(FastNoiseLite.CellularReturnType.CellValue);
        biomeNoise.SetFrequency(0.5f);

        biomeNoise.SetSeed(seed);

        //Set up chunk noise
        //This is a standard (perlin?) (openSimplex?) noise map
        chunkNoise = new FastNoiseLite();
        chunkNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
        chunkNoise.SetFrequency(0.5f);

        chunkNoise.SetSeed(seed);
        //StartCoroutine(Generate(Vector3.zero));
    }

    public void StartGeneration(Vector3 position)
    {
        StartCoroutine(Generate(position));
    }

    /// <summary>
    /// Generate the actual game
    /// </summary>
    /// <param name="position">Player position</param>
    public IEnumerator Generate(Vector3 position)
    {
        position /= chunkSize;

        Vector2Int relativePosition = new(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));

        List<Vector2Int> loadedChunks = new();

        //Load the level
        for (int i = -loadedChunkDistance + 1; i < loadedChunkDistance; i++)
        {
            for(int j = -loadedChunkDistance + 1; j < loadedChunkDistance; j++)
            {
                Vector2Int pos = relativePosition + new Vector2Int(i, j);
                loadedChunks.Add(pos);

                if (loadedMap.ContainsKey(pos))
                {
                    continue;
                }
                //Load Gameobject code
                GameObject gameObject = GetChunkAtPoint(pos);

                gameObject = Instantiate(gameObject, parentTransform, true);
                gameObject.transform.position = new(pos.x * chunkSize, 0, pos.y * chunkSize);

                //print(new Vector3(pos.x * chunkSize, 0, pos.y * chunkSize));

                loadedMap.Add(pos, gameObject);

                yield return null;
            }
        }

        //Unload uneeded chunks
        foreach (Vector2Int pos in loadedMap.Keys)
        {
            if (loadedChunks.Contains(pos))
            {
                continue;
            }

            StartCoroutine(QueueUnloadKey(pos));
        }

        GetComponent<NavMeshSurface>().BuildNavMesh();
        FindFirstObjectByType<EnemySpawner>().StartSpawningEnemies();
    }

    private IEnumerator QueueUnloadKey(Vector2Int unloadChunk)
    {
        yield return new WaitForEndOfFrame();
        
        Destroy(loadedMap[unloadChunk]);
        loadedMap.Remove(unloadChunk);
    }

    /// <summary>
    /// Gets the chunk at a specific point. Should be nicely scalable
    /// </summary>
    /// <param name="position">Posiiton of the chunk</param>
    /// <returns></returns>
    private GameObject GetChunkAtPoint(Vector2Int position)
    {
        int biome = LucasHelper.Distribute(-1, 1, biomeNoise.GetNoise(position.x, position.y), biomes) + 1;

        GameObject[] allChunks = Resources.LoadAll<GameObject>("Biome" + biome.ToString());

        int chunk = LucasHelper.Distribute(-1, 1, chunkNoise.GetNoise(position.x, position.y), allChunks.Length);
        return allChunks[chunk];
    }
}
