using System.Collections;
using TMPro;
using UnityEngine;

public class InfiniteButtonManager : MonoBehaviour
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

    public void Surrender()
    {
        SFXManager.instance.PlaySFX(3);
        PanelAnimation.instance.AnimatePanel("Open", gameOver, 0.5f, clickingBehindAvoider);
        Time.timeScale = 0f;
        gameOverText.text = "SURRENDER";
        GameQuestManager.instance.SetProgress();
        GameAchievementsManager.instance.ProcessAchievements();
    }

    public void PauseRestart()
    {
        SFXManager.instance.PlaySFX(0);
        StartCoroutine(DelayTimescale(0.25f));
        TemporaryData.instance.totalAttempts++;
        TemporaryData.instance.questVariables[2]++;
        SetScore();
        DataManager.SaveData(TemporaryData.instance);
        SceneTransition.instance.LoadScene("Infinite");
    }

    public void PauseHome()
    {
        SFXManager.instance.PlaySFX(0);
        StartCoroutine(DelayTimescale(0.25f));
        SetScore();
        DataManager.SaveData(TemporaryData.instance);
        SceneTransition.instance.LoadScene("Play");
    }

    public void PauseContinue()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Close", pause, 0.5f, clickingBehindAvoider);
        StartCoroutine(DelayTimescale(0.5f));
    }

    public void GameOverRetry()
    {
        SFXManager.instance.PlaySFX(0);
        StartCoroutine(DelayTimescale(0.25f));
        TemporaryData.instance.totalAttempts++;
        TemporaryData.instance.questVariables[2]++;
        SetScore();
        DataManager.SaveData(TemporaryData.instance);
        SceneTransition.instance.LoadScene("Infinite");
    }

    public void GameOverHome()
    {
        SFXManager.instance.PlaySFX(0);
        StartCoroutine(DelayTimescale(0.25f));
        SetScore();
        DataManager.SaveData(TemporaryData.instance);
        SceneTransition.instance.LoadScene("Play");
    }

    public void GameOverShop()
    {
        SFXManager.instance.PlaySFX(0);
        StartCoroutine(DelayTimescale(0.25f));
        SetScore();
        DataManager.SaveData(TemporaryData.instance);
        SceneTransition.instance.LoadScene("Shop");
    }

    private void SetScore()
    {
        if (TemporaryData.instance.highscore < InfiniteData.instance.gameScore)
        {
            TemporaryData.instance.highscore = InfiniteData.instance.gameScore;
        }
    }
    IEnumerator DelayTimescale(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;
    }

}
