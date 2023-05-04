using System.Collections;
using UnityEngine;

public class ToggleLight : MonoBehaviour
{
    [HideInInspector] public bool toggle = true;

    private GameObject lightLeft;
    private GameObject lightRight;

    private MeshRenderer leftRenderer;
    private MeshRenderer rightRenderer;

    public Material[] materials;

    void Start()
    {
        lightLeft = transform.Find("Toggle Light Left").gameObject;
        lightRight = transform.Find("Toggle Light Right").gameObject;

        leftRenderer = lightLeft.GetComponent<MeshRenderer>();
        rightRenderer = lightRight.GetComponent<MeshRenderer>();

        Toggle();
    }

    public void Toggle()
    {
        toggle = !toggle;
        if (toggle)
        {
            leftRenderer.sharedMaterial = materials[1];
            rightRenderer.sharedMaterial = materials[0];
        }
        else
        {
            leftRenderer.sharedMaterial = materials[0];
            rightRenderer.sharedMaterial = materials[1];
        }
        StartCoroutine(ToggleTimer());
    }

    IEnumerator ToggleTimer()
    {
        yield return new WaitForSeconds(3);
        Toggle();
    }
}
