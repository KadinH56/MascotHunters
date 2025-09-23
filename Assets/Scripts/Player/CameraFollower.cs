using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private float maxCameraDistance = 25f;

    [SerializeField] private float cameraHeightMin = 10f;
    [SerializeField] private Vector3 offSet = Vector3.zero;

    public float MaxCameraDistance { get => maxCameraDistance; }
    public Vector3 OffSet { get => offSet; set => offSet = value; }

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

        Vector3 average = Vector3.zero;

        foreach (Vector3 position in positions)
        {
            average += new Vector3(position.x, 0, position.z);
        }

        average /= (float)positions.Count;

        //I'm cheating here. The player should always be at 1.5f...I hope
        float yValue = Vector3.Distance(positions[0] + (Vector3.down * 1.5f), average);

        average += Vector3.up * Mathf.Max(cameraHeightMin, yValue);

        transform.position = average + offSet;

        float fov = Mathf.Lerp(40, 60, (Mathf.Min(Mathf.Max(yValue, cameraHeightMin), maxCameraDistance) - cameraHeightMin) / (maxCameraDistance - cameraHeightMin));
        GetComponent<CinemachineCamera>().Lens.FieldOfView = fov;
    }
}
