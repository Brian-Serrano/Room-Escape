using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [HideInInspector] public static SFXManager instance;

    public AudioSource[] sfx;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetVolume();
    }

    public void SetVolume()
    {
        foreach(AudioSource s in sfx)
        {
            s.volume = TemporaryData.instance.SFXVolume;
        }
    }

    public void PlaySFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Play();
    }
}
