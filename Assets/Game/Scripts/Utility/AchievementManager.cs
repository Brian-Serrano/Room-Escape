using System.Linq;
using UnityEngine;

public static class AchievementManager
{
    public static void CheckAchievements(AchievementData achievements, PlayerData playerData, ToastManager toastManager)
    {
        foreach (Achievement achievement in achievements.achievements)
        {
            bool isCompletedPreviously = achievement.progress >= 100f;

            switch (achievement.type)
            {
                case AchievementType.JUMP:
                    achievement.progress = Mathf.Min(((float)playerData.totalJumps / achievement.amountToComplete) * 100, 100f);
                    break;
                case AchievementType.ATTEMPT:
                    achievement.progress = Mathf.Min(((float)playerData.totalAttempts / achievement.amountToComplete) * 100, 100f);
                    break;
                case AchievementType.TIME:
                    achievement.progress = Mathf.Min((playerData.totalTime / achievement.amountToComplete) * 100, 100f);
                    break;
                case AchievementType.LEVEL:
                    achievement.progress = Mathf.Min(((float)(playerData.level - 1) / achievement.amountToComplete) * 100, 100f);
                    break;
                case AchievementType.SCORE:
                    achievement.progress = Mathf.Min(((float)playerData.highScore / achievement.amountToComplete) * 100, 100f);
                    break;
                case AchievementType.LOGIN:
                    if (playerData.hasLoggedIn)
                    {
                        achievement.progress = 100f;
                    }
                    else if (playerData.playerName.Length > 0 && playerData.playerId > 0 && playerData.playerAccessToken.Length > 0)
                    {
                        achievement.progress = 100f;
                        playerData.hasLoggedIn = true;
                    }
                    break;
                case AchievementType.QUEST:
                    achievement.progress = Mathf.Min(((float)playerData.totalQuestsCompletedOneGame / achievement.amountToComplete) * 100, 100f);
                    break;
                case AchievementType.COIN:
                    achievement.progress = Mathf.Min(((float)(playerData.totalCoins - 100) / achievement.amountToComplete) * 100, 100f);
                    break;
                case AchievementType.TEXTURE:
                    achievement.progress = Mathf.Min(((float)playerData.materialsOwned.Count(x => x == '1') / achievement.amountToComplete) * 100, 100f);
                    break;
            }

            if (achievement.progress >= 100f && !isCompletedPreviously)
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

        achievements.SaveData();
        playerData.SaveData();
    }
}
