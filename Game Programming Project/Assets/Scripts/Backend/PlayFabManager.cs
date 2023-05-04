using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class PlayFabManager : MonoBehaviour
{
    [HideInInspector] public static PlayFabManager instance;

    [HideInInspector] public string username;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Register(string username, string email, string password)
    {

        var request = new RegisterPlayFabUserRequest
        {
            DisplayName = username,
            Username = username,
            Email = email,
            Password = password,
            RequireBothUsernameAndEmail = false,
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, result => Login(email, password), OnError);
    }

    public void Login(string email, string password)
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, result =>
        {
            username = result.InfoResultPayload.PlayerProfile.DisplayName;
            TemporaryData.instance.username = username;
            TemporaryData.instance.email = email;
            TemporaryData.instance.password = password;
            TemporaryData.instance.isLoggedIn = true;
            if(SceneManager.GetActiveScene().name == "Menu")
                SessionManager.instance.IsLoggedIn();
            GameAchievementsManager.instance.ProcessAchievements();
            DataManager.SaveData(TemporaryData.instance);
            ToastMessage.instance.Show("Successfully logged in");
        }, OnError);
    }

    public void ResetPassword(string email)
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = email,
            TitleId = "19158"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, result => ToastMessage.instance.Show("Password resetted"), OnError);
    }

    public void SetLeaderboard(string statisticName, int value)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> { new StatisticUpdate
            {
                StatisticName = statisticName,
                Value = value
            } }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, result =>
        {
            GetLeaderboard(statisticName);
            GetLeaderboardAround(statisticName);
        }, OnError);
    }

    public void GetLeaderboardAround(string statisticName)
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = statisticName,
            MaxResultsCount = 50
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, result => LeaderboardManager.instance.GetLeaderboardAroundName(statisticName, result), OnError);
    }

    public void GetLeaderboard(string statisticName)
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = statisticName,
            MaxResultsCount = 50
        };
        PlayFabClientAPI.GetLeaderboard(request, result => LeaderboardManager.instance.GetLeaderboardName(statisticName, result), OnError);
    }

    public void SaveData(Data data)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "Data", JsonConvert.SerializeObject(data) }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, result => ToastMessage.instance.Show("Successfully Saved Data"), OnError);
    }

    public void LoadData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            TemporaryData.instance.ProcessData(JsonConvert.DeserializeObject<Data>(result.Data["Data"].Value));
            ToastMessage.instance.Show("Successfully Loaded Data");
        }, OnError);
    }

    public void Logout()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        TemporaryData.instance.username = "User";
        TemporaryData.instance.email = "user@example.com";
        TemporaryData.instance.password = "User123";
        TemporaryData.instance.isLoggedIn = false;
        ToastMessage.instance.Show("Successfully Logged Out");
        SessionManager.instance.IsLoggedIn();
    }

    void OnError(PlayFabError error)
    {
        ToastMessage.instance.Show(error.ErrorMessage);
    }
}
