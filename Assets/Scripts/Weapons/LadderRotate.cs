using UnityEngine;

public class LadderRotate : WeaponBase
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool isRotating;
    [SerializeField] private AudioClip ladderHit;

    [SerializeField] private GameObject secondLadderObject;
    [SerializeField] private Collider secondLadderCollider;
    [SerializeField] private CapsuleCollider firstLadderCollider;

    [SerializeField] private GameObject[] extensions;

    private void Start()
    {
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
            AudioSource.PlayClipAtPoint(ladderHit, transform.position);
        }
    }

    public override void LevelUpWeapon()
    {
        weaponLevel++;
        switch (weaponLevel)
        {
            case 2:
                foreach (GameObject go in extensions)
                {
                    go.SetActive(true);
                }
                firstLadderCollider.center = new(-3.2f, 0, 0);
                firstLadderCollider.height = 5.1f;
                
                break;
            case 3:
                //speedModifier = 0.9f;
                secondLadderCollider.enabled = true;
                secondLadderObject.SetActive(true);
                break;
        }
    }
}
