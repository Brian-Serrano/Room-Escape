using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToastMessage : MonoBehaviour
{
    [HideInInspector] public static ToastMessage instance;

    private Queue<string> messageQueue = new Queue<string>();
    private bool isShowing = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Show(string message)
    {
        messageQueue.Enqueue(message);

        if (!isShowing)
        {
            ShowNextMessage();
        }
    }

    public void ShowNextMessage()
    {
        if (messageQueue.Count == 0)
        {
            isShowing = false;
            return;
        }

        string message = messageQueue.Dequeue();

        transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = message;
        if(message.EndsWith(" Completed!"))
        {
            SFXManager.instance.PlaySFX(6);
        }
        transform.GetChild(0).gameObject.SetActive(true);
        isShowing = true;
        StartCoroutine(HideMessage());
        StartCoroutine(DelayShowMessage());
    }

    IEnumerator HideMessage()
    {
        yield return new WaitForSecondsRealtime(2f);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    IEnumerator DelayShowMessage()
    {
        yield return new WaitForSecondsRealtime(2.1f);
        ShowNextMessage();
    }
}
