using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryData : MonoBehaviour
{
    [HideInInspector] public static TemporaryData instance;

    // Game
    [HideInInspector] public int level;
    [HideInInspector] public int highscore;
    [HideInInspector] public int coins;
    [HideInInspector] public int totalCoins;
    [HideInInspector] public int completedQuests;
    [HideInInspector] public int completedQuestsOneGame;
    [HideInInspector] public int[] completedAchievements;
    [HideInInspector] public int totalJumps;
    [HideInInspector] public float totalTime;
    [HideInInspector] public int totalAttempts;

    // Level
    [HideInInspector] public int levelAttempts;
    [HideInInspector] public float levelProgress;
    [HideInInspector] public List<List<int>> currentLevel;

    // Materials
    [HideInInspector] public int[] ownedMaterials;
    [HideInInspector] public int[] selectedMaterials;
    [HideInInspector] public int selectedMaterialPreview;

    // Settings
    [HideInInspector] public float musicVolume;
    [HideInInspector] public float SFXVolume;
    [HideInInspector] public float sensitivity;
    [HideInInspector] public float simulationDistance;

    // Quest
    [HideInInspector] public int[] questCompleted;
    [HideInInspector] public List<int> currentQuest;
    [HideInInspector] public System.DateTime date;
    [HideInInspector] public int[] questVariables;

    // Account
    [HideInInspector] public string username;
    [HideInInspector] public string email;
    [HideInInspector] public string password;
    [HideInInspector] public bool isLoggedIn;

    // Game Components
    public Material[] materials;
    private List<List<int>> questCombinations;
    [HideInInspector] public float[] progressAchievements;
    [HideInInspector] public float[] questProgress;
    public bool alreadyLoggedIn = false;

    private void Awake()
    {
        questCombinations = new List<List<int>>()
        {
            new List<int>() { 0, 1, 2 },
            new List<int>() { 0, 2, 1 },
            new List<int>() { 1, 0, 2 },
            new List<int>() { 2, 0, 1 },
            new List<int>() { 1, 2, 0 },
            new List<int>() { 2, 1, 0 }
        };

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        Data data = DataManager.LoadData();

        ProcessData(data);

        CheckDate();

        InvokeRepeating(nameof(CheckNetwork), 5.0f, 5.0f);
    }

    public void CheckDate()
    {
        if(System.DateTime.Today.Date > date.Date)
        {
            ResetDate();
        }
    }

    private void ResetDate()
    {
        questCompleted = new int[] { 0, 0, 0 };
        currentQuest = questCombinations[Random.Range(0, questCombinations.Count)];
        date = System.DateTime.Today;
        questVariables = new int[] { 0, 0, 0 }; // Coin, Level, Attempt
    }

    public void ProcessData(Data data)
    {
        if (data != null)
        {
            level = data.level;
            highscore = data.highscore;
            coins = data.coins;
            totalCoins = data.totalCoins;
            completedQuests = data.completedQuests;
            completedQuestsOneGame = data.completedQuestsOneGame;
            completedAchievements = data.completedAchievements;
            totalJumps = data.totalJumps;
            totalTime = data.totalTime;
            totalAttempts = data.totalAttempts;

            levelAttempts = data.levelAttempts;
            levelProgress = data.levelProgress;
            currentLevel = data.currentLevel;

            ownedMaterials = data.ownedMaterials;
            selectedMaterials = data.selectedMaterials;
            selectedMaterialPreview = data.selectedMaterialPreview;

            musicVolume = data.musicVolume;
            SFXVolume = data.SFXVolume;
            sensitivity = data.sensitivity;
            simulationDistance = data.simulationDistance;

            questCompleted = data.questCompleted;
            currentQuest = data.currentQuest;
            date = data.date;
            questVariables = data.questVariables;

            username = data.username;
            email = data.email;
            password = data.password;
            isLoggedIn = data.isLoggedIn;
        }
        else
        {
            level = 0;
            highscore = 0;
            coins = 100;
            totalCoins = 100;
            completedQuests = 0;
            completedQuestsOneGame = 0;
            completedAchievements = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            totalJumps = 0;
            totalTime = 0;
            totalAttempts = 0;

            levelAttempts = 0;
            levelProgress = 0;
            currentLevel = new List<List<int>>();

            ownedMaterials = new int[] { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            selectedMaterials = new int[] { 0, 23, 5, 15, 25, 40 }; // Block, Door, Floor, Spike, Wall, Wood
            selectedMaterialPreview = 0;

            ResetDate();

            musicVolume = 1f;
            SFXVolume = 1f;
            sensitivity = 0.5f;
            simulationDistance = 1f;

            username = "User";
            email = "user@example.com";
            password = "User123";
            isLoggedIn = false;
        }

        progressAchievements = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        questProgress = new float[] { 0, 0, 0 };
    }

    IEnumerator LoginUser()
    {
        yield return new WaitForSeconds(1);

        if (isLoggedIn)
        {
            PlayFabManager.instance.Login(email, password);
        }
    }

    public void CheckNetwork()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable && !alreadyLoggedIn)
        {
            StartCoroutine(LoginUser());
            alreadyLoggedIn = true;
        }
    }
}
