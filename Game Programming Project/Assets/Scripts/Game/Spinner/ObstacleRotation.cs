using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleRotation : MonoBehaviour
{
    private float speed;
    private float[] orientation;

    void Start()
    {
        orientation = new float[] { Random.Range(20f, 30f), Random.Range(-20f, -30f) };

        if(name == "Main Camera" || SceneManager.GetActiveScene().name == "Theme")
        {
            speed = 7f;
        }
        else if(name == "Spinner(Clone)")
        {
            speed = orientation[Random.Range(0, orientation.Length)];
        }
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
    }
}
