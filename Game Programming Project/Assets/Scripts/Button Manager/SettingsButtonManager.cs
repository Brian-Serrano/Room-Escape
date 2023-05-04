using System.Collections;
using UnityEngine;

public class SettingsButtonManager : MonoBehaviour
{
    public GameObject clickingBehindAvoider;
    public GameObject clickingBehindAvoider2;

    public GameObject quest;
    public GameObject login;
    public GameObject register;
    public GameObject questHelp;

    public void Back()
    {
        SFXManager.instance.PlaySFX(0);
        SceneTransition.instance.LoadScene("Menu");
    }

    public void OpenQuest()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Open", quest, 0.5f, clickingBehindAvoider);
    }

    public void CloseQuest()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Close", quest, 0.5f, clickingBehindAvoider);
    }

    public void OpenQuestHelp()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Open", questHelp, 0.25f, clickingBehindAvoider2);
    }

    public void CloseQuestHelp()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Close", questHelp, 0.25f, clickingBehindAvoider2);
    }

    public void OpenLogin()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Open", login, 0.5f, clickingBehindAvoider);
    }

    public void CloseLogin()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Close", login, 0.5f, clickingBehindAvoider);
    }

    public void OpenRegister()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Close", login, 0.5f, clickingBehindAvoider);
        StartCoroutine(Wait("Open", register, 0.5f, clickingBehindAvoider));
    }

    public void CloseRegister()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Close", register, 0.5f, clickingBehindAvoider);
        StartCoroutine(Wait("Open", login, 0.5f, clickingBehindAvoider));
    }

    public void CloseRegister2()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Close", register, 0.5f, clickingBehindAvoider);
        clickingBehindAvoider.SetActive(false);
    }

    public void Leaderboard()
    {
        SFXManager.instance.PlaySFX(0);
        SceneTransition.instance.LoadScene("Leaderboard");
    }

    public void Shop()
    {
        SFXManager.instance.PlaySFX(0);
        SceneTransition.instance.LoadScene("Shop");
    }

    IEnumerator Wait(string trigger, GameObject panel, float delay, GameObject CBA)
    {
        yield return new WaitForSeconds(delay + 0.1f);
        PanelAnimation.instance.AnimatePanel(trigger, panel, delay, CBA);
    }
}
