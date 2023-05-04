using PlayFab;
using TMPro;
using UnityEngine;

public class RegisterManager : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField email;
    public TMP_InputField password;
    public TMP_InputField confPassword;

    public GameObject CBA;
    public GameObject registerPanel;

    public void Register()
    {
        SFXManager.instance.PlaySFX(0);
        if (string.Equals(password.text, confPassword.text))
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                if (!PlayFabClientAPI.IsClientLoggedIn())
                    PlayFabManager.instance.Register(username.text, email.text, password.text);
                else
                    ToastMessage.instance.Show("You are already logged in");
                PanelAnimation.instance.AnimatePanel("Close", registerPanel, 0.5f, CBA);
            }
            else
            {
                ToastMessage.instance.Show("No Internet Connection");
            }
        }
        else
        {
            ToastMessage.instance.Show("Password don't match");
        }
    }
}
