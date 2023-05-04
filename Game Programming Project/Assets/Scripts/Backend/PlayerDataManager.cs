using PlayFab;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public GameObject CBA;
    public GameObject saveConfirm;
    public GameObject loadConfirm;

    public void SaveData()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                TemporaryData d = TemporaryData.instance;
                PlayFabManager.instance.SaveData(new Data(d.level, d.highscore, d.coins, d.totalCoins, d.completedQuests, d.completedQuestsOneGame, d.completedAchievements,
                d.totalJumps, d.totalTime, d.totalAttempts, d.levelAttempts, d.levelProgress, d.currentLevel, d.ownedMaterials, d.selectedMaterials,
                d.selectedMaterialPreview, d.musicVolume, d.SFXVolume, d.sensitivity, d.simulationDistance, d.questCompleted, d.currentQuest, d.date,
                d.questVariables, d.username, d.email, d.password, d.isLoggedIn));
            }
            else
                ToastMessage.instance.Show("You are not logged in");
            PanelAnimation.instance.AnimatePanel("Close", saveConfirm, 0.25f, CBA);
        }
        else
        {
            ToastMessage.instance.Show("No Internet Connection");
        }
        SFXManager.instance.PlaySFX(0);
    }

    public void LoadData()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (PlayFabClientAPI.IsClientLoggedIn())
                PlayFabManager.instance.LoadData();
            else
                ToastMessage.instance.Show("You are not logged in");
            PanelAnimation.instance.AnimatePanel("Close", loadConfirm, 0.25f, CBA);
        }
        else
        {
            ToastMessage.instance.Show("No Internet Connection");
        }
        SFXManager.instance.PlaySFX(0);
    }
}
