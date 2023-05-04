using PlayFab;
using TMPro;
using UnityEngine;

public class ResetPasswordManager : MonoBehaviour
{
    public TMP_InputField email;

    public GameObject CBA;
    public GameObject reset;

    public void ResetPassword()
    {
        SFXManager.instance.PlaySFX(0);
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (!PlayFabClientAPI.IsClientLoggedIn())
                PlayFabManager.instance.ResetPassword(email.text);
            else
                ToastMessage.instance.Show("You are already logged in");
            PanelAnimation.instance.AnimatePanel("Close", reset, 0.5f, CBA);
        }
        else
        {
            ToastMessage.instance.Show("No Internet Connection");
        }
    }
}
