using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public TMP_Text attempts;
    public TMP_Text jumps;
    public TMP_Text time;

    public Image progressBar;

    void Start()
    {
        attempts.text = TemporaryData.instance.levelAttempts.ToString();
        jumps.text = GameData.instance.gameJumps.ToString();
        time.text = GameData.instance.gameTimer.ToString("0");
        progressBar.fillAmount = GameData.instance.progress;
    }
}
