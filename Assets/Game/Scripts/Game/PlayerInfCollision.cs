using UnityEngine;

public class PlayerInfCollision : MonoBehaviour
{
    public InfiniteManager infiniteManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            infiniteManager.CheckRevive(DeathType.SPIKE_HIT);
        }
        if (collision.gameObject.CompareTag("Water"))
        {
            infiniteManager.CheckRevive(DeathType.WATER_STUCK);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            infiniteManager.OnCoinHit();
        }
    }
}
