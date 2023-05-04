using UnityEngine;
using UnityEngine.UI;

public class PlayerProgress : MonoBehaviour
{
    public Image fillBar;

    private void Update()
    {
        fillBar.fillAmount = GameData.instance.progress;
    }
}
