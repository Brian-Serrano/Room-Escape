using TMPro;
using UnityEngine;

public class IGameOver : MonoBehaviour
{
    public TMP_Text jumps;
    public TMP_Text time;
    public TMP_Text coin;
    public TMP_Text score;

    void Start()
    {
        jumps.text = InfiniteData.instance.gameJumps.ToString();
        time.text = InfiniteData.instance.gameTimer.ToString("0");
        coin.text = InfiniteData.instance.gameCoins.ToString();
        score.text = InfiniteData.instance.gameScore.ToString();
    }
}
