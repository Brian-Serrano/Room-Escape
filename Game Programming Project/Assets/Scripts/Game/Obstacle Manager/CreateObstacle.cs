using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateObstacle : MonoBehaviour
{
    // Floor Offsets (0, 3, 7.5), (0, 3, 2.5), (0, 3, -2.5), (0, 3, -7.5) id = 1, 2, 3, 4
    // Spinning Obstacle Offsets (0, 6, 0), (0, 6, 5), (0, 6, -5) id = 5, 6, 7
    // Up Down Obstacle Offsets (0, 4.5, 1.5), (0, 4.5, -1.5), (0, 4.5, 3.5), (0, 4.5, 6.5), (0, 4.5, -3.5), (0, 4.5, -6.5) id = 8, 9, 10
    // Forward Backward Obstacle Offsets (0, 4.5, 0), (0, 4.5, 5), (0, 4.5, -5) id = 11, 12, 13
    // Door Obstacle Offsets (0, 8, 7.5), (0, 8, 2.5), (0, 8, -2.5), (0, 8, -7.5) id = 14, 15, 16, 17
    // 1 Spike (0, 6, 7.5), (0, 6, 2.5), (0, 6, -2.5), (0, 6, -7.5) id = 18, 19, 20, 21
    // 2 Spike (-2.5 and 2.5, 6, 7.5), (-2.5 and 2.5, 6, 2.5), (-2.5 and 2.5, 6, -2.5), (-2.5 and 2.5, 6, -7.5) id = 22, 23, 24, 25
    // 3 Spike (-3.5, 0 and 3.5, 6, 7.5), (-3.5, 0 and 3.5, 6, 2.5), (-3.5, 0 and 3.5, 6, -2.5), (-3.5, 0 and 3.5, 6, -7.5) id = 26, 27, 28, 29
    // Acid (0, 2, 7.5), (0, 2, 2.5), (0, 2, -2.5), (0, 2, -7.5) id = 30, 31, 32, 33

    // Floor index = 0
    // Spinning Obstacle index = 1
    // UD Obstacle index = 2
    // FW Obstacle index = 3
    // Door Obstacle index = 4
    // Spike index = 5
    // Acid index = 6
    // Room index = 7

    private List<int> numbers1;
    private List<int> numbers2;

    private List<List<int>> obstacles;

    public GameObject[] structures;

    private List<List<float>> obstaclesToCreate;

    void Awake()
    {
        numbers1 = new List<int>() { 5, 8, 11 };
        numbers2 = new List<int>() { 14, 18, 22, 26 };

        obstacles = new List<List<int>>()
        {
            new List<int>() {4, 32, 31, 1, numbers1[Random.Range(0, numbers1.Count)], numbers2[Random.Range(0, numbers2.Count)] + 3, numbers2[Random.Range(0, numbers2.Count)]},

            new List<int>() {33, 32, 2, 1, numbers1[Random.Range(0, numbers1.Count)] + 2, numbers2[Random.Range(0, numbers2.Count)] + 1, numbers2[Random.Range(0, numbers2.Count)]},

            new List<int>() {4, 3, 31, 30, numbers1[Random.Range(0, numbers1.Count)] + 1, numbers2[Random.Range(0, numbers2.Count)] + 3, numbers2[Random.Range(0, numbers2.Count)] + 2},

            new List<int>() {33, 32, 31, 30, numbers1[Random.Range(0, numbers1.Count)] + 1, numbers1[Random.Range(0, numbers1.Count)] + 2},

            new List<int>() {4, 3, 2, 1, numbers2[Random.Range(0, numbers2.Count)] + 3, numbers2[Random.Range(0, numbers2.Count)] + 2, numbers2[Random.Range(0, numbers2.Count)] + 1, numbers2[Random.Range(0, numbers2.Count)]}
        };

        // [0] Game Object, [1] X Position, [2] Y Position, [3] Z Position
        obstaclesToCreate = new List<List<float>>()
        {
            new List<float>() {0f, 0f, 3f, 7.5f},
            new List<float>() {0f, 0f, 3f, 2.5f},
            new List<float>() {0f, 0f, 3f, -2.5f},
            new List<float>() {0f, 0f, 3f, -7.5f},
            new List<float>() {1f, 0f, 6f, 0f},
            new List<float>() {1f, 0f, 6f, 5f},
            new List<float>() {1f, 0f, 6f, -5f},
            new List<float>() {2f, 0f, 4.5f, 1.5f, 2f, 0f, 4.5f, -1.5f},
            new List<float>() {2f, 0f, 4.5f, 3.5f, 2f, 0f, 4.5f, 6.5f},
            new List<float>() {2f, 0f, 4.5f, -3.5f, 2f, 0f, 4.5f, -6.5f},
            new List<float>() {3f, 0f, 4.5f, 0f},
            new List<float>() {3f, 0f, 4.5f, 5f},
            new List<float>() {3f, 0f, 4.5f, -5f},
            new List<float>() {4f, 0f, 8f, 7.5f},
            new List<float>() {4f, 0f, 8f, 2.5f},
            new List<float>() {4f, 0f, 8f, -2.5f},
            new List<float>() {4f, 0f, 8f, -7.5f},
            new List<float>() {5f, 0f, 6.5f, 7.5f},
            new List<float>() {5f, 0f, 6.5f, 2.5f},
            new List<float>() {5f, 0f, 6.5f, -2.5f},
            new List<float>() {5f, 0f, 6.5f, -7.5f},
            new List<float>() {5f, -2.5f, 6.5f, 7.5f, 5f, 2.5f, 6.5f, 7.5f},
            new List<float>() {5f, -2.5f, 6.5f, 2.5f, 5f, 2.5f, 6.5f, 2.5f},
            new List<float>() {5f, -2.5f, 6.5f, -2.5f, 5f, 2.5f, 6.5f, -2.5f},
            new List<float>() {5f, -2.5f, 6.5f, -7.5f, 5f, 2.5f, 6.5f, -7.5f},
            new List<float>() {5f, -3.5f, 6.5f, 7.5f, 5f, 0f, 6.5f, 7.5f, 5f, 3.5f, 6.5f, 7.5f},
            new List<float>() {5f, -3.5f, 6.5f, 2.5f, 5f, 0f, 6.5f, 2.5f, 5f, 3.5f, 6.5f, 2.5f},
            new List<float>() {5f, -3.5f, 6.5f, -2.5f, 5f, 0f, 6.5f, -2.5f, 5f, 3.5f, 6.5f, -2.5f},
            new List<float>() {5f, -3.5f, 6.5f, -7.5f, 5f, 0f, 6.5f, -7.5f, 5f, 3.5f, 6.5f, -7.5f},
            new List<float>() {6f, 0f, 2f, 7.5f},
            new List<float>() {6f, 0f, 2f, 2.5f},
            new List<float>() {6f, 0f, 2f, -2.5f},
            new List<float>() {6f, 0f, 2f, -7.5f}
        };

        List<int> randomObstacles;

        if (SceneManager.GetActiveScene().name == "Menu" || SceneManager.GetActiveScene().name == "Infinite")
        {
            randomObstacles = obstacles[Random.Range(0, obstacles.Count)];
        }
        else
        {
            if (TemporaryData.instance.levelAttempts >= 2)
            {
                randomObstacles = TemporaryData.instance.currentLevel[(int)(transform.position.z) / 20];
            }
            else
            {
                randomObstacles = obstacles[Random.Range(0, obstacles.Count)];
                TemporaryData.instance.currentLevel.Add(randomObstacles);
            }
        }

        Instantiate(structures[7], new Vector3(0, 6, 0) + transform.position, Quaternion.identity, transform);

        foreach(int o in randomObstacles)
        {
            List<float> createIndex = obstaclesToCreate[o - 1];

            Instantiate(structures[(int) createIndex[0]], new Vector3(createIndex[1], createIndex[2], createIndex[3]) + transform.position, Quaternion.identity, transform);

            if (createIndex.Count >= 8)
            {
                Instantiate(structures[(int) createIndex[4]], new Vector3(createIndex[5], createIndex[6], createIndex[7]) + transform.position, Quaternion.identity, transform);

                if (createIndex.Count >= 12)
                {
                    Instantiate(structures[(int) createIndex[8]], new Vector3(createIndex[9], createIndex[10], createIndex[11]) + transform.position, Quaternion.identity, transform);
                }
            }
        }
    }
}
