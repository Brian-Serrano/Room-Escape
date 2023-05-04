using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [HideInInspector] public static SceneTransition instance;

    public Animator transition;

    private void Start()
    {
        instance = this;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneWithAnimation(sceneName));
    }

    IEnumerator LoadSceneWithAnimation(string sceneName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene(sceneName);
    }
}
