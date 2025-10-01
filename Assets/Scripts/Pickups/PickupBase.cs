using UnityEngine;

public class PickupBase : MonoBehaviour
{
    public virtual void Effect(PlayerStatManager player)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Effect(other.gameObject.transform.parent.GetComponent<PlayerStatManager>());
            Destroy(gameObject);
        }
    }
}
