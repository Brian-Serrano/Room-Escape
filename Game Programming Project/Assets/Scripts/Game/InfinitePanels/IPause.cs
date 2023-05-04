using UnityEngine;
using UnityEngine.UI;

public class IPause : MonoBehaviour
{
    public Slider music;
    public Slider sfx;
    public Slider sensitivity;

    void Start()
    {
        music.value = TemporaryData.instance.musicVolume;
        sfx.value = TemporaryData.instance.SFXVolume;
        sensitivity.value = TemporaryData.instance.sensitivity;
    }
}
