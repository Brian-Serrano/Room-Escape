using TMPro;
using UnityEngine;

public class GameData : MonoBehaviour
{
    [HideInInspector] public static GameData instance;

    private ObstacleMaker om;
    public GameObject clickingBehindAvoider;
    public GameObject gameOver;
    public GameObject levelUp;
    public GameObject player;
    public TMP_Text gameOverText;

    private GameObject[] finishLines;
    private float levelDistance;

    [HideInInspector] public bool gameTimerFlag = true;
    [HideInInspector] public bool progressFlag = true;

    public TMP_Text text;

    [HideInInspector] public int gameJumps;
    [HideInInspector] public int gameCoins;
    [HideInInspector] public float gameTimer;
    [HideInInspector] public float progress;

    private void Start()
    {
        instance = this;

        RenderSettings.ambientIntensity = 0.3f;

        om = GetComponent<ObstacleMaker>();
        finishLines = GameObject.FindGameObjectsWithTag("Finish Line");

        gameJumps = 0;
        gameCoins = 0;
        gameTimer = om.randomNumber * 10;
        levelDistance = finishLines[1].transform.position.z - finishLines[0].transform.position.z;
    }

    private void Update()
    {
        gameTimer -= 1 * Time.deltaTime;
        TemporaryData.instance.totalTime += 1 * Time.deltaTime;
        text.text = gameTimer.ToString("0");

        if(gameTimer <= 0 && gameTimerFlag)
        {
            SFXManager.instance.PlaySFX(3);
            PanelAnimation.instance.AnimatePanel("Open", gameOver, 0.5f, clickingBehindAvoider);
            Time.timeScale = 0f;
            gameOverText.text = "TIMES UP";
            GameQuestManager.instance.SetProgress();
            GameAchievementsManager.instance.ProcessAchievements();
            gameTimerFlag = false;
        }

        progress = (player.transform.position.z - finishLines[0].transform.position.z) / levelDistance;

        if(progress >= 1 && progressFlag)
        {
            SFXManager.instance.PlaySFX(2);
            PanelAnimation.instance.AnimatePanel("Open", levelUp, 0.5f, clickingBehindAvoider);
            Time.timeScale = 0f;
            TemporaryData.instance.level++;
            TemporaryData.instance.questVariables[1]++;
            TemporaryData.instance.levelProgress = 0;
            GameQuestManager.instance.SetProgress();
            GameAchievementsManager.instance.ProcessAchievements();
            progressFlag = false;
        }
    }
}
