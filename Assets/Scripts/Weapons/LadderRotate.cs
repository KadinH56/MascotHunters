using UnityEngine;

public class LadderRotate : WeaponBase
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool isRotating;

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
            other.gameObject.GetComponent<EnemyScript>().TakeDamage(CalculateDamage());   
        }
    }
}
