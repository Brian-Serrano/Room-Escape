using System.Collections.Generic;
using UnityEngine;

public class RoomMaterial : MonoBehaviour, IMaterialController
{
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetMaterial(Material[] materials, List<int> materialsSelected)
    {
        meshRenderer.sharedMaterials = new Material[] { materials[materialsSelected[4]], materials[materialsSelected[2]] };
    }
}
