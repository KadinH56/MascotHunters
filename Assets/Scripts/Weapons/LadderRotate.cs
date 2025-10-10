using UnityEngine;

public class LadderRotate : WeaponBase
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool isRotating;

    [SerializeField] private GameObject secondLadderObject;
    [SerializeField] private Collider secondLadderCollider;

    private void Start()
    {
        LevelupDescriptions.Add(1, "Get a Spinning Ladder");
        LevelupDescriptions.Add(2, "Add a Second Ladder");
        weaponLevel = 1;
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
            other.gameObject.GetComponent<EnemyScript>().TakeDamage(CalculateDamage());   
        }
    }

    public override void LevelUpWeapon()
    {
        weaponLevel++;
        switch (weaponLevel)
        {
            case 2:
                //speedModifier = 0.9f;
                secondLadderCollider.enabled = true;
                secondLadderObject.SetActive(true);
                break;
        }
    }
}
