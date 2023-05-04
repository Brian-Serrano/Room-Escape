using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider music;
    public Slider sfx;
    public Slider sensitivity;
    public Slider simDis;

    void Start()
    {
        music.value = TemporaryData.instance.musicVolume;
        sfx.value = TemporaryData.instance.SFXVolume;
        sensitivity.value = TemporaryData.instance.sensitivity;
        simDis.value = TemporaryData.instance.simulationDistance;
    }
}
