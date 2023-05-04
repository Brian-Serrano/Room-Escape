using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collision : MonoBehaviour
{
    public GameObject gameOver;
    public GameObject clickingBehindAvoider;
    public TMP_Text gameOverText;

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.collider.CompareTag("Spike"))
        {
            SFXManager.instance.PlaySFX(3);
            PanelAnimation.instance.AnimatePanel("Open", gameOver, 0.5f, clickingBehindAvoider);
            Time.timeScale = 0f;
            gameOverText.text = "HIT SPIKE";
            GameQuestManager.instance.SetProgress();
            GameAchievementsManager.instance.ProcessAchievements();
        }

        if (collision.collider.CompareTag("Water"))
        {
            SFXManager.instance.PlaySFX(3);
            PanelAnimation.instance.AnimatePanel("Open", gameOver, 0.5f, clickingBehindAvoider);
            Time.timeScale = 0f;
            gameOverText.text = "WATER STUCK";
            GameQuestManager.instance.SetProgress();
            GameAchievementsManager.instance.ProcessAchievements();
        }

        if (collision.collider.CompareTag("Platform"))
        {
            transform.SetParent(collision.collider.transform);
        }
    }

    private void OnCollisionExit(UnityEngine.Collision collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            transform.SetParent(null);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            TemporaryData.instance.coins++;
            TemporaryData.instance.totalCoins++;
            TemporaryData.instance.questVariables[0]++;
            SFXManager.instance.PlaySFX(7);
            if (SceneManager.GetActiveScene().name == "Game")
            {
                GameData.instance.gameCoins++;
            }
            else if (SceneManager.GetActiveScene().name == "Infinite")
            {
                InfiniteData.instance.gameCoins++;
            }
        }
    }
}
