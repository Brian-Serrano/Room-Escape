using UnityEngine;

[System.Serializable]
public class Achievement
{
    public string title;
    public string description;
    public AchievementRewardType rewardType;
    public AchievementType type;
    public int quantityOrIdx;
    public int amountToComplete;

    public Achievement(string title, string description, AchievementRewardType rewardType, AchievementType type, int quantityOrIdx, int amountToComplete)
    {
        this.title = title;
        this.description = description;
        this.rewardType = rewardType;
        this.type = type;
        this.quantityOrIdx = quantityOrIdx;
        this.amountToComplete = amountToComplete;
    }
}
