using TMPro;
using UnityEngine;

public class ThemeButtonManager : MonoBehaviour
{
    public TMP_Text text;

    public GameObject clickingBehindAvoider;
    public GameObject help;

    public void Block()
    {
        SFXManager.instance.PlaySFX(0);
        ThemeManager.instance.selectedMaterial = 0;
        SwitchMaterial(ThemeManager.instance.selectedMaterial);
        text.text = "CLOTH";
    }

    public void Door()
    {
        SFXManager.instance.PlaySFX(0);
        ThemeManager.instance.selectedMaterial = 1;
        SwitchMaterial(ThemeManager.instance.selectedMaterial);
        text.text = "PANEL";
    }

    public void Floor()
    {
        SFXManager.instance.PlaySFX(0);
        ThemeManager.instance.selectedMaterial = 2;
        SwitchMaterial(ThemeManager.instance.selectedMaterial);
        text.text = "FLOOR";
    }

    public void Spike()
    {
        SFXManager.instance.PlaySFX(0);
        ThemeManager.instance.selectedMaterial = 3;
        SwitchMaterial(ThemeManager.instance.selectedMaterial);
        text.text = "GROUND";
    }

    public void Wall()
    {
        SFXManager.instance.PlaySFX(0);
        ThemeManager.instance.selectedMaterial = 4;
        SwitchMaterial(ThemeManager.instance.selectedMaterial);
        text.text = "WALL";
    }

    public void Wood()
    {
        SFXManager.instance.PlaySFX(0);
        ThemeManager.instance.selectedMaterial = 5;
        SwitchMaterial(ThemeManager.instance.selectedMaterial);
        text.text = "WOOD";
    }

    public void Back()
    {
        SFXManager.instance.PlaySFX(0);
        SceneTransition.instance.LoadScene("Menu");
    }

    public void Next()
    {
        SFXManager.instance.PlaySFX(0);
        TemporaryData.instance.selectedMaterialPreview++;
        if(TemporaryData.instance.selectedMaterialPreview >= PreviewStructureChanger.instance.childCount)
        {
            TemporaryData.instance.selectedMaterialPreview = 0;
        }
        PreviewStructureChanger.instance.ChangeStructure();
    }

    public void Previous()
    {
        SFXManager.instance.PlaySFX(0);
        TemporaryData.instance.selectedMaterialPreview--;
        if(TemporaryData.instance.selectedMaterialPreview < 0)
        {
            TemporaryData.instance.selectedMaterialPreview = PreviewStructureChanger.instance.childCount - 1;
        }
        PreviewStructureChanger.instance.ChangeStructure();
    }

    public void OpenHelp()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Open", help, 0.5f, clickingBehindAvoider);
    }

    public void CloseHelp()
    {
        SFXManager.instance.PlaySFX(0);
        PanelAnimation.instance.AnimatePanel("Close", help, 0.5f, clickingBehindAvoider);
    }

    private void SwitchMaterial(int selected)
    {
        ThemeManager.instance.SwitchMaterial(ThemeManager.instance.statusSlots[TemporaryData.instance.selectedMaterials[selected]]);
        DataManager.SaveData(TemporaryData.instance);
    }
}
