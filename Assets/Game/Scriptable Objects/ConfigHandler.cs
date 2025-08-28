using UnityEngine;

[CreateAssetMenu(fileName = "ConfigHandler", menuName = "Scriptable Objects/ConfigHandler")]
public class ConfigHandler : ScriptableObject
{
    public Material[] materials;
    public Sprite[] sprites;
    public GameObject[] structures;
    public GameObject room;
}
