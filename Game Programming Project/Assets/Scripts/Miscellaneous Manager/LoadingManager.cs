using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public Image fillBar;

    void Start()
    {
        StartCoroutine(LoadSceneAsynchronously("Menu"));
    }

    IEnumerator LoadSceneAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            fillBar.fillAmount = Mathf.Clamp01(operation.progress / 0.9f);

            yield return null;
        }
    }
}
