using TMPro;
using UnityEngine;

public class ScoreAndLifeManager : MonoBehaviour
{
    public TMP_Text score;
    public TMP_Text life;

    void Update()
    {
        score.text = "Score: " + InfiniteData.instance.gameScore;
        life.text = "Life: " + InfiniteData.instance.gameLives;
    }
}
