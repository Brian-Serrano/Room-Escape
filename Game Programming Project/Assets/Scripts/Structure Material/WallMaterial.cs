using UnityEngine;
using UnityEngine.SceneManagement;

public class WallMaterial : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    void Start()
    {
        ChangeMaterial();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Theme")
        {
            ChangeMaterial();
        }
    }

    void ChangeMaterial()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        meshRenderer.sharedMaterial = TemporaryData.instance.materials[TemporaryData.instance.selectedMaterials[4]];
    }
}
