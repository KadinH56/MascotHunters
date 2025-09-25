using UnityEngine;
using UnityEngine.UIElements;

public class LadderRotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool isRotating;

    private EnemyScript enemyScript;

    /// <summary>
    /// If the bool is true, the ladder will rotate around the fixed object
    /// </summary>
    void FixedUpdate()
    {
        if(isRotating == true)
        {
            transform.Rotate(0, rotationSpeed, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Taking damage!");
            other.gameObject.GetComponent<EnemyScript>().TakeDamage(1);   
        }
    }
}
