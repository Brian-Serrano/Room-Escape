using UnityEngine;

public class GameQuestManager : MonoBehaviour
{
    [HideInInspector] public static GameQuestManager instance;

    [HideInInspector] public string[,] questText;
    [HideInInspector] public string[,] questTitleText;

    private int tempCompQueOneGame;

    private int[,] questAmount;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        questText = new string[,]
        {
            { "Collect 20 coins", "Collect 40 coins", "Collect 60 coins" },
            { "Complete 2 levels", "Complete 4 levels", "Complete 6 levels" },
            { "Do 5 attempts", "Do 10 attempts", "Do 15 attempts" }
        };

        questTitleText = new string[,]
        {
            { "Coin Finder", "Coin Collector", "Coin Master" },
            { "Level Beater", "Level Grinder", "Level Master" },
            { "Nice Try", "Another Try", "Always Trying" }
        };

        questAmount = new int[,]
        {
            { 20, 40, 60 },
            { 2, 4, 6 },
            { 5, 10, 15 }
        };

        SetProgress();
    }

    public void SetProgress()
    {
        TemporaryData.instance.CheckDate();
        tempCompQueOneGame = 0;

        for (int i = 0; i < 3; i++)
        {
            TemporaryData.instance.questProgress[i] = (float)TemporaryData.instance.questVariables[TemporaryData.instance.currentQuest[i]] / questAmount[TemporaryData.instance.currentQuest[i], i];
        }

        CheckCompletedQuest();
    }

    private void CheckCompletedQuest()
    {
        for(int i = 0; i < TemporaryData.instance.questProgress.Length; i++)
        {
            if (TemporaryData.instance.questProgress[i] >= 1 && TemporaryData.instance.questCompleted[i] == 0)
            {
                TemporaryData.instance.questCompleted[i] = 1;
                ToastMessage.instance.Show(questTitleText[TemporaryData.instance.currentQuest[i], i] + " Completed!");
                TemporaryData.instance.totalCoins += (i + 1) * 15;
                TemporaryData.instance.coins += (i + 1) * 15;
                TemporaryData.instance.completedQuests++;
                tempCompQueOneGame++;
            }
        }

        SetCompletedQuestOneGame();
    }

    private void SetCompletedQuestOneGame()
    {
        if(TemporaryData.instance.completedQuestsOneGame < tempCompQueOneGame)
        {
            TemporaryData.instance.completedQuestsOneGame = tempCompQueOneGame;
        }
    }
}
