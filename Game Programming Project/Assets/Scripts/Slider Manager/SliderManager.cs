using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    public Slider music;
    public Slider sfx;
    public Slider sensitivity;
    public Slider simDis;

    public void changeMusicVolume()
    {
        TemporaryData.instance.musicVolume = music.value;
        VolumeManager.instance.SetVolume();
    }

    public void changeSFXVolume()
    {
        TemporaryData.instance.SFXVolume = sfx.value;
        SFXManager.instance.SetVolume();
    }

    public void changeSensitivity()
    {
        TemporaryData.instance.sensitivity = sensitivity.value;
        if(SceneManager.GetActiveScene().name == "Game" || SceneManager.GetActiveScene().name == "Infinite")
            PlayerMovement.instance.ResetSensitivity();
    }

    public void changeSimulationDistance()
    {
        TemporaryData.instance.simulationDistance = simDis.value;
        RenderDistance.instance.SetView();
    }
}
