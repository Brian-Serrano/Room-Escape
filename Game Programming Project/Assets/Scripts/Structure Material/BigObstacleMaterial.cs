using UnityEngine;
using UnityEngine.SceneManagement;

public class BigObstacleMaterial : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    void Start()
    {
        ChangeMaterial();
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Theme")
        {
            ChangeMaterial();
        }
    }

    void ChangeMaterial()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        Material[] materials = new Material[]
        {
            TemporaryData.instance.materials[TemporaryData.instance.selectedMaterials[5]], TemporaryData.instance.materials[TemporaryData.instance.selectedMaterials[0]]
        };

        meshRenderer.sharedMaterials = materials;
    }
}
