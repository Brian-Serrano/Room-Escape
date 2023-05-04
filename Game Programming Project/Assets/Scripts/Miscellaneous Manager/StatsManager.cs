using System.Linq;
using TMPro;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public TMP_Text completedLevels;
    public TMP_Text highscore;
    public TMP_Text coins;
    public TMP_Text totalCoins;
    public TMP_Text quests;
    public TMP_Text achievements;
    public TMP_Text jumps;
    public TMP_Text time;
    public TMP_Text attempts;
    public TMP_Text materialsOwned;

    void Start()
    {
        completedLevels.text = TemporaryData.instance.level.ToString();
        highscore.text = TemporaryData.instance.highscore.ToString();
        coins.text = TemporaryData.instance.coins.ToString();
        totalCoins.text = TemporaryData.instance.totalCoins.ToString();
        quests.text = TemporaryData.instance.completedQuests.ToString();
        achievements.text = TemporaryData.instance.completedAchievements.Count(x => x == 1).ToString();
        jumps.text = TemporaryData.instance.totalJumps.ToString();
        time.text = TemporaryData.instance.totalTime.ToString("0") + " s";
        attempts.text = TemporaryData.instance.totalAttempts.ToString();
        materialsOwned.text = TemporaryData.instance.ownedMaterials.Count(x => x == 1).ToString();
    }
}
