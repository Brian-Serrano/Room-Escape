using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ThemeManager : MonoBehaviour
{
    [Header("UI Elements")]
    public ConfigHandler configHandler;
    public Transform helpPanel;
    public Transform obstaclesContainer;
    public GameObject textureItemPrefab;
    public Transform texturesContainer;
    public Image[] buttons;
    public TMP_Text coinsTxt;
    public TMP_Text themeTabTxt;
    public GameObject background;
    public Animator crossfade;

    [Header("Texture Status Sprites")]
    public Sprite lockSprite;
    public Sprite addSprite;
    public Sprite checkSprite;

    [Header("Audio Source")]
    public AudioSource buttonClickSfx;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    private PlayerData playerData;

    private int obstacleIndex = 0;
    private ThemeTab tab = ThemeTab.CLOTH;

    private void Awake()
    {
        playerData = PlayerData.LoadData();

        coinsTxt.text = playerData.coins.ToString();

        for (int i = 0; i < configHandler.sprites.Length; i++)
        {
            GameObject instance = Instantiate(textureItemPrefab, texturesContainer);

            instance.transform.GetChild(0).GetComponent<Image>().sprite = configHandler.sprites[i];

            SetTextureStatus(instance.transform, i);
        }

        UpdateObstacles();
        UpdateTab();
    }

    private void Start()
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(playerData.musicVolume) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(playerData.sfxVolume) * 20);

        SetMaterials(background.GetComponentsInChildren<IMaterialController>());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (helpPanel.gameObject.activeSelf)
            {
                CloseThemeHelpPanel();
            }
            else
            {
                Back();
            }
        }

        obstaclesContainer.GetChild(obstacleIndex).Rotate(25 * Time.deltaTime * Vector3.up);
    }

    private void SetMaterials(IMaterialController[] matControllers)
    {
        foreach (IMaterialController matController in matControllers)
        {
            matController.SetMaterial(configHandler.materials, playerData.materialsSelected);
        }
    }

    public void PreviousObstacle()
    {
        obstacleIndex = obstacleIndex <= 0 ? 5 : obstacleIndex - 1;
        buttonClickSfx.Play();

        UpdateObstacles();
    }

    public void NextObstacle()
    {
        obstacleIndex = obstacleIndex >= 5 ? 0 : obstacleIndex + 1;
        buttonClickSfx.Play();

        UpdateObstacles();
    }

    public void ClothTab()
    {
        tab = ThemeTab.CLOTH;
        buttonClickSfx.Play();

        UpdateTab();
    }

    public void PanelTab()
    {
        tab = ThemeTab.PANEL;
        buttonClickSfx.Play();

        UpdateTab();
    }

    public void FloorTab()
    {
        tab = ThemeTab.FLOOR;
        buttonClickSfx.Play();

        UpdateTab();
    }

    public void GroundTab()
    {
        tab = ThemeTab.GROUND;
        buttonClickSfx.Play();

        UpdateTab();
    }

    public void WallTab()
    {
        tab = ThemeTab.WALL;
        buttonClickSfx.Play();

        UpdateTab();
    }

    public void WoodTab()
    {
        tab = ThemeTab.WOOD;
        buttonClickSfx.Play();

        UpdateTab();
    }

    private void UpdateObstacles()
    {
        for (int i = 0; i < obstaclesContainer.childCount; i++)
        {
            obstaclesContainer.GetChild(i).gameObject.SetActive(i == obstacleIndex);
        }

        SetMaterials(obstaclesContainer.GetChild(obstacleIndex).GetComponentsInChildren<IMaterialController>());
    }

    private void UpdateTab()
    {
        for (int i = 0; i < texturesContainer.childCount; i++)
        {
            Transform textureItem = texturesContainer.GetChild(i);

            SetTextureStatus(textureItem, i);
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].color = (i == (int)tab) ? new Color(0.45f, 0.45f, 0.45f, 1f) : Color.white;
        }

        themeTabTxt.text = tab.ToString();
    }

    private void SetTextureStatus(Transform texture, int i)
    {
        if (playerData.materialsSelected[(int)tab] == i)
        {
            texture.GetChild(1).GetComponent<Image>().sprite = checkSprite;
        }
        else
        {
            Sprite spriteToUse = playerData.materialsOwned[i] == '1' ? addSprite : lockSprite;
            texture.GetChild(1).GetComponent<Image>().sprite = spriteToUse;
        }

        Button button = texture.GetComponent<Button>();
        button.interactable = playerData.materialsOwned[i] == '1' && playerData.materialsSelected[(int)tab] != i;
        button.onClick.RemoveAllListeners();

        if (button.interactable)
        {
            button.onClick.AddListener(() =>
            {
                Transform prevTexture = texturesContainer.GetChild(playerData.materialsSelected[(int)tab]);
                int prevIdx = playerData.materialsSelected[(int)tab];

                playerData.materialsSelected[(int)tab] = i;

                playerData.SaveData();

                SetTextureStatus(prevTexture, prevIdx);
                SetTextureStatus(texture, i);

                buttonClickSfx.Play();
                SetMaterials(background.GetComponentsInChildren<IMaterialController>());
                SetMaterials(obstaclesContainer.GetChild(obstacleIndex).GetComponentsInChildren<IMaterialController>());
            });
        }
    }

    public void Back()
    {
        buttonClickSfx.Play();
        StartCoroutine(SwitchScene("Menu"));
    }

    public void OpenThemeHelpPanel()
    {
        helpPanel.gameObject.SetActive(true);
        buttonClickSfx.Play();
        helpPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", true);
    }

    public void CloseThemeHelpPanel()
    {
        helpPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", false);
        buttonClickSfx.Play();
        StartCoroutine(DelayedPanelClose(helpPanel));
    }

    private IEnumerator DelayedPanelClose(Transform panel)
    {
        yield return new WaitForSecondsRealtime(0.2f);
        panel.gameObject.SetActive(false);
    }

    private IEnumerator SwitchScene(string name)
    {
        crossfade.SetBool("isOpen", true);
        yield return new WaitForSecondsRealtime(0.3f);
        SceneManager.LoadScene(name);
    }
}
