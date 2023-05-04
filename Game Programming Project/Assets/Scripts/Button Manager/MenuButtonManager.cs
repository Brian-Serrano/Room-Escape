using UnityEngine;

public class MenuButtonManager : MonoBehaviour
{
    public GameObject clickingBehindAvoider;
    public GameObject clickingBehindAvoider2;

    public GameObject achievement;
    public GameObject stats;
    public GameObject settings;
    public GameObject help;

    public GameObject logoutConfirm;
    public GameObject saveConfirm;
    public GameObject loadConfirm;

    public void Play()
    {
        SFXManager.instance.PlaySFX(0);
        SceneTransition.instance.LoadScene("Play");
    }

    public void Theme()
    {
        SFXManager.instance.PlaySFX(0);
        SceneTransition.instance.LoadScene("Theme");
    }

    public void Edit()
    {
        SFXManager.instance.PlaySFX(0);
        SceneTransition.instance.LoadScene("Settings");
    }

    public void OpenAchievements()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Open", achievement, 0.5f, clickingBehindAvoider);
    }

    public void CloseAchievements()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Close", achievement, 0.5f, clickingBehindAvoider);
    }

    public void OpenStats()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Open", stats, 0.5f, clickingBehindAvoider);
    }

    public void CloseStats()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Close", stats, 0.5f, clickingBehindAvoider);
    }

    public void OpenSettings()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Open", settings, 0.5f, clickingBehindAvoider);
    }

    public void CloseSettings()
    {
        SFXManager.instance.PlaySFX(0);
        DataManager.SaveData(TemporaryData.instance);
        PanelAnimation.instance.AnimatePanel("Close", settings, 0.5f, clickingBehindAvoider);
    }

    public void OpenHelp()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Open", help, 0.5f, clickingBehindAvoider);
    }

    public void CloseHelp()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Close", help, 0.5f, clickingBehindAvoider);
    }

    public void OpenLogoutConfirm()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Open", logoutConfirm, 0.25f, clickingBehindAvoider2);
    }

    public void CloseLogoutConfirm()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Close", logoutConfirm, 0.25f, clickingBehindAvoider2);
    }

    public void OpenSaveConfirm()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Open", saveConfirm, 0.25f, clickingBehindAvoider2);
    }

    public void CloseSaveConfirm()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Close", saveConfirm, 0.25f, clickingBehindAvoider2);
    }

    public void OpenLoadConfirm()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Open", loadConfirm, 0.25f, clickingBehindAvoider2);
    }

    public void CloseLoadConfirm()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Close", loadConfirm, 0.25f, clickingBehindAvoider2);
    }

    public void Quit()
    {
        SFXManager.instance.PlaySFX(0);
        Application.Quit();
    }
}
