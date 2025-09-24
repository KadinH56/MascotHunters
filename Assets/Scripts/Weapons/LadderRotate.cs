using UnityEngine;

public class LadderRotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform rotateAround;
    [SerializeField] private bool isRotating;

    /// <summary>
    /// If the bool is true, the ladder will rotate around the fixed object
    /// </summary>
    void FixedUpdate()
    {
        if(isRotating == true)
        {
            this.transform.RotateAround(Vector3.forward, rotateAround.position, rotationSpeed * Time.deltaTime);
        }
    }
}
