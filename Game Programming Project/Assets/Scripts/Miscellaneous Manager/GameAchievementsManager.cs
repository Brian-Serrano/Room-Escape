using System.Linq;
using UnityEngine;

public class GameAchievementsManager : MonoBehaviour
{
    [HideInInspector] public static GameAchievementsManager instance;

    private string[] achievementName;
    private string[,] reward;

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

        achievementName = new string[]
        {
            "Leap of Faith", "Bouncing Around", "High Jumper", "Trial Runner", "Persistent Parkourist", "Unstoppable Traceur", "Marathon Runner",
            "Dedicated Runner", "Endurance Runner", "Beginner Traceur", "Skillful Jumper", "Experienced Traceur", "Advanced Parkourist", "Master Traceur",
            "Legendary Runner", "Parkour Prodigy", "Extreme Athlete", "Parkour Virtuoso", "Parkour Titan", "Parkour Legend", "Penny Pincher",
            "Coin Collector", "Money Maker", "Gold Hoarder", "Treasure Hunter", "Wealth Accumulator", "Coin Connoisseur", "Coin Tycoon",
            "Money Magnet", "Rich Runner", "Point Scorer", "Double the Fun", "Triple Threat", "Point Prodigy", "Point Master",
            "Texture Enthusiast", "Texture Collector", "Texture Connoisseur", "Newcomer", "Quest Master"
        };

        reward = new string[,]
        {
            { "Coin", "100" },
            { "Texture", "11" },
            { "Texture", "38" },
            { "Coin", "150" },
            { "Texture", "32" },
            { "Coin", "500" },
            { "Coin", "250" },
            { "Texture", "43" },
            { "Texture", "13" },
            { "Coin", "150" },
            { "Texture", "22" },
            { "Coin", "450" },
            { "Texture", "2" },
            { "Texture", "18" },
            { "Coin", "900" },
            { "Coin", "1250" },
            { "Texture", "9" },
            { "Coin", "1500" },
            { "Coin", "2000" },
            { "Texture", "49" },
            { "Texture", "4" },
            { "Texture", "20" },
            { "Coin", "250" },
            { "Texture", "47" },
            { "Coin", "750" },
            { "Texture", "16" },
            { "Texture", "34" },
            { "Coin", "2000" },
            { "Coin", "2500" },
            { "Texture", "36" },
            { "Coin", "250" },
            { "Texture", "45" },
            { "Texture", "30" },
            { "Texture", "26" },
            { "Coin", "1250" },
            { "Coin", "250" },
            { "Texture", "41" },
            { "Coin", "750" },
            { "Texture", "28" },
            { "Coin", "500" }
        };

        ProcessAchievements();
    }

    public void ProcessAchievements()
    {
        Jump(10, 0); Jump(100, 1); Jump(1000, 2);
        Attempt(10, 3); Attempt(100, 4); Attempt(1000, 5);
        TimesPlayed(43200, 6); TimesPlayed(432000, 7); TimesPlayed(4320000, 8);
        Level(10, 9); Level(20, 10); Level(30, 11); Level(40, 12); Level(50, 13); Level(100, 14); Level(150, 15); Level(200, 16); Level(250, 17); Level(500, 18); Level(1000, 19);
        Coin(500, 20); Coin(1000, 21); Coin(2500, 22); Coin(5000, 23); Coin(7500, 24); Coin(10000, 25); Coin(20000, 26); Coin(30000, 27); Coin(40000, 28); Coin(50000, 29);
        Points(1000, 30); Points(2000, 31); Points(3000, 32); Points(5000, 33); Points(10000, 34);
        Textures(30, 35); Textures(40, 36); Textures(50, 37);
        LogIn(38); Quest(39);

        CheckCompletedAchievement();
    }

    private void Jump(int amount, int index)
    {
        TemporaryData.instance.progressAchievements[index] = ((float)TemporaryData.instance.totalJumps / amount) * 100;
    }

    private void Attempt(int amount, int index)
    {
        TemporaryData.instance.progressAchievements[index] = ((float)TemporaryData.instance.totalAttempts / amount) * 100;
    }

    private void TimesPlayed(int amount, int index)
    {
        TemporaryData.instance.progressAchievements[index] = ((float)TemporaryData.instance.totalTime / amount) * 100;
    }

    private void Level(int amount, int index)
    {
        TemporaryData.instance.progressAchievements[index] = ((float)TemporaryData.instance.level / amount) * 100;
    }

    private void Coin(int amount, int index)
    {
        TemporaryData.instance.progressAchievements[index] = ((float)TemporaryData.instance.totalCoins / amount) * 100;
    }

    private void Points(int amount, int index)
    {
        TemporaryData.instance.progressAchievements[index] = ((float)TemporaryData.instance.highscore / amount) * 100;
    }

    private void Textures(int amount, int index)
    {
        TemporaryData.instance.progressAchievements[index] = ((float)TemporaryData.instance.ownedMaterials.Count(x => x == 1) / amount) * 100;
    }

    private void LogIn(int index)
    {
        TemporaryData.instance.progressAchievements[index] = TemporaryData.instance.isLoggedIn ? 100 : 0;
    }

    private void Quest(int index)
    {
        TemporaryData.instance.progressAchievements[index] = ((float)TemporaryData.instance.completedQuestsOneGame / 3) * 100;
    }

    private void CheckCompletedAchievement()
    {
        for(int i = 0; i < TemporaryData.instance.progressAchievements.Length; i++)
        {
            if (TemporaryData.instance.progressAchievements[i] >= 100 && TemporaryData.instance.completedAchievements[i] == 0)
            {
                TemporaryData.instance.completedAchievements[i] = 1;
                ToastMessage.instance.Show(achievementName[i] + " Completed!");
                switch(reward[i, 0])
                {
                    case "Texture":
                        TemporaryData.instance.ownedMaterials[int.Parse(reward[i, 1])] = 1;
                        break;
                    case "Coin":
                        TemporaryData.instance.totalCoins += int.Parse(reward[i, 1]);
                        TemporaryData.instance.coins += int.Parse(reward[i, 1]);
                        break;
                }
            }
        }
    }
}
