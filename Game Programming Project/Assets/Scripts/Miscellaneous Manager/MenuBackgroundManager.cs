using UnityEngine;

public class MenuBackgroundManager : MonoBehaviour
{
    public GameObject obstacleManager;

    public GameObject room;

    private int ROOM_SIZE = 5;

    private void Awake()
    {
        Instantiate(room, new Vector3(0, 6, -16), Quaternion.identity, transform);
        Instantiate(room, new Vector3(0, 6, (ROOM_SIZE * 20) - 4), Quaternion.Euler(0, 180, 0), transform);

        for (int i = 0; i < ROOM_SIZE; i++)
        {
            Instantiate(obstacleManager, new Vector3(0, 0, i * 20), Quaternion.identity, transform);
        }
    }
}
