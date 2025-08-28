using CI.HttpClient;
using UnityEngine;

public class RoomEscapeHTTPClient
{
    private static RoomEscapeHTTPClient instance;

    public HttpClient client;
    public string baseUrl = "http://127.0.0.1:5000/";

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
