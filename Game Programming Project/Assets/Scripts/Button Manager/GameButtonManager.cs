using System.Collections;
using TMPro;
using UnityEngine;

public class GameButtonManager : MonoBehaviour
{
    public GameObject clickingBehindAvoider;

    public GameObject pause;
    public GameObject gameOver;

    public TMP_Text gameOverText;

    public void Pause()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Open", pause, 0.5f, clickingBehindAvoider);
        Time.timeScale = 0f;
        GameAchievementsManager.instance.ProcessAchievements();
    }

    public void PauseRestart()
    {
        SFXManager.instance.PlaySFX(0);
        StartCoroutine(DelayTimescale(0.25f));
        TemporaryData.instance.totalAttempts++;
        TemporaryData.instance.levelAttempts++;
        TemporaryData.instance.questVariables[2]++;
        SetProgress();
        DataManager.SaveData(TemporaryData.instance);
        SceneTransition.instance.LoadScene("Game");
    }

    public void PauseContinue()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Close", pause, 0.5f, clickingBehindAvoider);
        StartCoroutine(DelayTimescale(0.5f));
    }

    public void PauseHome()
    {
        SFXManager.instance.PlaySFX(0);
        StartCoroutine(DelayTimescale(0.25f));
        SetProgress();
        DataManager.SaveData(TemporaryData.instance);
        SceneTransition.instance.LoadScene("Play");
    }

    public void GameOverRetry()
    {
        SFXManager.instance.PlaySFX(0);
        StartCoroutine(DelayTimescale(0.25f));
        TemporaryData.instance.totalAttempts++;
        TemporaryData.instance.levelAttempts++;
        TemporaryData.instance.questVariables[2]++;
        SetProgress();
        DataManager.SaveData(TemporaryData.instance);
        SceneTransition.instance.LoadScene("Game");
    }

    public void GameOverHome()
    {
        SFXManager.instance.PlaySFX(0);
        StartCoroutine(DelayTimescale(0.25f));
        SetProgress();
        DataManager.SaveData(TemporaryData.instance);
        SceneTransition.instance.LoadScene("Play");
    }

    public void GameOverShop()
    {
        SFXManager.instance.PlaySFX(0);
        StartCoroutine(DelayTimescale(0.25f));
        SetProgress();
        DataManager.SaveData(TemporaryData.instance);
        SceneTransition.instance.LoadScene("Shop");
    }

    public void LevelUpNext()
    {
        SFXManager.instance.PlaySFX(0);
        StartCoroutine(DelayTimescale(0.25f));
        TemporaryData.instance.totalAttempts++;
        TemporaryData.instance.levelAttempts++;
        TemporaryData.instance.questVariables[2]++;
        DataManager.SaveData(TemporaryData.instance);
        SceneTransition.instance.LoadScene("Game");
    }

    public void LevelUpHome()
    {
        SFXManager.instance.PlaySFX(0);
        StartCoroutine(DelayTimescale(0.25f));
        DataManager.SaveData(TemporaryData.instance);
        SceneTransition.instance.LoadScene("Play");
    }

    public void LevelUpShop()
    {
        SFXManager.instance.PlaySFX(0);
        StartCoroutine(DelayTimescale(0.25f));
        DataManager.SaveData(TemporaryData.instance);
        SceneTransition.instance.LoadScene("Shop");
    }

    public void Surrender()
    {
        SFXManager.instance.PlaySFX(3);
        PanelAnimation.instance.AnimatePanel("Open", gameOver, 0.5f, clickingBehindAvoider);
        Time.timeScale = 0f;
        gameOverText.text = "SURRENDER";
        GameQuestManager.instance.SetProgress();
        GameAchievementsManager.instance.ProcessAchievements();
    }

    private void SetProgress()
    {
        if (TemporaryData.instance.levelProgress < GameData.instance.progress)
        {
            TemporaryData.instance.levelProgress = GameData.instance.progress;
        }
    }

    IEnumerator DelayTimescale(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;
    }
}
