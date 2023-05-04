using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeManager : MonoBehaviour
{
    [HideInInspector] public static ThemeManager instance;

    private GameObject[] slots;

    [HideInInspector] public List<GameObject> statusSlots;
    [HideInInspector] public int selectedMaterial;

    public Sprite[] sprites;

    private void Start()
    {
        instance = this;

        slots = GameObject.FindGameObjectsWithTag("Slots");
        statusSlots = new List<GameObject>();

        selectedMaterial = 0;

        for(int i = 0; i < slots.Length; i++)
        {
            statusSlots.Add(slots[i].transform.Find("Status").gameObject);

            Button button = slots[i].GetComponentInChildren<Button>();
            if (button != null)
            {
                int slotIndex = i;
                button.onClick.AddListener(() => Select(slotIndex));
            }
        }

        SwitchMaterial(statusSlots[TemporaryData.instance.selectedMaterials[selectedMaterial]]);
    }

    private void Refresh()
    {
        for (int i = 0; i < statusSlots.Count; i++)
        {
            statusSlots[i].GetComponent<Image>().sprite = sprites[TemporaryData.instance.ownedMaterials[i]];
        }
    }

    private void Select(int slotIndex)
    {
        GameObject status = statusSlots[slotIndex];
        if (status.GetComponent<Image>().sprite.Equals(sprites[1]))
        {
            TemporaryData.instance.selectedMaterials[selectedMaterial] = slotIndex;
            SFXManager.instance.PlaySFX(0);
            DataManager.SaveData(TemporaryData.instance);
            SwitchMaterial(status);
        }
    }

    public void SwitchMaterial(GameObject status)
    {
        Refresh();
        status.GetComponent<Image>().sprite = sprites[2];
    }
}
