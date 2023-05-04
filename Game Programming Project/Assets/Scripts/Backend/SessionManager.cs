using UnityEngine;
using PlayFab;
using TMPro;

public class SessionManager : MonoBehaviour
{
    [HideInInspector] public static SessionManager instance;

    public TMP_Text username;

    void Start()
    {
        instance = this;
        IsLoggedIn();
    }

    public void IsLoggedIn()
    {
        Invoke("CheckLogin", 0.01f);
    }

    private void CheckLogin()
    {
        if (PlayFabClientAPI.IsClientLoggedIn() && Application.internetReachability != NetworkReachability.NotReachable)
        {
            username.text = PlayFabManager.instance.username;
        }
        else
        {
            username.text = "User";
        }
    }
}
