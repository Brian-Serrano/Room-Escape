using TMPro;
using UnityEngine;

public class AchievementsManager : MonoBehaviour
{
    private GameObject[] achSlots;

    void Start()
    {
        achSlots = GameObject.FindGameObjectsWithTag("Achievement");

        for(int i = 0; i < achSlots.Length; i++)
        {
            if (TemporaryData.instance.completedAchievements[i] == 1)
            {
                achSlots[i].transform.Find("Completed").gameObject.SetActive(true);
                achSlots[i].transform.Find("Status").gameObject.SetActive(false);
            }
            else
            {
                achSlots[i].transform.Find("Completed").gameObject.SetActive(false);
                achSlots[i].transform.Find("Status").gameObject.SetActive(true);
                achSlots[i].transform.Find("Status").gameObject.GetComponent<TMP_Text>().text = TemporaryData.instance.progressAchievements[i].ToString("0") + "%";
            }
        }
    }
}
