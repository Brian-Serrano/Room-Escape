using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayGameManager : MonoBehaviour
{
    public TMP_Text level;
    public TMP_Text attempt;
    public Image progress;

    void Start()
    {
        int currentLevel = TemporaryData.instance.level + 1;
        level.text = currentLevel.ToString();
        attempt.text = "ATTEMPTS: " + TemporaryData.instance.levelAttempts;
        progress.fillAmount = TemporaryData.instance.levelProgress;
    }
}
