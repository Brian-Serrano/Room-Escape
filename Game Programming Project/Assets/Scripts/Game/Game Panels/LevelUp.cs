using TMPro;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    public TMP_Text attempts;
    public TMP_Text jumps;
    public TMP_Text time;
    public TMP_Text coins;

    void Start()
    {
        attempts.text = TemporaryData.instance.levelAttempts.ToString();
        jumps.text = GameData.instance.gameJumps.ToString();
        time.text = GameData.instance.gameTimer.ToString("0");
        coins.text = "x" + GameData.instance.gameCoins;

        TemporaryData.instance.levelAttempts = 0;
    }
}
