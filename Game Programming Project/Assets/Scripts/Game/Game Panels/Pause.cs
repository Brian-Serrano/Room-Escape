using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public Image progressBar;
    public Slider music;
    public Slider sfx;
    public Slider sensitivity;

    void Start()
    {
        progressBar.fillAmount = GameData.instance.progress;
        music.value = TemporaryData.instance.musicVolume;
        sfx.value = TemporaryData.instance.SFXVolume;
        sensitivity.value = TemporaryData.instance.sensitivity;
    }
}
