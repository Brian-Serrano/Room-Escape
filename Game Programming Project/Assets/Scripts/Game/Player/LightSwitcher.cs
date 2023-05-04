using UnityEngine;

public class LightSwitcher : MonoBehaviour
{
    public Camera cam;
    public LayerMask layerMask;
    public float distance = 3f;

    private bool lightOpen = false;

    void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, distance, layerMask))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                lightOpen = !lightOpen;

                if (lightOpen)
                {
                    RenderSettings.ambientIntensity = 1f;
                }
                else
                {
                    RenderSettings.ambientIntensity = 0.3f;
                }
            }
        }
    }
}
