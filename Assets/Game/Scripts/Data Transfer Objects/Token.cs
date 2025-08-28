using Newtonsoft.Json;
using UnityEngine;

public class Token
{
    [JsonProperty("token")]
    public string token;

    [JsonProperty("player_id")]
    public int playerId;
}