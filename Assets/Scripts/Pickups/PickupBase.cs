using UnityEngine;

public class PickupBase : MonoBehaviour
{
    [SerializeField] private AudioClip PickupSFX;
    public virtual void Effect(PlayerStatManager player)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHurt"))
        {
            SFX.SpawnClip(PickupSFX, transform.position);
            Effect(other.gameObject.transform.parent.GetComponent<PlayerStatManager>());
            Destroy(gameObject);
        }
    }
}
