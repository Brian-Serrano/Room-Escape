using UnityEngine;
using UnityEngine.UI;

public class SpinnerController : MonoBehaviour
{
    private bool spinnerEnabled;

    private RectTransform spinnerRect;

    void Start()
    {
        spinnerEnabled = gameObject.activeInHierarchy;

        spinnerRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (spinnerEnabled)
        {
            spinnerRect.eulerAngles += Vector3.back * (Time.deltaTime * 180f);
        }
    }

    void OnEnable()
    {
        spinnerEnabled = true;
    }

    void OnDisable()
    {
        spinnerEnabled = false;
    }
}