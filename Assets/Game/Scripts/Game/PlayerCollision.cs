using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public GameManager gameManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            gameManager.CheckRevive(DeathType.SPIKE_HIT);
        }
        if (collision.gameObject.CompareTag("Water"))
        {
            gameManager.CheckRevive(DeathType.WATER_STUCK);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            gameManager.OnCoinHit();
        }
    }
}
