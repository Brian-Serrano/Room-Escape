using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [HideInInspector] public static CoinManager instance;

    public TMP_Text coin;

    void Start()
    {
        instance = this;

        SetCoin();
    }

    public void SetCoin()
    {
        coin.text = TemporaryData.instance.coins.ToString();
    }
}
