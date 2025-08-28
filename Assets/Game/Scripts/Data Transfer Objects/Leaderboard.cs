using Newtonsoft.Json;
using UnityEngine;

public class Leaderboard
{
    [JsonProperty("rank")]
    public int rank;

    [JsonProperty("player_id")]
    public int playerId;

    [JsonProperty("player_name")]
    public string playerName;

    [JsonProperty("amount")]
    public int amount;
}
