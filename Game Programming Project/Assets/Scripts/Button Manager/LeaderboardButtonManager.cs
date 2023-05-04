using UnityEngine;

public class LeaderboardButtonManager : MonoBehaviour
{
    public GameObject[] data;

    private int subTab = 0;
    private int tab = 0;

    public void Level()
    {
        SFXManager.instance.PlaySFX(0);
        tab = 0;
        Reset();
        data[(tab * 2) + subTab].SetActive(true);
    }

    public void Coin()
    {
        SFXManager.instance.PlaySFX(0);
        tab = 1;
        Reset();
        data[(tab * 2) + subTab].SetActive(true);
    }

    public void Highscore()
    {
        SFXManager.instance.PlaySFX(0);
        tab = 2;
        Reset();
        data[(tab * 2) + subTab].SetActive(true);
    }

    public void Top50()
    {
        SFXManager.instance.PlaySFX(0);
        subTab = 0;
        Reset();
        data[(tab * 2) + subTab].SetActive(true);
    }

    public void YourPosition()
    {
        SFXManager.instance.PlaySFX(0);
        subTab = 1;
        Reset();
        data[(tab * 2) + subTab].SetActive(true);
    }

    public void Back()
    {
        SFXManager.instance.PlaySFX(0);
        SceneTransition.instance.LoadScene("Menu");
    }

    private void Reset()
    {
        for(int i = 0; i < data.Length; i++)
        {
            data[i].SetActive(false);
        }
    }
}
