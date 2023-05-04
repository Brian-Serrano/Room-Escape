using UnityEngine;

public class UpandDown : MonoBehaviour
{
    private float speed;
    private float pointOne;
    private float pointTwo;
    private bool switchDirection = true;

    void Start()
    {
        speed = Random.Range(1f, 4f);
        pointOne = transform.position.y - 1.5f;
        pointTwo = transform.position.y + 1.5f;
    }

    void Update()
    {
        if (transform.position.y >= pointTwo)
        {
            switchDirection = false;
        }
        if (transform.position.y <= pointOne)
        {
            switchDirection = true;
        }
        if (switchDirection)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
    }
}
