using System.Collections.Generic;
using UnityEngine;

public class DoorMaterial : MonoBehaviour, IMaterialController
{
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetMaterial(Material[] materials, List<int> materialsSelected)
    {
        meshRenderer.sharedMaterial = materials[materialsSelected[1]];
    }
}
