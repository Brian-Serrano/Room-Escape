using UnityEngine;

public class ForwardAndBackwardObstacle : MonoBehaviour
{
    public GameObject coin;

    private float speed;
    private float pointOne;
    private float pointTwo;
    private bool switchDirection = true;

    private Rigidbody rb;

    void Start()
    {
        Vector3 coinPos = new Vector3(Random.Range(-3f, 3f), 1.2f, 0f) + transform.position;
        Quaternion coinRot = Quaternion.Euler(new Vector3(90f, 0f, Random.Range(0f, 180f)));

        Instantiate(coin, coinPos, coinRot, transform);

        rb = GetComponent<Rigidbody>();

        speed = Random.Range(3f, 4f);
        pointOne = rb.position.z - 3.5f;
        pointTwo = rb.position.z + 3.5f;
    }

    void FixedUpdate()
    {
        if (rb.position.z >= pointTwo)
        {
            switchDirection = false;
        }
        if (rb.position.z <= pointOne)
        {
            switchDirection = true;
        }

        if (switchDirection)
        {
            rb.MovePosition(rb.position + (speed * Time.fixedDeltaTime * Vector3.forward));
        }
        else
        {
            rb.MovePosition(rb.position + (speed * Time.fixedDeltaTime * Vector3.back));
        }
    }
}
