using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AchievementManager
{
    public static void CheckAchievements(PlayerData playerData, ToastManager toastManager)
    {
        List<Achievement> achievements = GetAchievementInfo();

        for (int i = 0; i < achievements.Count; i++)
        {
            Achievement achievement = achievements[i];

            bool isCompletedPreviously = playerData.achievementProgress[i] >= 100f;

            switch (achievement.type)
            {
                case AchievementType.JUMP:
                    playerData.achievementProgress[i] = Mathf.Min(((float)playerData.totalJumps / achievement.amountToComplete) * 100, 100f);
                    break;
                case AchievementType.ATTEMPT:
                    playerData.achievementProgress[i] = Mathf.Min(((float)playerData.totalAttempts / achievement.amountToComplete) * 100, 100f);
                    break;
                case AchievementType.TIME:
                    playerData.achievementProgress[i] = Mathf.Min((playerData.totalTime / achievement.amountToComplete) * 100, 100f);
                    break;
                case AchievementType.LEVEL:
                    playerData.achievementProgress[i] = Mathf.Min(((float)(playerData.level - 1) / achievement.amountToComplete) * 100, 100f);
                    break;
                case AchievementType.SCORE:
                    playerData.achievementProgress[i] = Mathf.Min(((float)playerData.highScore / achievement.amountToComplete) * 100, 100f);
                    break;
                case AchievementType.LOGIN:
                    if (playerData.hasLoggedIn)
                    {
                        playerData.achievementProgress[i] = 100f;
                    }
                    else if (playerData.playerName.Length > 0 && playerData.playerId > 0 && playerData.playerAccessToken.Length > 0)
                    {
                        playerData.achievementProgress[i] = 100f;
                        playerData.hasLoggedIn = true;
                    }
                    break;
                case AchievementType.QUEST:
                    playerData.achievementProgress[i] = Mathf.Min(((float)playerData.totalQuestsCompletedOneGame / achievement.amountToComplete) * 100, 100f);
                    break;
                case AchievementType.COIN:
                    playerData.achievementProgress[i] = Mathf.Min(((float)(playerData.totalCoins - 100) / achievement.amountToComplete) * 100, 100f);
                    break;
                case AchievementType.TEXTURE:
                    playerData.achievementProgress[i] = Mathf.Min(((float)playerData.materialsOwned.Count(x => x == '1') / achievement.amountToComplete) * 100, 100f);
                    break;
            }

            if (playerData.achievementProgress[i] >= 100f && !isCompletedPreviously)
            {
                switch (achievement.rewardType)
                {
                    case AchievementRewardType.COIN:
                        playerData.coins += achievement.quantityOrIdx;
                        playerData.totalCoins += achievement.quantityOrIdx;
                        toastManager.ShowToast("Achievement Completed: " + achievement.title);
                        break;
                    case AchievementRewardType.TEXTURE:
                        playerData.materialsOwned = playerData.materialsOwned.Remove(achievement.quantityOrIdx, 1).Insert(achievement.quantityOrIdx, "1");
                        toastManager.ShowToast("Achievement Completed: " + achievement.title);
                        break;
                }
            }
        }

        playerData.SaveData();
    }

    public static List<Achievement> GetAchievementInfo()
    {
        return new List<Achievement>
        {
            new Achievement("Leap of Faith", "Jump 10 Times", AchievementRewardType.COIN, AchievementType.JUMP, 100, 10),
            new Achievement("Bouncing Around", "Jump 100 Times", AchievementRewardType.TEXTURE, AchievementType.JUMP, 11, 100),
            new Achievement("High Jumper", "Jump 1000 Times", AchievementRewardType.TEXTURE, AchievementType.JUMP, 38, 1000),
            new Achievement("Trial Runner", "Do 10 Attempts", AchievementRewardType.COIN, AchievementType.ATTEMPT, 150, 10),
            new Achievement("Persistent Parkourist", "Do 100 Attempts", AchievementRewardType.TEXTURE, AchievementType.ATTEMPT, 32, 100),
            new Achievement("Unstoppable Traceur", "Do 1000 Attempts", AchievementRewardType.COIN, AchievementType.ATTEMPT, 500, 1000),
            new Achievement("Marathon Runner", "Play for 12 Hours", AchievementRewardType.COIN, AchievementType.TIME, 250, 43200),
            new Achievement("Dedicated Runner", "Play for 5 Days", AchievementRewardType.TEXTURE, AchievementType.TIME, 43, 432000),
            new Achievement("Endurance Runner", "Play for 50 Days", AchievementRewardType.TEXTURE, AchievementType.TIME, 13, 4320000),
            new Achievement("Beginner Traceur", "Complete 10 Levels", AchievementRewardType.COIN, AchievementType.LEVEL, 150, 10),
            new Achievement("Skillful Jumper", "Complete 20 Levels", AchievementRewardType.TEXTURE, AchievementType.LEVEL, 22, 20),
            new Achievement("Experienced Traceur", "Complete 30 Levels", AchievementRewardType.COIN, AchievementType.LEVEL, 450, 30),
            new Achievement("Advanced Parkourist", "Complete 40 Levels", AchievementRewardType.TEXTURE, AchievementType.LEVEL, 2, 40),
            new Achievement("Master Traceur", "Complete 50 Levels", AchievementRewardType.TEXTURE, AchievementType.LEVEL, 18, 50),
            new Achievement("Legendary Runner", "Complete 100 Levels", AchievementRewardType.COIN, AchievementType.LEVEL, 900, 100),
            new Achievement("Parkour Prodigy", "Complete 150 Levels", AchievementRewardType.COIN, AchievementType.LEVEL, 1250, 150),
            new Achievement("Extreme Athlete", "Complete 200 Levels", AchievementRewardType.TEXTURE, AchievementType.LEVEL, 9, 200),
            new Achievement("Parkour Virtuoso", "Complete 250 Levels", AchievementRewardType.COIN, AchievementType.LEVEL, 1500, 250),
            new Achievement("Parkour Titan", "Complete 500 Levels", AchievementRewardType.COIN, AchievementType.LEVEL, 2000, 500),
            new Achievement("Parkour Legend", "Complete 1000 Levels", AchievementRewardType.TEXTURE, AchievementType.LEVEL, 49, 1000),
            new Achievement("Penny Pincher", "Collect 500 Coins", AchievementRewardType.TEXTURE, AchievementType.COIN, 4, 500),
            new Achievement("Coin Collector", "Collect 1000 Coins", AchievementRewardType.TEXTURE, AchievementType.COIN, 20, 1000),
            new Achievement("Money Maker", "Collect 2500 Coins", AchievementRewardType.COIN, AchievementType.COIN, 250, 2500),
            new Achievement("Gold Hoarder", "Collect 5000 Coins", AchievementRewardType.TEXTURE, AchievementType.COIN, 47, 5000),
            new Achievement("Treasure Hunter", "Collect 7500 Coins", AchievementRewardType.COIN, AchievementType.COIN, 750, 7500),
            new Achievement("Wealth Accumulator", "Collect 10000 Coins", AchievementRewardType.TEXTURE, AchievementType.COIN, 16, 10000),
            new Achievement("Coin Connoisseur", "Collect 20000 Coins", AchievementRewardType.TEXTURE, AchievementType.COIN, 34, 20000),
            new Achievement("Coin Tycoon", "Collect 30000 Coins", AchievementRewardType.COIN, AchievementType.COIN, 2000, 30000),
            new Achievement("Money Magnet", "Collect 40000 Coins", AchievementRewardType.COIN, AchievementType.COIN, 2500, 40000),
            new Achievement("Rich Runner", "Collect 50000 Coins", AchievementRewardType.TEXTURE, AchievementType.COIN, 36, 50000),
            new Achievement("Point Scorer", "Score 100 Points", AchievementRewardType.COIN, AchievementType.SCORE, 250, 100),
            new Achievement("Double the Fun", "Score 200 Points", AchievementRewardType.TEXTURE, AchievementType.SCORE, 45, 200),
            new Achievement("Triple Threat", "Score 300 Points", AchievementRewardType.TEXTURE, AchievementType.SCORE, 30, 300),
            new Achievement("Point Prodigy", "Score 500 Points", AchievementRewardType.TEXTURE, AchievementType.SCORE, 26, 500),
            new Achievement("Point Master", "Score 1000 Points", AchievementRewardType.COIN, AchievementType.SCORE, 1250, 1000),
            new Achievement("Texture Enthusiast", "Own 25 Textures", AchievementRewardType.COIN, AchievementType.TEXTURE, 250, 25),
            new Achievement("Texture Collector", "Own 35 Textures", AchievementRewardType.TEXTURE, AchievementType.TEXTURE, 41, 35),
            new Achievement("Texture Connoisseur", "Own 45 Textures", AchievementRewardType.COIN, AchievementType.TEXTURE, 750, 45),
            new Achievement("Newcomer", "Create an Account", AchievementRewardType.TEXTURE, AchievementType.LOGIN, 28, 0),
            new Achievement("Quest Master", "Complete 3 Quests at the Same Game", AchievementRewardType.COIN, AchievementType.QUEST, 500, 3)
        };
    }
}
