using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RotatingObstacle : MonoBehaviour
{
    public GameObject coin;

    private float speed;
    private float[] orientation;

    private Rigidbody rb;

    void Start()
    {
        List<Vector3> positions = new List<Vector3>()
        {
            new Vector3(-1.5f, -0.3f, 0f),
            new Vector3(1.5f, -0.3f, 0f),
            new Vector3(0f, -0.3f, -1.5f),
            new Vector3(0f, -0.3f, 1.5f)
        };

        Vector3 coinPos = positions[Random.Range(0, positions.Count)] + transform.position;
        Quaternion coinRot = Quaternion.Euler(new Vector3(90f, 0f, Random.Range(0f, 180f)));

        Instantiate(coin, coinPos, coinRot, transform);

        rb = GetComponent<Rigidbody>();

        orientation = new float[] { Random.Range(30f, 40f), Random.Range(-30f, -40f) };
        speed = orientation[Random.Range(0, orientation.Length)];
    }

    void Update()
    {
        rb.MoveRotation(rb.rotation * (Quaternion.Euler(0, speed * Time.deltaTime, 0)));
    }
}
