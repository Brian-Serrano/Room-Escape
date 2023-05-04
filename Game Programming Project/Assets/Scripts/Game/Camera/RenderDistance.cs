using UnityEngine;

public class RenderDistance : MonoBehaviour
{
    [HideInInspector] public static RenderDistance instance;

    public Camera cam;

    void Start()
    {
        instance = this;

        SetView();
    }

    public void SetView()
    {
        cam.farClipPlane = (TemporaryData.instance.simulationDistance * 800) + 200;
    }
}
