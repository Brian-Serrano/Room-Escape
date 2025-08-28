using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("Shop Items UI")]
    public GameObject shopItemPrefab;
    public Transform shopItemsContainer;

    [Header("Confirm Panel UI")]
    public Transform confirmPanel;
    public TMP_Text confirmPanelTxt;
    public Button confirmPanelOkButton;

    [Header("Others")]
    public TMP_Text coinsTxt;
    public Transform notEnoughPanel;
    public ConfigHandler configHandler;
    public GameObject background;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Audio Source")]
    public AudioSource buttonClickSfx;
    public AudioSource itemBoughtSfx;

    private PlayerData playerData;
    private ToastManager toastManager;
    private List<TextureInfo> texturesInShop;

    class TextureInfo
    {
        public int index;
        public int price;

        public TextureInfo(int index, int price)
        {
            this.index = index;
            this.price = price;
        }
    }

    private void Awake()
    {
        playerData = PlayerData.LoadData();
        toastManager = GetComponent<ToastManager>();

        coinsTxt.text = playerData.coins.ToString();

        texturesInShop = new List<TextureInfo>
        {
            new TextureInfo(37, 300),
            new TextureInfo(19, 330),
            new TextureInfo(42, 360),
            new TextureInfo(3, 390),
            new TextureInfo(10, 420),
            new TextureInfo(14, 450),
            new TextureInfo(46, 480),
            new TextureInfo(29, 510),
            new TextureInfo(35, 540),
            new TextureInfo(6, 570),
            new TextureInfo(48, 600),
            new TextureInfo(24, 630),
            new TextureInfo(17, 660),
            new TextureInfo(39, 690),
            new TextureInfo(12, 720),
            new TextureInfo(1, 750),
            new TextureInfo(8, 780),
            new TextureInfo(44, 810),
            new TextureInfo(21, 840),
            new TextureInfo(31, 870),
            new TextureInfo(27, 900),
            new TextureInfo(33, 930)
        };

        foreach (TextureInfo texture in texturesInShop)
        {
            GameObject instance = Instantiate(shopItemPrefab, shopItemsContainer);

            instance.transform.GetChild(0).GetComponent<Image>().sprite = configHandler.sprites[texture.index];
            instance.transform.GetChild(1).GetComponent<TMP_Text>().text = texture.price + " <sprite index=0>";

            instance.transform.GetChild(2).gameObject.SetActive(playerData.materialsOwned[texture.index] == '1');

            Button button = instance.GetComponent<Button>();

            button.interactable = playerData.materialsOwned[texture.index] == '0';

            if (button.interactable)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    if (playerData.coins >= texture.price)
                    {
                        confirmPanel.gameObject.SetActive(true);
                        buttonClickSfx.Play();
                        confirmPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", true);

                        confirmPanelTxt.text = $"Are you sure you want to buy Texture #{texture.index + 1} for {texture.price} coins?";

                        confirmPanelOkButton.onClick.RemoveAllListeners();
                        confirmPanelOkButton.onClick.AddListener(() =>
                        {
                            confirmPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", false);
                            itemBoughtSfx.Play();
                            StartCoroutine(DelayedPanelClose(confirmPanel));

                            playerData.coins -= texture.price;
                            playerData.materialsOwned = playerData.materialsOwned.Remove(texture.index, 1).Insert(texture.index, "1");

                            coinsTxt.text = playerData.coins.ToString();

                            AchievementManager.CheckAchievements(AchievementData.LoadData(), playerData, toastManager);

                            playerData.SaveData();

                            instance.transform.GetChild(2).gameObject.SetActive(playerData.materialsOwned[texture.index] == '1');
                            button.interactable = playerData.materialsOwned[texture.index] == '0';
                        });
                    }
                    else
                    {
                        OpenNotEnoughPanel();
                    }
                });
            }
        }

        SetMaterials(background.GetComponentsInChildren<IMaterialController>());
    }

    private void Start()
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(playerData.musicVolume) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(playerData.sfxVolume) * 20);
    }

    private void SetMaterials(IMaterialController[] matControllers)
    {
        foreach (IMaterialController matController in matControllers)
        {
            matController.SetMaterial(configHandler.materials, playerData.materialsSelected);
        }
    }

    public void CloseConfirmPanel()
    {
        confirmPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", false);
        buttonClickSfx.Play();
        StartCoroutine(DelayedPanelClose(confirmPanel));
    }

    public void OpenNotEnoughPanel()
    {
        notEnoughPanel.gameObject.SetActive(true);
        buttonClickSfx.Play();
        notEnoughPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", true);
    }

    public void CloseNotEnoughPanel()
    {
        notEnoughPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", false);
        buttonClickSfx.Play();
        StartCoroutine(DelayedPanelClose(notEnoughPanel));
    }

    private IEnumerator DelayedPanelClose(Transform panel)
    {
        yield return new WaitForSecondsRealtime(0.2f);
        panel.gameObject.SetActive(false);
    }

    public void Back()
    {
        buttonClickSfx.Play();
        SceneManager.LoadScene("Submenu");
    }
}
