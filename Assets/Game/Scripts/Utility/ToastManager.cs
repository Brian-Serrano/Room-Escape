using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToastManager : MonoBehaviour
{
    public GameObject toastPrefab;
    public Transform toastParent;
    public float defaultDuration = 2f;

    private Queue<ToastData> toastQueue = new Queue<ToastData>();
    private bool isShowing = false;

    private class ToastData
    {
        public string message;
        public float duration;

        public ToastData(string msg, float dur)
        {
            message = msg;
            duration = dur;
        }
    }

    public void ShowToast(string message, float duration = -1f)
    {
        float showDuration = (duration <= 0) ? defaultDuration : duration;
        toastQueue.Enqueue(new ToastData(message, showDuration));
        if (!isShowing)
        {
            StartCoroutine(HandleToastQueue());
        }
    }

    private IEnumerator HandleToastQueue()
    {
        isShowing = true;

        while (toastQueue.Count > 0)
        {
            ToastData data = toastQueue.Dequeue();

            GameObject toast = Instantiate(toastPrefab, toastParent);
            Animator animator = toast.GetComponent<Animator>();
            toast.GetComponent<AudioSource>().Play();
            TMP_Text text = toast.GetComponentInChildren<TMP_Text>();
            if (text != null) text.text = data.message;

            // Play SlideIn
            animator.SetBool("isOpen", true);

            // Wait for slide-in duration
            yield return new WaitForSecondsRealtime(0.2f);

            // Stay on screen
            yield return new WaitForSecondsRealtime(data.duration);

            // Play SlideOut
            animator.SetBool("isOpen", false);

            // Wait for slide-out duration
            yield return new WaitForSecondsRealtime(0.2f);

            Destroy(toast);
        }

        isShowing = false;
    }
}
