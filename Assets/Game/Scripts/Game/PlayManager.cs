using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text levelTxt;
    public Slider levelProgressSlider;
    public TMP_Text attemptsTxt;
    public TMP_Text highscoreTxt;

    [Header("Audio Source")]
    public AudioSource buttonClickSfx;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Others")]
    public ConfigHandler configHandler;
    public GameObject background;

    private PlayerData playerData;

    private void Start()
    {
        playerData = PlayerData.LoadData();

        levelTxt.text = playerData.level.ToString();
        levelProgressSlider.value = playerData.levelProgress;
        attemptsTxt.text = "ATTEMPTS: " + playerData.levelAttempts;
        highscoreTxt.text = playerData.highScore.ToString();

        audioMixer.SetFloat("MusicVolume", Mathf.Log10(playerData.musicVolume) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(playerData.sfxVolume) * 20);

        SetMaterials(background.GetComponentsInChildren<IMaterialController>());
    }

    private void SetMaterials(IMaterialController[] matControllers)
    {
        foreach (IMaterialController matController in matControllers)
        {
            matController.SetMaterial(configHandler.materials, playerData.materialsSelected);
        }
    }

    public void ClassicButton()
    {
        buttonClickSfx.Play();
        SceneManager.LoadScene("Game");
    }

    public void InfiniteButton()
    {
        buttonClickSfx.Play();
        SceneManager.LoadScene("Infinite");
    }

    public void BackButton()
    {
        buttonClickSfx.Play();
        SceneManager.LoadScene("Menu");
    }
}
