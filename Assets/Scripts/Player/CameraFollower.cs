using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private float maxCameraDistance = 25f;

    [SerializeField] private float cameraHeightMin = 10f;
    [SerializeField] private Vector3 offSet = Vector3.zero;
    private MainProcGen generation;

    private Vector3 average = Vector3.zero;

    public float MaxCameraDistance { get => maxCameraDistance; }
    public Vector3 OffSet { get => offSet; set => offSet = value; }
    public Vector3 Average { get => average; set => average = value; }

    private Vector2Int? lastGeneration = null;

    private void Start()
    {
        generation = FindFirstObjectByType<MainProcGen>();
    }

    /// <summary>
    /// Get average distance and generate new chunks
    /// </summary>
    private void Update()
    {
        //Get the average distance 
        List<Vector3> positions = new();
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (!player.activeSelf)
            {
                continue;
            }

            positions.Add(player.transform.position);
        }

        if(positions.Count == 0)
        {
            return;
        }

        average = Vector3.zero;

        foreach (Vector3 position in positions)
        {
            average += new Vector3(position.x, 0, position.z);
        }

        average /= (float)positions.Count;

        //I'm cheating here. The player should always be at 1.5f...I hope
        float yValue = Vector3.Distance(positions[0] + (Vector3.down * 1.5f), average);

        average += Vector3.up * Mathf.Max(cameraHeightMin, yValue);

        transform.position = average + offSet;

        //We're not even gonna think about FOV
        //float fov = Mathf.Lerp(40, 60, (Mathf.Min(Mathf.Max(yValue, cameraHeightMin), maxCameraDistance) - cameraHeightMin) / (maxCameraDistance - cameraHeightMin));
        //GetComponent<CinemachineCamera>().Lens.FieldOfView = fov;

        //Generation
        Vector2Int currentPosition = new(Mathf.RoundToInt((transform.position.x - OffSet.x) / generation.ChunkSize), 
            Mathf.RoundToInt((transform.position.z - OffSet.z) / generation.ChunkSize));

        if(currentPosition != lastGeneration)
        {
            generation.Generate(transform.position - OffSet);
            lastGeneration = currentPosition;
        }
    }
}
