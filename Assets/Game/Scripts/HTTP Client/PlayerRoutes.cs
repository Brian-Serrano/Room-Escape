using CI.HttpClient;
using System;
using System.IO;
using UnityEngine;

public class PlayerRoutes
{
    private static PlayerRoutes instance;
    private RoomEscapeHTTPClient client;

    public static PlayerRoutes GetInstance(RoomEscapeHTTPClient client)
    {
        instance ??= new PlayerRoutes(client);

        return instance;
    }

    private PlayerRoutes(RoomEscapeHTTPClient client)
    {
        this.client = client;
    }

    public void SavePlayerData(string token, Action<SuccessMessage> responseCallback, Action<ErrorResponse> errorCallback, Action<int> progressCallback)
    {
        string url = client.baseUrl + "re_player_routes/save_player_data";
        string playerDataPath = Path.Combine(Application.persistentDataPath, "player_data.re");
        string achievementDataPath = Path.Combine(Application.persistentDataPath, "achievement.re");

        byte[] compressedFile = FileCompressor.CompressFilesAndFolders(playerDataPath, achievementDataPath);

        MultipartFormDataContent multipartFormData = new MultipartFormDataContent
        {
            { new ByteArrayContent(compressedFile, "application/zip"), "data", "player_data.playerdata" }
        };

        HttpRequestMessage message = new HttpRequestMessage
        {
            Uri = new Uri(url),
            Method = HttpAction.Post,
            Content = multipartFormData
        };

        message.Headers.Add("Authorization", token);

        client.client.Send(message, HttpCompletionOption.StreamResponseContent, response => {

            if (response.PercentageComplete == 100 && response.IsSuccessStatusCode)
            {
                responseCallback?.Invoke(response.ReadAsJson<SuccessMessage>());
                return;
            }

            if (!response.IsSuccessStatusCode)
            {
                if (!response.HasContent)
                {
                    errorCallback?.Invoke(new ErrorResponse("Server unavailable", "Server not running."));
                    return;
                }

                errorCallback?.Invoke(response.ReadAsJson<ErrorResponse>());
                return;
            }
        }, upload =>
        {
            progressCallback?.Invoke(upload.PercentageComplete);
        });
    }

    public void LoadPlayerData(string token, Action<SuccessMessage> responseCallback, Action<ErrorResponse> errorCallback, Action<int> progressCallback)
    {
        string url = client.baseUrl + "re_player_routes/load_player_data";

        HttpRequestMessage message = new HttpRequestMessage
        {
            Uri = new Uri(url),
            Method = HttpAction.Get
        };

        message.Headers.Add("Authorization", token);

        byte[] data = null;

        client.client.Send(message, HttpCompletionOption.StreamResponseContent, response => {

            data ??= new byte[response.ContentLength];

            response.ReadAsByteArray().CopyTo(data, response.TotalContentRead - response.ContentReadThisRound);

            if (response.PercentageComplete == 100 && response.IsSuccessStatusCode)
            {
                FileCompressor.ExtractFilesAndFolders(data, Application.persistentDataPath);
                responseCallback?.Invoke(new SuccessMessage { message = "Your data has been successfully restored." });
                return;
            }

            if (!response.IsSuccessStatusCode)
            {
                if (!response.HasContent)
                {
                    errorCallback?.Invoke(new ErrorResponse("Server unavailable", "Server not running."));
                    return;
                }

                errorCallback?.Invoke(response.ReadAsJson<ErrorResponse>());
                return;
            }

            progressCallback?.Invoke(response.PercentageComplete);
        });
    }

    public void SaveLeaderboardData(string token, LeaderboardRequest requestData, Action<SuccessMessage> responseCallback, Action<ErrorResponse> errorCallback)
    {
        string url = client.baseUrl + "re_player_routes/save_leaderboard_data";

        HttpRequestMessage message = new HttpRequestMessage
        {
            Uri = new Uri(url),
            Method = HttpAction.Post,
            Content = StringContent.FromObject(requestData)
        };

        message.Headers.Add("Authorization", token);

        client.client.Send(message, HttpCompletionOption.AllResponseContent, response => {

            if (response.IsSuccessStatusCode)
            {
                responseCallback?.Invoke(response.ReadAsJson<SuccessMessage>());
                return;
            }

            if (!response.IsSuccessStatusCode)
            {
                if (!response.HasContent)
                {
                    errorCallback?.Invoke(new ErrorResponse("Server unavailable", "Server not running."));
                    return;
                }

                errorCallback?.Invoke(response.ReadAsJson<ErrorResponse>());
                return;
            }
        });
    }

    public void GetLeaderboard(string token, string queryString, Action<LeaderboardResponse> responseCallback, Action<ErrorResponse> errorCallback)
    {
        string url = client.baseUrl + "re_player_routes/get_leaderboard" + queryString;

        HttpRequestMessage message = new HttpRequestMessage
        {
            Uri = new Uri(url),
            Method = HttpAction.Get
        };

        message.Headers.Add("Authorization", token);

        client.client.Send(message, HttpCompletionOption.AllResponseContent, response =>
        {
            if (response.IsSuccessStatusCode)
            {
                responseCallback?.Invoke(response.ReadAsJson<LeaderboardResponse>());
                return;
            }

            if (!response.IsSuccessStatusCode)
            {
                if (!response.HasContent)
                {
                    errorCallback?.Invoke(new ErrorResponse("Server unavailable", "Server not running."));
                    return;
                }

                errorCallback?.Invoke(response.ReadAsJson<ErrorResponse>());
                return;
            }
        });
    }
}
