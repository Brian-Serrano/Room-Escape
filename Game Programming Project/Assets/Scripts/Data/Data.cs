using System.Collections.Generic;

[System.Serializable]
public class Data
{
    public int level;
    public int highscore;
    public int coins;
    public int totalCoins;
    public int completedQuests;
    public int completedQuestsOneGame;
    public int[] completedAchievements;
    public int totalJumps;
    public float totalTime;
    public int totalAttempts;

    public int levelAttempts;
    public float levelProgress;
    public List<List<int>> currentLevel;

    public int[] ownedMaterials;
    public int[] selectedMaterials;
    public int selectedMaterialPreview;

    public float musicVolume;
    public float SFXVolume;
    public float sensitivity;
    public float simulationDistance;

    public int[] questCompleted;
    public List<int> currentQuest;
    public System.DateTime date;
    public int[] questVariables;

    public string username;
    public string email;
    public string password;
    public bool isLoggedIn;

    public Data(int l, int h, int c, int tc, int cq, int cqog, int[] ca, int tj, float tt, int ta, int la, float lp, List<List<int>> cl,
        int[] om, int[] sm, int smp, float mv, float sv, float s, float sd, int[] qc, List<int> cq1, System.DateTime d, int[] qv, string u,
        string e, string p, bool ili)
    {
        level = l;
        highscore = h;
        coins = c;
        totalCoins = tc;
        completedQuests = cq;
        completedQuestsOneGame = cqog;
        completedAchievements = ca;
        totalJumps = tj;
        totalTime = tt;
        totalAttempts = ta;

        levelAttempts = la;
        levelProgress = lp;
        currentLevel = cl;

        ownedMaterials = om;
        selectedMaterials = sm;
        selectedMaterialPreview = smp;

        musicVolume = mv;
        SFXVolume = sv;
        sensitivity = s;
        simulationDistance = sd;

        questCompleted = qc;
        currentQuest = cq1;
        date = d;
        questVariables = qv;

        username = u;
        email = e;
        password = p;
        isLoggedIn = ili;
    }
}
