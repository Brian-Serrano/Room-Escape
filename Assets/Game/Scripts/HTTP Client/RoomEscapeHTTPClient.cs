using CI.HttpClient;
using UnityEngine;

public class RoomEscapeHTTPClient
{
    private static RoomEscapeHTTPClient instance;

    public HttpClient client;
    public string baseUrl = "https://briser-games-server.onrender.com/";

    public static RoomEscapeHTTPClient GetInstance()
    {
        instance ??= new RoomEscapeHTTPClient();

        return instance;
    }

    private RoomEscapeHTTPClient()
    {
        client = new HttpClient();
    }

    public AuthorizationRoutes GetAuthorizationRoutes()
    {
        return AuthorizationRoutes.GetInstance(this);
    }

    public PlayerRoutes GetPlayerRoutes()
    {
        return PlayerRoutes.GetInstance(this);
    }
}
