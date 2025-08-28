using Newtonsoft.Json;
using UnityEngine;

public class LeaderboardRequest
{
    [JsonProperty("total_coins")]
    public int totalCoins;

    [JsonProperty("level")]
    public int level;

    [JsonProperty("high_score")]
    public int highScore;

    public LeaderboardRequest(int totalCoins, int level, int highScore)
    {
        this.totalCoins = totalCoins;
        this.level = level;
        this.highScore = highScore;
    }
}
