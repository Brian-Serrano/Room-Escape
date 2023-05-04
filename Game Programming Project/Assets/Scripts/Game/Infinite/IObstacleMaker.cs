using UnityEngine;

public class IObstacleMaker : MonoBehaviour
{
    public GameObject createObstacle;
    public GameObject room;

    public Transform playerPos;

    private int obstacleOffset = 0;

    void Awake()
    {
        Instantiate(room, new Vector3(0, 6, -16), Quaternion.identity, transform);
    }

    void Update()
    {
        if(playerPos.position.z + 300 >= obstacleOffset * 20)
        {
            Instantiate(createObstacle, new Vector3(0, 0, obstacleOffset * 20), Quaternion.identity, transform);
            obstacleOffset++;
        }
    }
}
