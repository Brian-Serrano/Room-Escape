using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    enum TypeTab
    {
        TOTAL_COINS, LEVEL, HIGH_SCORE
    }

    enum AroundTab
    {
        TOP_50, YOUR_POSITION
    }

    [Header("Buttons UI")]
    public Image coinsButtonImg;
    public Image levelButtonImg;
    public Image highScoreButtonImg;
    public Image top50ButtonImg;
    public Image yourPositionButtonImg;

    [Header("Leaderboard UI")]
    public GameObject leaderboardItemPrefab;
    public Transform leaderboardContainer;

    [Header("Others")]
    public GameObject spinner;
    public GameObject leaderboardScrollView;
    public TMP_Text errorTxt;
    public ConfigHandler configHandler;
    public GameObject background;
    public Animator crossfade;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Audio Source")]
    public AudioSource buttonClickSfx;

    private PlayerData playerData;
    private RoomEscapeHTTPClient client;
    private TypeTab selectedTypeTab = TypeTab.LEVEL;
    private AroundTab selectedAroundTab = AroundTab.TOP_50;

    private Dictionary<(TypeTab, AroundTab), LeaderboardResponse> cache;

    private Color gray = new Color(0.45f, 0.45f, 0.45f, 1f);

    private void Awake()
    {
        playerData = PlayerData.LoadData();
        client = RoomEscapeHTTPClient.GetInstance();
        cache = new Dictionary<(TypeTab, AroundTab), LeaderboardResponse>();

        SelectTypeTab();
        SelectAroundTab();

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            spinner.SetActive(true);
            leaderboardScrollView.SetActive(false);
            errorTxt.gameObject.SetActive(false);

            LeaderboardRequest request = new LeaderboardRequest(playerData.totalCoins, playerData.level, playerData.highScore);

            client.GetPlayerRoutes().SaveLeaderboardData(playerData.playerToken, request, response =>
            {
                spinner.SetActive(false);
                leaderboardScrollView.SetActive(true);

                Add50LeaderboardEntries();

                GetLeaderboardData();
            }, error =>
            {
                spinner.SetActive(false);
                errorTxt.gameObject.SetActive(true);

                errorTxt.text = error.details.Truncate(60);
            });
        }
        else
        {
            spinner.SetActive(false);
            errorTxt.gameObject.SetActive(true);
            leaderboardScrollView.SetActive(false);

            errorTxt.text = "No Internet Connection";
        }
    }

    private void Start()
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(playerData.musicVolume) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(playerData.sfxVolume) * 20);

        SetMaterials(background.GetComponentsInChildren<IMaterialController>());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }

    private void SetMaterials(IMaterialController[] matControllers)
    {
        foreach (IMaterialController matController in matControllers)
        {
            matController.SetMaterial(configHandler.materials, playerData.materialsSelected);
        }
    }

    private void Add50LeaderboardEntries()
    {
        for (int i = 0; i < 50; i++)
        {
            Instantiate(leaderboardItemPrefab, leaderboardContainer);
        }
    }

    private void GetLeaderboardData()
    {
        string type = selectedTypeTab switch
        {
            TypeTab.LEVEL => "level",
            TypeTab.TOTAL_COINS => "coins",
            TypeTab.HIGH_SCORE => "high_score",
            _ => "level"
        };

        string around = selectedAroundTab == AroundTab.YOUR_POSITION ? "true" : "false";

        var key = (selectedTypeTab, selectedAroundTab);

        if (!cache.ContainsKey(key))
        {
            GetLeaderboardDataFromServer(type, around, data =>
            {
                cache[key] = data;
                UpdateLeaderboardEntry(data);
            });
        }
        else
        {
            UpdateLeaderboardEntry(cache[key]);
        }
    }

    private void UpdateLeaderboardEntry(LeaderboardResponse leaderboard)
    {
        int count = Mathf.Min(leaderboardContainer.childCount, leaderboard.leaderboard.Count);

        for (int i = 0; i < count; i++)
        {
            Transform row = leaderboardContainer.GetChild(i);

            TMP_Text rankTxt = row.GetChild(0).GetComponent<TMP_Text>();
            TMP_Text playerNameTxt = row.GetChild(1).GetComponent<TMP_Text>();
            TMP_Text amountTxt = row.GetChild(2).GetComponent<TMP_Text>();

            if (leaderboard.leaderboard[i].playerId == playerData.playerId)
            {
                row.GetComponent<Image>().color = gray;

                rankTxt.fontStyle = FontStyles.Bold;
                playerNameTxt.fontStyle = FontStyles.Bold;
                amountTxt.fontStyle = FontStyles.Bold;

                rankTxt.text = leaderboard.leaderboard[i].rank.ToString();
                playerNameTxt.text = leaderboard.leaderboard[i].playerName;
                amountTxt.text = leaderboard.leaderboard[i].amount.ToString();
            }
            else
            {
                row.GetComponent<Image>().color = Color.white;

                rankTxt.fontStyle = FontStyles.Normal;
                playerNameTxt.fontStyle = FontStyles.Normal;
                amountTxt.fontStyle = FontStyles.Normal;

                rankTxt.text = leaderboard.leaderboard[i].rank.ToString();
                playerNameTxt.text = leaderboard.leaderboard[i].playerName;
                amountTxt.text = leaderboard.leaderboard[i].amount.ToString();
            }
        }
    }

    private void GetLeaderboardDataFromServer(string type, string around, Action<LeaderboardResponse> data)
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            spinner.SetActive(true);
            leaderboardScrollView.SetActive(false);
            errorTxt.gameObject.SetActive(false);

            client.GetPlayerRoutes().GetLeaderboard(playerData.playerToken, $"?type={type}&around={around}", response =>
            {
                spinner.SetActive(false);
                leaderboardScrollView.SetActive(true);

                data?.Invoke(response);
            }, error =>
            {
                spinner.SetActive(false);
                errorTxt.gameObject.SetActive(true);

                errorTxt.text = error.details.Truncate(60);
            });
        }
        else
        {
            spinner.SetActive(false);
            errorTxt.gameObject.SetActive(true);
            leaderboardScrollView.SetActive(false);

            errorTxt.text = "No Internet Connection";
        }
    }

    public void SwitchToCoinTab()
    {
        selectedTypeTab = TypeTab.TOTAL_COINS;
        buttonClickSfx.Play();

        SelectTypeTab();
        GetLeaderboardData();
    }

    public void SwitchToLevelTab()
    {
        selectedTypeTab = TypeTab.LEVEL;
        buttonClickSfx.Play();

        SelectTypeTab();
        GetLeaderboardData();
    }

    public void SwitchToHighScoreTab()
    {
        selectedTypeTab = TypeTab.HIGH_SCORE;
        buttonClickSfx.Play();

        SelectTypeTab();
        GetLeaderboardData();
    }

    public void SwitchToTop50Tab()
    {
        selectedAroundTab = AroundTab.TOP_50;
        buttonClickSfx.Play();

        SelectAroundTab();
        GetLeaderboardData();
    }

    public void SwitchToYourPositionTab()
    {
        selectedAroundTab = AroundTab.YOUR_POSITION;
        buttonClickSfx.Play();

        SelectAroundTab();
        GetLeaderboardData();
    }

    private void SelectTypeTab()
    {
        switch (selectedTypeTab)
        {
            case TypeTab.TOTAL_COINS:
                coinsButtonImg.color = gray;
                levelButtonImg.color = Color.white;
                highScoreButtonImg.color = Color.white;
                break;
            case TypeTab.LEVEL:
                coinsButtonImg.color = Color.white;
                levelButtonImg.color = gray;
                highScoreButtonImg.color = Color.white;
                break;
            case TypeTab.HIGH_SCORE:
                coinsButtonImg.color = Color.white;
                levelButtonImg.color = Color.white;
                highScoreButtonImg.color = gray;
                break;
        }
    }

    private void SelectAroundTab()
    {
        switch (selectedAroundTab)
        {
            case AroundTab.TOP_50:
                top50ButtonImg.color = gray;
                yourPositionButtonImg.color = Color.white;
                break;
            case AroundTab.YOUR_POSITION:
                top50ButtonImg.color = Color.white;
                yourPositionButtonImg.color = gray;
                break;
        }
    }

    public void Back()
    {
        buttonClickSfx.Play();
        StartCoroutine(SwitchScene("Submenu"));
    }

    private IEnumerator SwitchScene(string name)
    {
        crossfade.SetBool("isOpen", true);
        yield return new WaitForSecondsRealtime(0.3f);
        SceneManager.LoadScene(name);
    }
}
