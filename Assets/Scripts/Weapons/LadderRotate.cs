using UnityEngine;
using UnityEngine.UIElements;

public class LadderRotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool isRotating;

    [SerializeField] private int baseDamage = 1;
    private Stats playerStats;

    private void Start()
    {
        playerStats = transform.parent.GetComponent<PlayerStatManager>().PlayerStats;
    }

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
            other.gameObject.GetComponent<EnemyScript>().TakeDamage(Mathf.RoundToInt((baseDamage + playerStats.DamageModifierAdditive) * playerStats.DamageModifierMultiplicitive));   
        }
    }
}
