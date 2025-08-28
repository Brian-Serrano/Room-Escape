using System.Collections;
using UnityEngine;

public class DoorLightController : MonoBehaviour
{
    [HideInInspector] public bool toggle = true;

    private MeshRenderer leftRenderer;
    private MeshRenderer rightRenderer;

    public Material[] materials;

    void Start()
    {
        leftRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        rightRenderer = transform.GetChild(1).GetComponent<MeshRenderer>();

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

        Invoke(nameof(Toggle), 3f);
    }
}
