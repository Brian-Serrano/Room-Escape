using UnityEngine;

public class ObstacleMaker : MonoBehaviour
{
    public GameObject obstacleManager;

    public GameObject room;

    [HideInInspector] public int randomNumber;

    void Awake()
    {
        if(TemporaryData.instance.levelAttempts >= 2)
        {
            randomNumber = TemporaryData.instance.currentLevel.Count;
        }
        else
        {
            TemporaryData.instance.currentLevel.Clear();

            if (TemporaryData.instance.level <= 10)
            {
                randomNumber = Random.Range(5, 10);
            }
            else if (TemporaryData.instance.level <= 100 && TemporaryData.instance.level > 10)
            {
                randomNumber = Random.Range(10, 15);
            }
            else if (TemporaryData.instance.level <= 1000 && TemporaryData.instance.level > 100)
            {
                randomNumber = Random.Range(15, 20);
            }
            else if (TemporaryData.instance.level > 1000)
            {
                randomNumber = Random.Range(20, 25);
            }
        }


        Instantiate(room, new Vector3(0, 6, -16), Quaternion.identity, transform);
        Instantiate(room, new Vector3(0, 6, (randomNumber * 20) - 4), Quaternion.Euler(0, 180, 0), transform);

        for (int i = 0; i < randomNumber; i++)
        {
            Instantiate(obstacleManager, new Vector3(0, 0, i * 20), Quaternion.identity, transform);
        }
    }
}
