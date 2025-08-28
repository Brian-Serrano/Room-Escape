using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class AchievementData
{
    public List<Achievement> achievements;

    public AchievementData()
    {
        achievements = new List<Achievement>
        {
            new Achievement("Leap of Faith", "Jump 10 Times", 0f, AchievementRewardType.COIN, AchievementType.JUMP, 100, 10),
            new Achievement("Bouncing Around", "Jump 100 Times", 0f, AchievementRewardType.TEXTURE, AchievementType.JUMP, 11, 100),
            new Achievement("High Jumper", "Jump 1000 Times", 0f, AchievementRewardType.TEXTURE, AchievementType.JUMP, 38, 1000),
            new Achievement("Trial Runner", "Do 10 Attempts", 0f, AchievementRewardType.COIN, AchievementType.ATTEMPT, 150, 10),
            new Achievement("Persistent Parkourist", "Do 100 Attempts", 0f, AchievementRewardType.TEXTURE, AchievementType.ATTEMPT, 32, 100),
            new Achievement("Unstoppable Traceur", "Do 1000 Attempts", 0f, AchievementRewardType.COIN, AchievementType.ATTEMPT, 500, 1000),
            new Achievement("Marathon Runner", "Play for 12 Hours", 0f, AchievementRewardType.COIN, AchievementType.TIME, 250, 43200),
            new Achievement("Dedicated Runner", "Play for 5 Days", 0f, AchievementRewardType.TEXTURE, AchievementType.TIME, 43, 432000),
            new Achievement("Endurance Runner", "Play for 50 Days", 0f, AchievementRewardType.TEXTURE, AchievementType.TIME, 13, 4320000),
            new Achievement("Beginner Traceur", "Complete 10 Levels", 0f, AchievementRewardType.COIN, AchievementType.LEVEL, 150, 10),
            new Achievement("Skillful Jumper", "Complete 20 Levels", 0f, AchievementRewardType.TEXTURE, AchievementType.LEVEL, 22, 20),
            new Achievement("Experienced Traceur", "Complete 30 Levels", 0f, AchievementRewardType.COIN, AchievementType.LEVEL, 450, 30),
            new Achievement("Advanced Parkourist", "Complete 40 Levels", 0f, AchievementRewardType.TEXTURE, AchievementType.LEVEL, 2, 40),
            new Achievement("Master Traceur", "Complete 50 Levels", 0f, AchievementRewardType.TEXTURE, AchievementType.LEVEL, 18, 50),
            new Achievement("Legendary Runner", "Complete 100 Levels", 0f, AchievementRewardType.COIN, AchievementType.LEVEL, 900, 100),
            new Achievement("Parkour Prodigy", "Complete 150 Levels", 0f, AchievementRewardType.COIN, AchievementType.LEVEL, 1250, 150),
            new Achievement("Extreme Athlete", "Complete 200 Levels", 0f, AchievementRewardType.TEXTURE, AchievementType.LEVEL, 9, 200),
            new Achievement("Parkour Virtuoso", "Complete 250 Levels", 0f, AchievementRewardType.COIN, AchievementType.LEVEL, 1500, 250),
            new Achievement("Parkour Titan", "Complete 500 Levels", 0f, AchievementRewardType.COIN, AchievementType.LEVEL, 2000, 500),
            new Achievement("Parkour Legend", "Complete 1000 Levels", 0f, AchievementRewardType.TEXTURE, AchievementType.LEVEL, 49, 1000),
            new Achievement("Penny Pincher", "Collect 500 Coins", 0f, AchievementRewardType.TEXTURE, AchievementType.COIN, 4, 500),
            new Achievement("Coin Collector", "Collect 1000 Coins", 0f, AchievementRewardType.TEXTURE, AchievementType.COIN, 20, 1000),
            new Achievement("Money Maker", "Collect 2500 Coins", 0f, AchievementRewardType.COIN, AchievementType.COIN, 250, 2500),
            new Achievement("Gold Hoarder", "Collect 5000 Coins", 0f, AchievementRewardType.TEXTURE, AchievementType.COIN, 47, 5000),
            new Achievement("Treasure Hunter", "Collect 7500 Coins", 0f, AchievementRewardType.COIN, AchievementType.COIN, 750, 7500),
            new Achievement("Wealth Accumulator", "Collect 10000 Coins", 0f, AchievementRewardType.TEXTURE, AchievementType.COIN, 16, 10000),
            new Achievement("Coin Connoisseur", "Collect 20000 Coins", 0f, AchievementRewardType.TEXTURE, AchievementType.COIN, 34, 20000),
            new Achievement("Coin Tycoon", "Collect 30000 Coins", 0f, AchievementRewardType.COIN, AchievementType.COIN, 2000, 30000),
            new Achievement("Money Magnet", "Collect 40000 Coins", 0f, AchievementRewardType.COIN, AchievementType.COIN, 2500, 40000),
            new Achievement("Rich Runner", "Collect 50000 Coins", 0f, AchievementRewardType.TEXTURE, AchievementType.COIN, 36, 50000),
            new Achievement("Point Scorer", "Score 1000 Points", 0f, AchievementRewardType.COIN, AchievementType.SCORE, 250, 1000),
            new Achievement("Double the Fun", "Score 2000 Points", 0f, AchievementRewardType.TEXTURE, AchievementType.SCORE, 45, 2000),
            new Achievement("Triple Threat", "Score 3000 Points", 0f, AchievementRewardType.TEXTURE, AchievementType.SCORE, 30, 3000),
            new Achievement("Point Prodigy", "Score 5000 Points", 0f, AchievementRewardType.TEXTURE, AchievementType.SCORE, 26, 5000),
            new Achievement("Point Master", "Score 10000 Points", 0f, AchievementRewardType.COIN, AchievementType.SCORE, 1250, 10000),
            new Achievement("Texture Enthusiast", "Own 30 Textures", 0f, AchievementRewardType.COIN, AchievementType.TEXTURE, 250, 30),
            new Achievement("Texture Collector", "Own 40 Textures", 0f, AchievementRewardType.TEXTURE, AchievementType.TEXTURE, 41, 40),
            new Achievement("Texture Connoisseur", "Own 50 Textures", 0f, AchievementRewardType.COIN, AchievementType.TEXTURE, 750, 50),
            new Achievement("Newcomer", "Create an Account", 0f, AchievementRewardType.TEXTURE, AchievementType.LOGIN, 28, 0),
            new Achievement("Quest Master", "Complete 3 Quests at the Same Game", 0f, AchievementRewardType.COIN, AchievementType.QUEST, 500, 3)
        };
    }

    public static AchievementData LoadData()
    {
        string path = Path.Combine(Application.persistentDataPath, "achievement.re");

        return PersistentDataController.LoadData<AchievementData>(path);
    }

    public bool SaveData()
    {
        string path = Path.Combine(Application.persistentDataPath, "achievement.re");

        return PersistentDataController.SaveData(this, path);
    }
}
