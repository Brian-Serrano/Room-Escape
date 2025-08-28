using UnityEngine;

public class PlayerInfCollision : MonoBehaviour
{
    public InfiniteManager infiniteManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            infiniteManager.OnSpikeHit();
        }
        if (collision.gameObject.CompareTag("Water"))
        {
            infiniteManager.OnWaterHit();
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
