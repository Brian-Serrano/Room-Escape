using System.Collections.Generic;
using UnityEngine;

public interface IMaterialController
{
    public void SetMaterial(Material[] materials, List<int> materialsSelected);
}
