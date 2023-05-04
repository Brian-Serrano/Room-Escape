using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    [HideInInspector] public static VolumeManager instance;

    private void Start()
    {
        instance = this;

        SetVolume();
    }

    public void SetVolume()
    {
        GetComponent<AudioSource>().volume = TemporaryData.instance.musicVolume;
    }
}
