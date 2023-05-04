using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    [HideInInspector] public static LeaderboardManager instance;

    public GameObject[] data;
    public GameObject error;
    public GameObject leaderboard;
    public GameObject loading;

    private string[] lbName;
    private int methodsCalled = 0;

    void Start()
    {
        instance = this;

        lbName = new string[]
        {
            "Level", "Coin", "Highscore"
        };

        if(Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                PlayFabManager.instance.SetLeaderboard(lbName[0], TemporaryData.instance.level);
                PlayFabManager.instance.SetLeaderboard(lbName[1], TemporaryData.instance.coins);
                PlayFabManager.instance.SetLeaderboard(lbName[2], TemporaryData.instance.highscore);
            }
            else
            {
                ShowError("You are not logged in");
            }
        }
        else
        {
            ShowError("No Internet Connection");
        }

    }

    public void GetLeaderboardAroundName(string leaderboardName, GetLeaderboardAroundPlayerResult result)
    {
        for(int i = 0; i < lbName.Length; i++)
        {
            if(string.Equals(leaderboardName, lbName[i]))
            {
                ShowLeaderboardAround(data[(i*2)+1], result);
            }
        }
    }

    public void GetLeaderboardName(string leaderboardName, GetLeaderboardResult result)
    {
        for (int i = 0; i < lbName.Length; i++)
        {
            if (string.Equals(leaderboardName, lbName[i]))
            {
                ShowLeaderboard(data[i*2], result);
            }
        }
    }

    void ShowLeaderboardAround(GameObject data, GetLeaderboardAroundPlayerResult result)
    {
        for (int i = 0; i < result.Leaderboard.Count; i++)
        {
            Transform panel = data.transform.GetChild(i);
            panel.GetChild(0).GetComponent<TMP_Text>().text = (result.Leaderboard[i].Position + 1).ToString();
            panel.GetChild(1).GetComponent<TMP_Text>().text = result.Leaderboard[i].DisplayName.ToString();
            panel.GetChild(2).GetComponent<TMP_Text>().text = result.Leaderboard[i].StatValue.ToString();
        }

        CheckCalledMethods();
    }

    void ShowLeaderboard(GameObject data, GetLeaderboardResult result)
    {
        for (int i = 0; i < result.Leaderboard.Count; i++)
        {
            Transform panel = data.transform.GetChild(i);
            panel.GetChild(0).GetComponent<TMP_Text>().text = (result.Leaderboard[i].Position + 1).ToString();
            panel.GetChild(1).GetComponent<TMP_Text>().text = result.Leaderboard[i].DisplayName.ToString();
            panel.GetChild(2).GetComponent<TMP_Text>().text = result.Leaderboard[i].StatValue.ToString();
        }

        CheckCalledMethods();
    }

    public void ShowError(string message)
    {
        error.SetActive(true);
        error.GetComponent<TMP_Text>().text = message;
        loading.SetActive(false);
        ToastMessage.instance.Show(message);
    }

    public void ShowLeaderboard()
    {
        leaderboard.SetActive(true);
        loading.SetActive(false);
    }

    public void CheckCalledMethods()
    {
        methodsCalled++;
        if(methodsCalled == 6)
        {
            ShowLeaderboard();
        }
    }
}
