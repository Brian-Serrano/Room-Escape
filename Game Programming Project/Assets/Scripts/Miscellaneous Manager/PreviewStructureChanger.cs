using UnityEngine;

public class PreviewStructureChanger : MonoBehaviour
{
    [HideInInspector] public static PreviewStructureChanger instance;

    [HideInInspector] public int childCount;

    void Start()
    {
        instance = this;

        ChangeStructure();
    }

    public void ChangeStructure()
    {
        childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        transform.GetChild(TemporaryData.instance.selectedMaterialPreview).gameObject.SetActive(true);
    }
}
