using UnityEngine;

public class PlayButtonManager : MonoBehaviour
{
    public void Level()
    {
        SFXManager.instance.PlaySFX(0);
        TemporaryData.instance.levelAttempts++;
        TemporaryData.instance.totalAttempts++;
        TemporaryData.instance.questVariables[2]++;
        SceneTransition.instance.LoadScene("Game");
    }

    public void Infinite()
    {
        SFXManager.instance.PlaySFX(0);
        TemporaryData.instance.totalAttempts++;
        TemporaryData.instance.questVariables[2]++;
        SceneTransition.instance.LoadScene("Infinite");
    }

    public void Back()
    {
        SFXManager.instance.PlaySFX(0);
        SceneTransition.instance.LoadScene("Menu");
    }
}
