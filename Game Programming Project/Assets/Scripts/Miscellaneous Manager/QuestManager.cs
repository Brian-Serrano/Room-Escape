using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public TMP_Text[] questTitle;
    public TMP_Text[] questDescription;
    public TMP_Text date;
    public GameObject[] check;
    public Image[] questProgress;

    void Start()
    {
        date.text = TemporaryData.instance.date.ToString("dd/MM/yyyy");

        for (int i = 0; i < 3; i++)
        {
            questTitle[i].text = GameQuestManager.instance.questTitleText[TemporaryData.instance.currentQuest[i],i];
            questDescription[i].text = GameQuestManager.instance.questText[TemporaryData.instance.currentQuest[i],i];
            questProgress[i].fillAmount = TemporaryData.instance.questProgress[i];

            if (TemporaryData.instance.questCompleted[i] == 1)
            {
                check[i].SetActive(true);
            }
            else
            {
                check[i].SetActive(false);
            }
        }
    }
}
