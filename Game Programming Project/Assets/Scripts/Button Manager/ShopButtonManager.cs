using UnityEngine;

public class ShopButtonManager : MonoBehaviour
{
    public GameObject clickingBehindAvoider;

    public GameObject confirm;
    public GameObject insufficient;

    public void Back()
    {
        SFXManager.instance.PlaySFX(0);
        SceneTransition.instance.LoadScene("Menu");
    }

    public void ConfirmOk()
    {
        SFXManager.instance.PlaySFX(5);
        PanelAnimation.instance.AnimatePanel("Close", confirm, 0.5f, clickingBehindAvoider);
        TemporaryData.instance.coins -= ShopManager.instance.coinNumber;
        TemporaryData.instance.ownedMaterials[ShopManager.instance.number - 1] = 1;
        GameAchievementsManager.instance.ProcessAchievements();
        CoinManager.instance.SetCoin();
        ShopManager.instance.SetBought(ShopManager.instance.index);
        DataManager.SaveData(TemporaryData.instance);
    }

    public void ConfirmCancel()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Close", confirm, 0.5f, clickingBehindAvoider);
    }

    public void ConfirmBack()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Close", confirm, 0.5f, clickingBehindAvoider);
    }

    public void InsufficientOk()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Close", insufficient, 0.5f, clickingBehindAvoider);
    }
}
