using UnityEngine;

public class UpAndDownObstacle : MonoBehaviour
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

        speed = Random.Range(3f, 5f);
        pointOne = rb.position.y - 1.5f;
        pointTwo = rb.position.y + 1.5f;
    }

    void Update()
    {
        if (rb.position.y >= pointTwo)
        {
            switchDirection = false;
        }
        if (rb.position.y <= pointOne)
        {
            switchDirection = true;
        }

        if (switchDirection)
        {
            rb.MovePosition(rb.position + (speed * Time.deltaTime * Vector3.up));
        }
        else
        {
            rb.MovePosition(rb.position + (speed * Time.deltaTime * Vector3.down));
        }
    }
}
