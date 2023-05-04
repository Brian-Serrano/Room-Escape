using UnityEngine;

public class LogoutManager : MonoBehaviour
{
    public GameObject CBA;
    public GameObject logoutPanel;

    public void LogOut()
    {
        SFXManager.instance.PlaySFX(0);

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            PlayFabManager.instance.Logout();
            PanelAnimation.instance.AnimatePanel("Close", logoutPanel, 0.25f, CBA);
        }
        else
        {
            ToastMessage.instance.Show("No Internet Connection");
        }
    }
}
