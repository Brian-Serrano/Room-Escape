using System.Collections;
using UnityEngine;

public class PanelAnimation : MonoBehaviour
{
    [HideInInspector] public static PanelAnimation instance;

    private void Start()
    {
        instance = this;
    }

    public void AnimatePanel(string trigger, GameObject panel, float delay, GameObject CBA)
    {
        switch (trigger)
        {
            case "Open":
                panel.SetActive(true);
                CBA.SetActive(true);
                break;
            case "Close":
                StartCoroutine(DelayActivation(panel, delay, CBA));
                break;
        }

        panel.GetComponent<Animator>().SetTrigger(trigger);
    }

    IEnumerator DelayActivation(GameObject panel, float delay, GameObject CBA)
    {
        yield return new WaitForSeconds(delay);
        panel.SetActive(false);
        CBA.SetActive(false);
    }
}
