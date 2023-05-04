using TMPro;
using UnityEngine;

public class InfiniteData : MonoBehaviour
{
    [HideInInspector] public static InfiniteData instance;

    private float startPosition;

    public GameObject clickingBehindAvoider;
    public Transform playerPosition;
    public GameObject gameOver;
    public TMP_Text gameOverText;

    [HideInInspector] public bool gameOverFlag = true;

    [HideInInspector] public int gameJumps;
    [HideInInspector] public int gameCoins;
    [HideInInspector] public float gameTimer;
    [HideInInspector] public int gameLives;
    [HideInInspector] public int gameScore;

    [HideInInspector] public int decrementation;

    void Start()
    {
        instance = this;

        RenderSettings.ambientIntensity = 0.3f;

        startPosition = playerPosition.position.z;

        gameJumps = 0;
        gameCoins = 0;
        gameTimer = 0;
        gameLives = 3;
        gameScore = 0;

        decrementation = 0;
    }

    void Update()
    {
        gameTimer += 1 * Time.deltaTime;
        TemporaryData.instance.totalTime += 1 * Time.deltaTime;

        gameScore = (int)((playerPosition.position.z - startPosition) - decrementation);

        if(gameScore <= 0)
        {
            gameScore = 0;
        }

        if (gameLives <= 0 && gameOverFlag)
        {
            SFXManager.instance.PlaySFX(3);
            PanelAnimation.instance.AnimatePanel("Open", gameOver, 0.5f, clickingBehindAvoider);
            Time.timeScale = 0f;
            gameOverText.text = "OUT OF LIVES";
            GameQuestManager.instance.SetProgress();
            GameAchievementsManager.instance.ProcessAchievements();
            gameOverFlag = false;
        }
    }
}
