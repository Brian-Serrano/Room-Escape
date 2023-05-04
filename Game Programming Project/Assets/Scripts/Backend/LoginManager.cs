using PlayFab;
using TMPro;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField email;
    public TMP_InputField password;

    public GameObject CBA;
    public GameObject loginPanel;

    public void Login()
    {
        SFXManager.instance.PlaySFX(0);
        if(Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (!PlayFabClientAPI.IsClientLoggedIn())
                PlayFabManager.instance.Login(email.text, password.text);
            else
                ToastMessage.instance.Show("You are already logged in");
            PanelAnimation.instance.AnimatePanel("Close", loginPanel, 0.5f, CBA);
        }
        else
        {
            ToastMessage.instance.Show("No Internet Connection");
        }
    }
}
