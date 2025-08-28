using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public GameManager gameManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            gameManager.OnSpikeHit();
        }
        if (collision.gameObject.CompareTag("Water"))
        {
            gameManager.OnWaterHit();
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
