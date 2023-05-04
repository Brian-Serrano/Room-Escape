using UnityEngine;

public class IDestroyObstacle : MonoBehaviour
{
    public Transform playerPos;

    void Update()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if (playerPos.position.z - 300 >= transform.GetChild(i).position.z)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
