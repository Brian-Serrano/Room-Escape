using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SubmenuManager : MonoBehaviour
{
    [Header("Login UI")]
    public TMP_InputField loginUsernameTxt;
    public TMP_InputField loginPasswordTxt;
    public TMP_InputField signupUsernameTxt;
    public TMP_InputField signupEmailTxt;
    public TMP_InputField signupPasswordTxt;
    public TMP_InputField signupConfirmPasswordTxt;
    public Button loginButton;
    public Button loginSubmitButton;
    public Button signupSubmitButton;
    public Transform loginPanel;
    public Transform registerPanel;

    [Header("Quest UI")]
    public Transform questPanel;
    public TMP_Text dateTxt;
    public TMP_Text questOneTitle;
    public TMP_Text questTwoTitle;
    public TMP_Text questThreeTitle;
    public TMP_Text questOneDescription;
    public TMP_Text questTwoDescription;
    public TMP_Text questThreeDescription;
    public Slider questOneProgress;
    public Slider questTwoProgress;
    public Slider questThreeProgress;
    public GameObject questOneCheck;
    public GameObject questTwoCheck;
    public GameObject questThreeCheck;

    [Header("Miscellaneous UI")]
    public GameObject spinnerContainer;
    public ConfigHandler configHandler;
    public GameObject background;
    public Animator crossfade;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Audio Source")]
    public AudioSource buttonClickSfx;

    private PlayerData playerData;
    private ToastManager toastManager;
    private RoomEscapeHTTPClient client;

    private List<int> levelsQuest = new List<int>() { 2, 4, 6 };
    private List<int> attemptsQuest = new List<int>() { 5, 10, 15 };
    private List<int> coinsQuest = new List<int>() { 20, 40, 60 };

    private string[] levelsQuestTitles = new string[] { "Level Beater", "Level Grinder", "Level Master" };
    private string[] attemptsQuestTitles = new string[] { "Nice Try", "Another Try", "Always Trying" };
    private string[] coinsQuestTitles = new string[] { "Coin Finder", "Coin Collector", "Coin Master" };

    private void Awake()
    {
        playerData = PlayerData.LoadData();
        toastManager = GetComponent<ToastManager>();
        client = RoomEscapeHTTPClient.GetInstance();

        BannerAdManager.GetInstance().EnsureBannerVisible();

        CheckLoginState();
    }

    private void Start()
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(playerData.musicVolume) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(playerData.sfxVolume) * 20);

        SetMaterials(background.GetComponentsInChildren<IMaterialController>());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!spinnerContainer.activeSelf)
            {
                if (questPanel.gameObject.activeSelf)
                {
                    CloseQuestPanel();
                }
                else if (loginPanel.gameObject.activeSelf)
                {
                    CloseLoginPanel();
                }
                else if (registerPanel.gameObject.activeSelf)
                {
                    CloseRegisterPanel();
                }
                else
                {
                    Back();
                }
            }
        }
    }

    private void SetMaterials(IMaterialController[] matControllers)
    {
        foreach (IMaterialController matController in matControllers)
        {
            matController.SetMaterial(configHandler.materials, playerData.materialsSelected);
        }
    }

    public void Back()
    {
        buttonClickSfx.Play();
        StartCoroutine(SwitchScene("Menu"));
    }

    public void Leaderboard()
    {
        buttonClickSfx.Play();
        StartCoroutine(SwitchScene("Leaderboard"));
    }

    public void Shop()
    {
        buttonClickSfx.Play();
        StartCoroutine(SwitchScene("Shop"));
    }

    public void OpenLoginPanel()
    {
        buttonClickSfx.Play();

        loginPanel.gameObject.SetActive(true);
        loginPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", true);

        ClearLoginInputFields();
    }

    public void CloseLoginPanel()
    {
        buttonClickSfx.Play();

        loginPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", false);
        StartCoroutine(DelayedPanelClose(loginPanel));
    }

    public void CloseRegisterPanel()
    {
        buttonClickSfx.Play();

        registerPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", false);
        StartCoroutine(DelayedPanelClose(registerPanel));
    }

    public void SwitchToRegisterPanel()
    {
        buttonClickSfx.Play();

        loginPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", false);
        StartCoroutine(DelayedPanelClose(loginPanel));
        StartCoroutine(DelayedPanelOpen(registerPanel));

        ClearSignupInputFields();
    }

    public void SwitchToLoginPanel()
    {
        buttonClickSfx.Play();

        registerPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", false);
        StartCoroutine(DelayedPanelClose(registerPanel));
        StartCoroutine(DelayedPanelOpen(loginPanel));

        ClearLoginInputFields();
    }

    public void Login()
    {
        buttonClickSfx.Play();

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (ValidateUsername(loginUsernameTxt.text.Trim()) && ValidatePassword(loginPasswordTxt.text.Trim()))
            {
                LoginRequest request = new LoginRequest(loginUsernameTxt.text.Trim(), loginPasswordTxt.text.Trim());

                spinnerContainer.SetActive(true);

                client.GetAuthorizationRoutes().Login(request, response =>
                {
                    playerData.playerAccessToken = response.accessToken;
                    playerData.playerRefreshToken = response.refreshToken;
                    playerData.playerId = response.playerId;
                    playerData.playerName = loginUsernameTxt.text.Trim();

                    AchievementManager.CheckAchievements(playerData, toastManager);

                    playerData.SaveData();

                    ClearLoginInputFields();

                    CheckLoginState();

                    toastManager.ShowToast("Successfully logged in");

                    spinnerContainer.SetActive(false);
                }, error =>
                {
                    toastManager.ShowToast(error.details.Truncate(60));

                    Debug.Log(error.error);
                    Debug.Log(error.details);

                    spinnerContainer.SetActive(false);
                });
            }
        }
        else
        {
            toastManager.ShowToast("No Internet Connection");
        }
    }

    public void Signup()
    {
        buttonClickSfx.Play();

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            bool nameValidate = ValidateUsername(signupUsernameTxt.text.Trim());
            bool emailValidate = ValidateEmail(signupEmailTxt.text.Trim());
            bool passwordValidate = ValidatePassword(signupPasswordTxt.text.Trim());
            bool passwordMatch = PasswordMatch(signupPasswordTxt.text.Trim(), signupConfirmPasswordTxt.text.Trim());

            if (nameValidate && emailValidate && passwordValidate && passwordMatch)
            {
                SignupRequest request = new SignupRequest(signupUsernameTxt.text.Trim(), signupEmailTxt.text.Trim(), signupPasswordTxt.text.Trim(), signupConfirmPasswordTxt.text.Trim());

                spinnerContainer.SetActive(true);

                client.GetAuthorizationRoutes().Signup(request, response =>
                {
                    playerData.playerAccessToken = response.accessToken;
                    playerData.playerRefreshToken = response.refreshToken;
                    playerData.playerId = response.playerId;
                    playerData.playerName = signupUsernameTxt.text.Trim();

                    AchievementManager.CheckAchievements(playerData, toastManager);

                    playerData.SaveData();

                    ClearSignupInputFields();

                    CheckLoginState();

                    toastManager.ShowToast("Successfully signed up");

                    spinnerContainer.SetActive(false);
                }, error =>
                {
                    toastManager.ShowToast(error.details.Truncate(60));

                    spinnerContainer.SetActive(false);
                });
            }
        }
        else
        {
            toastManager.ShowToast("No Internet Connection");
        }
    }

    private bool ValidateUsername(string username)
    {
        if (username.Length > 20 || username.Length < 8)
        {
            toastManager.ShowToast("Username should be 8 to 20 characters");
            return false;
        }
        if (username.Any(u => !char.IsLetterOrDigit(u)))
        {
            toastManager.ShowToast("Username should only contain alphanumeric characters");
            return false;
        }

        return true;
    }

    private bool ValidatePassword(string password)
    {
        if (password.Length > 20 || password.Length < 8)
        {
            toastManager.ShowToast("Password should be 8 to 20 characters");
            return false;
        }
        if (password.Any(u => !char.IsLetterOrDigit(u)))
        {
            toastManager.ShowToast("Password should only contain alphanumeric characters");
            return false;
        }

        return true;
    }

    private bool ValidateEmail(string email)
    {
        try
        {
            MailAddress address = new MailAddress(email);

            if (email.Length > 100 || email.Length < 15)
            {
                toastManager.ShowToast("Email should be 15 to 100 characters");
                return false;
            }
            if (address.Address != email)
            {
                toastManager.ShowToast("Invalid email");
                return false;
            }

            return true;
        }
        catch (Exception)
        {
            toastManager.ShowToast("Invalid email");
            return false;
        }
    }

    private bool PasswordMatch(string password, string confirmPassword)
    {
        if (password != confirmPassword)
        {
            toastManager.ShowToast("Passwords do not match");
            return false;
        }

        return true;
    }

    private void CheckLoginState()
    {
        if (playerData.playerName.Length > 0 && playerData.playerId > 0)
        {
            loginButton.interactable = false;
            loginSubmitButton.interactable = false;
            signupSubmitButton.interactable = false;
        }
        else
        {
            loginButton.interactable = true;
            loginSubmitButton.interactable = true;
            signupSubmitButton.interactable = true;
        }
    }

    private void ClearSignupInputFields()
    {
        signupUsernameTxt.text = "";
        signupEmailTxt.text = "";
        signupPasswordTxt.text = "";
        signupConfirmPasswordTxt.text = "";
    }

    private void ClearLoginInputFields()
    {
        loginUsernameTxt.text = "";
        loginPasswordTxt.text = "";
    }

    private IEnumerator DelayedPanelClose(Transform panel)
    {
        yield return new WaitForSecondsRealtime(0.2f);
        panel.gameObject.SetActive(false);
    }

    private IEnumerator DelayedPanelOpen(Transform panel)
    {
        yield return new WaitForSecondsRealtime(0.2f);
        panel.gameObject.SetActive(true);
        panel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", true);
    }

    private void SetQuestTexts()
    {
        dateTxt.text = DateTime.Parse(playerData.lastQuestLoadTime).ToString("MM/dd/yyyy");

        SetQuestText(levelsQuest, playerData.levelsCompletedQuestTotal, playerData.levelsCompletedQuestProgress,
            levelsQuestTitles, "Complete", "Levels");

        SetQuestText(attemptsQuest, playerData.attemptsQuestTotal, playerData.attemptsQuestProgress,
            attemptsQuestTitles, "Do", "Attempts");

        SetQuestText(coinsQuest, playerData.coinsCollectedQuestTotal, playerData.coinsCollectedQuestProgress,
            coinsQuestTitles, "Collect", "Coins");
    }

    private void SetQuestText(List<int> questArray, int total, int progress, string[] titles, string descWord1, string descWord2)
    {
        GameObject[] checks = { questOneCheck, questTwoCheck, questThreeCheck };
        TMP_Text[] titlesText = { questOneTitle, questTwoTitle, questThreeTitle };
        TMP_Text[] descriptionsText = { questOneDescription, questTwoDescription, questThreeDescription };
        Slider[] progresses = { questOneProgress, questTwoProgress, questThreeProgress };

        int idx = questArray.IndexOf(Math.Abs(total));

        if (total < 0)
        {
            checks[idx].SetActive(true);
            progresses[idx].gameObject.SetActive(false);
        }
        else
        {
            checks[idx].SetActive(false);
            progresses[idx].gameObject.SetActive(true);
            progresses[idx].value = Mathf.Clamp(progress / (float)total, 0f, 1f);
        }

        titlesText[idx].text = titles[idx];
        descriptionsText[idx].text = $"{descWord1} {Mathf.Abs(total)} {descWord2}.";
    }

    public void OpenQuestPanel()
    {
        buttonClickSfx.Play();

        questPanel.gameObject.SetActive(true);
        questPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", true);

        SetQuestTexts();
    }

    public void CloseQuestPanel()
    {
        buttonClickSfx.Play();

        questPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", false);
        StartCoroutine(DelayedPanelClose(questPanel));
    }

    private IEnumerator SwitchScene(string name)
    {
        crossfade.GetComponent<CanvasGroup>().blocksRaycasts = true;
        crossfade.SetBool("isOpen", true);
        yield return new WaitForSecondsRealtime(0.3f);
        SceneManager.LoadScene(name);
    }
}
