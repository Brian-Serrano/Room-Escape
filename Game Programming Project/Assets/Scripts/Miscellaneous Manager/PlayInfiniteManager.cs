using TMPro;
using UnityEngine;

public class PlayInfiniteManager : MonoBehaviour
{
    public TMP_Text highScore;

    void Start()
    {
        highScore.text = TemporaryData.instance.highscore.ToString();
    }
}
