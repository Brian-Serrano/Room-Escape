using System.Collections.Generic;
using UnityEngine;

public class GenerateCoinForRotation : MonoBehaviour
{
    public GameObject coin;

    public List<List<float>> positions;

    void Start()
    {
        positions = new List<List<float>>()
        {
            new List<float>() {-1.5f, -0.3f, 0f},
            new List<float>() {1.5f, -0.3f, 0f},
            new List<float>() {0f, -0.3f, -1.5f},
            new List<float>() {0f, -0.3f, 1.5f}
        };

        List<float> random = positions[Random.Range(0, positions.Count)];

        Instantiate(coin, new Vector3(random[0], random[1], random[2]) + transform.position, Quaternion.Euler(new Vector3(90f, 0f, Random.Range(0f, 180f))), transform);
    }
}
