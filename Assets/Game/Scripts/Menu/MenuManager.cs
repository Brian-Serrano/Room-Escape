using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Achievements UI")]
    public Transform achievementsContainer;
    public GameObject achievementItemPrefab;
    public Transform achievementPanel;

    [Header("Stats UI")]
    public Transform statsPanel;
    public TMP_Text completedLevelsTxt;
    public TMP_Text highscoreTxt;
    public TMP_Text statsCoinsTxt;
    public TMP_Text totalCoinsTxt;
    public TMP_Text questsCompletedTxt;
    public TMP_Text achievementsCompletedTxt;
    public TMP_Text totalJumpsTxt;
    public TMP_Text totalTimeTxt;
    public TMP_Text totalAttemptsTxt;
    public TMP_Text texturesOwnedTxt;

    [Header("Settings UI")]
    public Transform settingsPanel;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider sensitivitySlider;
    public Button logoutButton;
    public Button saveButton;
    public Button loadButton;

    [Header("Help UI")]
    public Transform helpPanel;
    public TMP_Text helpText;
    public RectTransform helpTextParent;

    [Header("Confirm UI")]
    public Transform confirmPanel;
    public TMP_Text confirmPanelText;
    public Button confirmPanelOkButton;

    [Header("Background Components")]
    public Transform obstaclesContainer;
    public Camera mainCamera;

    [Header("Config Handler")]
    public ConfigHandler configHandler;

    [Header("Others")]
    public TMP_Text coinsTxt;
    public TMP_Text userTxt;
    public GameObject spinnerContainer;
    public Animator crossfade;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Audio Source")]
    public AudioSource buttonClickSfx;

    private AchievementData achievements;
    private PlayerData playerData;
    private ToastManager toastManager;
    private RoomEscapeHTTPClient client;
    private BannerAdManager bannerAdManager;
    private IAPV5Manager iAPV5Manager;

    private List<int> levelsQuest = new List<int>() { 2, 4, 6 };
    private List<int> attemptsQuest = new List<int>() { 5, 10, 15 };
    private List<int> coinsQuest = new List<int>() { 20, 40, 60 };

    private void Awake()
    {
        achievements = AchievementData.LoadData();
        playerData = PlayerData.LoadData();
        toastManager = GetComponent<ToastManager>();
        client = RoomEscapeHTTPClient.GetInstance();
        bannerAdManager = BannerAdManager.GetInstance();
        iAPV5Manager = IAPV5Manager.GetInstance();

        coinsTxt.text = playerData.coins.ToString();

        CheckLoginState();

        List<int> nums1 = new List<int>() { 5, 8, 11 };
        List<int> nums2 = new List<int>() { 14, 18, 22, 26 };

        List<List<float>> obstaclesToCreate = RoomEscapeUtils.GetObstacleSpawnPoints();

        int randomNumber = 6;

        GameObject roomStart = Instantiate(configHandler.room, new Vector3(0, 6, -16), Quaternion.identity, obstaclesContainer);
        GameObject roomEnd = Instantiate(configHandler.room, new Vector3(0, 6, (randomNumber * 20) - 4), Quaternion.Euler(0, 180, 0), obstaclesContainer);

        SetMaterials(roomStart.GetComponentsInChildren<IMaterialController>());
        SetMaterials(roomEnd.GetComponentsInChildren<IMaterialController>());

        for (int i = 0; i < randomNumber; i++)
        {
            Vector3 spawnOffset = i * 20 * Vector3.forward;

            List<List<int>> obstacles = RoomEscapeUtils.CreateObstacleSpawns(nums1, nums2);

            GameObject roomSlice = Instantiate(configHandler.structures[7], new Vector3(0, 6, 0) + spawnOffset, Quaternion.identity, obstaclesContainer);
            SetMaterials(roomSlice.GetComponentsInChildren<IMaterialController>());

            List<int> randomObstacles = obstacles[UnityEngine.Random.Range(0, obstacles.Count)];

            foreach (int obstacle in randomObstacles)
            {
                List<float> idx = obstaclesToCreate[obstacle - 1];

                for (int j = 0; j < idx.Count; j += 4)
                {
                    GameObject structure = Instantiate(configHandler.structures[(int)idx[j]], new Vector3(idx[j + 1], idx[j + 2], idx[j + 3]) + spawnOffset, Quaternion.identity, obstaclesContainer);
                    SetMaterials(structure.GetComponentsInChildren<IMaterialController>());
                }
            }
        }

        Debug.Log(playerData.playerRefreshToken);
    }

    private void SetMaterials(IMaterialController[] matControllers)
    {
        foreach (IMaterialController matController in matControllers)
        {
            matController.SetMaterial(configHandler.materials, playerData.materialsSelected);
        }
    }

    private void Start()
    {
        UpdateMusicVolume();
        UpdateSfxVolume();

        CheckForNewQuest();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!spinnerContainer.activeSelf)
            {
                if (achievementPanel.gameObject.activeSelf)
                {
                    CloseAchievementsPanel();
                }
                else if (statsPanel.gameObject.activeSelf)
                {
                    CloseStatsPanel();
                }
                else if (settingsPanel.gameObject.activeSelf && !confirmPanel.gameObject.activeSelf)
                {
                    CloseSettingsPanel();
                }
                else if (helpPanel.gameObject.activeSelf)
                {
                    CloseHelpPanel();
                }
                else if (confirmPanel.gameObject.activeSelf)
                {
                    CloseConfirmPanel();
                }
                else
                {
                    Quit();
                }
            }
        }

        mainCamera.transform.Rotate(15 * Time.deltaTime * Vector3.up);
    }

    public void PlayButton()
    {
        buttonClickSfx.Play();
        StartCoroutine(SwitchScene("Play"));
    }

    public void ThemeButton()
    {
        buttonClickSfx.Play();
        StartCoroutine(SwitchScene("Theme"));
    }

    public void SubmenuButton()
    {
        buttonClickSfx.Play();
        StartCoroutine(SwitchScene("Submenu"));
    }

    public void Quit()
    {
        buttonClickSfx.Play();
        Application.Quit();
    }

    public void OpenAchievementsPanel()
    {
        if (achievementsContainer.childCount == 0)
        {
            foreach (Achievement achievement in achievements.achievements)
            {
                GameObject instance = Instantiate(achievementItemPrefab, achievementsContainer);
                Transform obj = instance.transform;

                obj.GetChild(2).GetComponent<TMP_Text>().text = achievement.title;
                obj.GetChild(3).GetComponent<TMP_Text>().text = achievement.description;

                if (achievement.rewardType == AchievementRewardType.COIN)
                {
                    Transform rewardCoin = obj.GetChild(0).GetChild(0);

                    rewardCoin.gameObject.SetActive(true);
                    obj.GetChild(0).GetChild(1).gameObject.SetActive(false);

                    rewardCoin.GetComponentInChildren<TMP_Text>().text = "+" + achievement.quantityOrIdx;
                }
                else
                {
                    Transform texture = obj.GetChild(0).GetChild(1);

                    obj.GetChild(0).GetChild(0).gameObject.SetActive(false);
                    texture.gameObject.SetActive(true);

                    texture.GetComponent<Image>().sprite = configHandler.sprites[achievement.quantityOrIdx];
                }

                UpdateAchievementProgress(obj, achievement);
            }
        }
        else
        {
            for (int i = 0; i < achievementsContainer.childCount; i++)
            {
                UpdateAchievementProgress(achievementsContainer.GetChild(i), achievements.achievements[i]);
            }
        }
        
        achievementPanel.gameObject.SetActive(true);
        buttonClickSfx.Play();
        achievementPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", true);
    }

    private void UpdateAchievementProgress(Transform obj, Achievement achievement)
    {
        if (achievement.progress >= 100)
        {
            obj.GetChild(1).GetChild(0).gameObject.SetActive(false);
            obj.GetChild(1).GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            Transform progressText = obj.GetChild(1).GetChild(0);

            progressText.gameObject.SetActive(true);
            obj.GetChild(1).GetChild(1).gameObject.SetActive(false);

            progressText.GetComponent<TMP_Text>().text = Mathf.FloorToInt(achievement.progress) + "%";
        }
    }

    public void CloseAchievementsPanel()
    {
        achievementPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", false);
        buttonClickSfx.Play();
        StartCoroutine(DelayedPanelClose(achievementPanel));
    }

    public void OpenStatsPanel()
    {
        completedLevelsTxt.text = (playerData.level - 1).ToString();
        highscoreTxt.text = playerData.highScore.ToString();
        statsCoinsTxt.text = playerData.coins.ToString();
        totalCoinsTxt.text = playerData.totalCoins.ToString();
        questsCompletedTxt.text = playerData.totalQuestsCompleted.ToString();
        achievementsCompletedTxt.text = achievements.achievements.Count(x => x.progress >= 100f).ToString();
        totalJumpsTxt.text = playerData.totalJumps.ToString();
        totalTimeTxt.text = Mathf.RoundToInt(playerData.totalTime).ToString();
        totalAttemptsTxt.text = playerData.totalAttempts.ToString();
        texturesOwnedTxt.text = playerData.materialsOwned.Count(x => x == '1').ToString();

        statsPanel.gameObject.SetActive(true);
        buttonClickSfx.Play();
        statsPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", true);
    }

    public void CloseStatsPanel()
    {
        statsPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", false);
        buttonClickSfx.Play();
        StartCoroutine(DelayedPanelClose(statsPanel));
    }

    public void OpenSettingsPanel()
    {
        musicSlider.value = playerData.musicVolume;
        sfxSlider.value = playerData.sfxVolume;
        sensitivitySlider.value = playerData.sensitivity;

        settingsPanel.gameObject.SetActive(true);
        buttonClickSfx.Play();
        settingsPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", true);
    }

    public void CloseSettingsPanel()
    {
        settingsPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", false);
        buttonClickSfx.Play();
        StartCoroutine(DelayedPanelClose(settingsPanel));

        playerData.SaveData();
    }

    public void OpenHelpPanel()
    {
        helpPanel.gameObject.SetActive(true);
        buttonClickSfx.Play();
        helpPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", true);

        helpText.text = "Drag the joystick to move player.\r\nPress the arrow button on the right to jump.\r\nHold the circle button on the right to sprint.\r\nTo open door, get the player close to the switch, aim at it, and press the left mouse button.\r\nThe bar at the top shows your progress throughout the level.\r\nThe number below the bar shows the time remaining to complete the level.\r\nAvoid touching the spikes.\r\nThere is a light between two doors. The light on the door should be green, otherwise there will be a deduction of 10 seconds from the time in classic mode or deduction of score and life decrease in infinite mode.\r\nPress the surrender if wanted to end the level.";
        helpTextParent.sizeDelta = new Vector2(helpTextParent.sizeDelta.x, 450);
    }

    public void CloseHelpPanel()
    {
        helpPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", false);
        buttonClickSfx.Play();
        StartCoroutine(DelayedPanelClose(helpPanel));
    }

    private IEnumerator DelayedPanelClose(Transform panel)
    {
        yield return new WaitForSecondsRealtime(0.2f);
        panel.gameObject.SetActive(false);
    }

    private void UpdateMusicVolume()
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(playerData.musicVolume) * 20);
    }

    private void UpdateSfxVolume()
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(playerData.sfxVolume) * 20);
    }

    public void OnMusicVolumeChange(float value)
    {
        playerData.musicVolume = value;
        UpdateMusicVolume();
    }

    public void OnSfxVolumeChange(float value)
    {
        playerData.sfxVolume = value;
        UpdateSfxVolume();
    }

    public void OnSensitivityChange(float value)
    {
        playerData.sensitivity = value;
    }

    private void CheckLoginState()
    {
        if (playerData.playerName.Length > 0 && playerData.playerId > 0)
        {
            userTxt.text = playerData.playerName;

            logoutButton.interactable = true;
            saveButton.interactable = true;
            loadButton.interactable = true;
        }
        else
        {
            userTxt.text = "User";

            logoutButton.interactable = false;
            saveButton.interactable = false;
            loadButton.interactable = false;
        }
    }

    private void Logout()
    {
        CloseConfirmPanel();

        playerData.playerAccessToken = "";
        playerData.playerRefreshToken = "";
        playerData.playerId = 0;
        playerData.playerName = "";

        playerData.SaveData();

        CheckLoginState();
    }

    private void SaveDataToServer()
    {
        CloseConfirmPanel();

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            spinnerContainer.SetActive(true);

            saveButton.interactable = false;
            loadButton.interactable = false;

            if (JwtHelper.IsExpired(playerData.playerAccessToken))
            {
                RefreshToken refreshToken = new RefreshToken(playerData.playerRefreshToken);

                client.GetAuthorizationRoutes().Refresh(refreshToken, response =>
                {
                    playerData.playerAccessToken = response.accessToken;
                    playerData.playerRefreshToken = response.refreshToken;

                    playerData.SaveData();

                    Debug.Log("New access token issued");

                    SavePlayerData();
                }, error =>
                {
                    spinnerContainer.SetActive(false);

                    toastManager.ShowToast(error.details.Truncate(60));

                    saveButton.interactable = true;
                    loadButton.interactable = true;
                });
            }
            else
            {
                SavePlayerData();
            }

            void SavePlayerData()
            {
                client.GetPlayerRoutes().SavePlayerData(playerData.playerAccessToken, response =>
                {
                    spinnerContainer.SetActive(false);

                    toastManager.ShowToast(response.message.Truncate(60));

                    saveButton.interactable = true;
                    loadButton.interactable = true;
                }, error =>
                {
                    spinnerContainer.SetActive(false);

                    toastManager.ShowToast(error.details.Truncate(60));

                    saveButton.interactable = true;
                    loadButton.interactable = true;
                }, progress => { });
            }
        }
        else
        {
            toastManager.ShowToast("No Internet Connection");
        }
    }

    private void LoadDataFromServer()
    {
        CloseConfirmPanel();

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            spinnerContainer.SetActive(true);

            saveButton.interactable = false;
            loadButton.interactable = false;

            if (JwtHelper.IsExpired(playerData.playerAccessToken))
            {
                RefreshToken refreshToken = new RefreshToken(playerData.playerRefreshToken);

                client.GetAuthorizationRoutes().Refresh(refreshToken, response =>
                {
                    playerData.playerAccessToken = response.accessToken;
                    playerData.playerRefreshToken = response.refreshToken;

                    playerData.SaveData();

                    Debug.Log("New access token issued");

                    LoadPlayerData();
                }, error =>
                {
                    spinnerContainer.SetActive(false);

                    toastManager.ShowToast(error.details.Truncate(60));

                    saveButton.interactable = true;
                    loadButton.interactable = true;
                });
            }
            else
            {
                LoadPlayerData();
            }

            void LoadPlayerData()
            {
                client.GetPlayerRoutes().LoadPlayerData(playerData.playerAccessToken, response =>
                {
                    playerData.SetPlayerDataFromServer(PlayerData.LoadData());

                    playerData.SaveData();

                    achievements = AchievementData.LoadData();

                    spinnerContainer.SetActive(false);

                    toastManager.ShowToast(response.message.Truncate(60));

                    saveButton.interactable = true;
                    loadButton.interactable = true;

                    coinsTxt.text = playerData.coins.ToString();

                    UpdateMusicVolume();
                    UpdateSfxVolume();
                }, error =>
                {
                    spinnerContainer.SetActive(false);

                    toastManager.ShowToast(error.details.Truncate(60));

                    saveButton.interactable = true;
                    loadButton.interactable = true;
                }, progress => { });
            }
        }
        else
        {
            toastManager.ShowToast("No Internet Connection");
        }
    }

    public void RestorePurchases()
    {
        iAPV5Manager.RestorePurchases();
    }

    public void ConfirmLogout()
    {
        confirmPanel.gameObject.SetActive(true);
        confirmPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", true);
        buttonClickSfx.Play();

        confirmPanelText.text = "Are you sure, you want to logout? Your data will not be lost.";

        confirmPanelOkButton.onClick.RemoveAllListeners();

        confirmPanelOkButton.onClick.AddListener(Logout);
    }

    public void ConfirmSaveData()
    {
        confirmPanel.gameObject.SetActive(true);
        confirmPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", true);
        buttonClickSfx.Play();

        confirmPanelText.text = "This will save your data to the server and will overwrite the previous data.";

        confirmPanelOkButton.onClick.RemoveAllListeners();

        confirmPanelOkButton.onClick.AddListener(SaveDataToServer);
    }

    public void ConfirmLoadData()
    {
        confirmPanel.gameObject.SetActive(true);
        confirmPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", true);
        buttonClickSfx.Play();

        confirmPanelText.text = "This will load your data from the server and overwrite your current progress.";

        confirmPanelOkButton.onClick.RemoveAllListeners();

        confirmPanelOkButton.onClick.AddListener(LoadDataFromServer);
    }

    public void CloseConfirmPanel()
    {
        confirmPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", false);
        buttonClickSfx.Play();
        StartCoroutine(DelayedPanelClose(confirmPanel));
    }

    private bool CheckForNewQuest()
    {
        string savedTime = playerData.lastQuestLoadTime;
        DateTime now = DateTime.Now;

        DateTime today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
        DateTime lastGeneratedTime = string.IsNullOrEmpty(savedTime)
            ? DateTime.MinValue
            : DateTime.Parse(savedTime);

        if (now >= today && lastGeneratedTime < today)
        {
            int[] shuffled = Shuffle(new int[] { 0, 1, 2 });

            playerData.levelsCompletedQuestTotal = levelsQuest[shuffled[0]];
            playerData.levelsCompletedQuestProgress = 0;
            playerData.attemptsQuestTotal = attemptsQuest[shuffled[1]];
            playerData.attemptsQuestProgress = 0;
            playerData.coinsCollectedQuestTotal = coinsQuest[shuffled[2]];
            playerData.coinsCollectedQuestProgress = 0;

            playerData.lastQuestLoadTime = now.ToString("o");
            playerData.SaveData();
            return true;
        }

        return false;
    }

    private int[] Shuffle(int[] array)
    {
        System.Random rng = new System.Random();
        int n = array.Length;

        for (int i = n - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);

            (array[j], array[i]) = (array[i], array[j]);
        }

        return array;
    }

    private IEnumerator SwitchScene(string name)
    {
        crossfade.GetComponent<CanvasGroup>().blocksRaycasts = true;
        crossfade.SetBool("isOpen", true);
        yield return new WaitForSecondsRealtime(0.3f);
        SceneManager.LoadScene(name);
    }
}
