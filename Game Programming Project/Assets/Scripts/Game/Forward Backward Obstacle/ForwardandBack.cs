using UnityEngine;

public class ForwardandBack : MonoBehaviour
{
    private float speed;
    private float pointOne;
    private float pointTwo;
    private bool switchDirection = true;

    void Start()
    {
        speed = Random.Range(1f, 4f);
        pointOne = transform.position.z - 3.5f;
        pointTwo = transform.position.z + 3.5f;
    }

    void Update()
    {
        if (transform.position.z >= pointTwo)
        {
            switchDirection = false;
        }
        if (transform.position.z <= pointOne)
        {
            switchDirection = true;
        }
        if (switchDirection)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
    }
}
