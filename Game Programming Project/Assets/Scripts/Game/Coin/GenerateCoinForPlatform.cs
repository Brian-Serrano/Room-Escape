using UnityEngine;

public class GenerateCoinForPlatform : MonoBehaviour
{
    public GameObject coin;

    void Start()
    {
        Instantiate(coin, new Vector3(Random.Range(-3f, 3f), 1.2f, 0f) + transform.position, Quaternion.Euler(new Vector3(90f, 0f, Random.Range(0f, 180f))), transform);
    }
}
