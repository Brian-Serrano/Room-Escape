using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    public float speed = 20f;

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));
    }
}
