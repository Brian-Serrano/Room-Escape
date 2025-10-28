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
    public Animator crossfade;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Audio Source")]
    public AudioSource buttonClickSfx;
    public AudioSource itemBoughtSfx;

    private PlayerData playerData;
    private ToastManager toastManager;
    private List<TextureInfo> texturesInShop;
    private IAPV5Manager iAPV5Manager;

    class TextureInfo
    {
        public int index;
        public int price;
        public string id;
        public Currency currency;

        public TextureInfo(int index, int price, string id, Currency currency)
        {
            this.index = index;
            this.price = price;
            this.id = id;
            this.currency = currency;
        }
    }

    private void Awake()
    {
        playerData = PlayerData.LoadData();
        toastManager = GetComponent<ToastManager>();
        iAPV5Manager = IAPV5Manager.GetInstance();

        BannerAdManager.GetInstance().EnsureBannerVisible();

        coinsTxt.text = playerData.coins.ToString();

        texturesInShop = new List<TextureInfo>
        {
            new TextureInfo(37, 300, "texture_38", Currency.COIN),
            new TextureInfo(19, 330, "texture_20", Currency.COIN),
            new TextureInfo(42, 360, "texture_43", Currency.COIN),
            new TextureInfo(3, 390, "texture_4", Currency.COIN),
            new TextureInfo(10, 420, "texture_11", Currency.COIN),
            new TextureInfo(14, 450, "texture_15", Currency.COIN),
            new TextureInfo(46, 480, "texture_47", Currency.COIN),
            new TextureInfo(29, 510, "texture_30", Currency.COIN),
            new TextureInfo(35, 540, "texture_36", Currency.COIN),
            new TextureInfo(6, 570, "texture_7", Currency.COIN),
            new TextureInfo(48, 600, "texture_49", Currency.COIN),
            new TextureInfo(24, 630, "texture_25", Currency.COIN),
            new TextureInfo(17, 660, "texture_18", Currency.COIN),
            new TextureInfo(39, 690, "texture_40", Currency.COIN),
            new TextureInfo(12, 720, "texture_13", Currency.COIN),
            new TextureInfo(1, 750, "texture_2", Currency.COIN),
            new TextureInfo(8, 780, "texture_9", Currency.COIN),
            new TextureInfo(44, 1, "texture_45", Currency.MONEY),
            new TextureInfo(21, 1, "texture_22", Currency.MONEY),
            new TextureInfo(31, 1, "texture_32", Currency.MONEY),
            new TextureInfo(27, 1, "texture_28", Currency.MONEY),
            new TextureInfo(33, 1, "texture_34", Currency.MONEY)
        };

        foreach (TextureInfo texture in texturesInShop)
        {
            GameObject instance = Instantiate(shopItemPrefab, shopItemsContainer);

            instance.transform.GetChild(0).GetComponent<Image>().sprite = configHandler.sprites[texture.index];
            instance.transform.GetChild(1).GetComponent<TMP_Text>().text = texture.currency == Currency.COIN ? $"{texture.price} <sprite index=0>" : $"{texture.price} $";

            instance.transform.GetChild(2).gameObject.SetActive(playerData.materialsOwned[texture.index] == '1');

            Button button = instance.GetComponent<Button>();

            button.interactable = playerData.materialsOwned[texture.index] == '0';

            if (button.interactable)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    if (texture.currency == Currency.COIN)
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

                                int current = playerData.coins;

                                playerData.coins -= texture.price;
                                playerData.materialsOwned = playerData.materialsOwned.Remove(texture.index, 1).Insert(texture.index, "1");

                                StartCoroutine(AnimationManager.AnimateCoinText(coinsTxt, current, playerData.coins));

                                AchievementManager.CheckAchievements(playerData, toastManager);

                                playerData.SaveData();

                                instance.transform.GetChild(2).gameObject.SetActive(playerData.materialsOwned[texture.index] == '1');
                                button.interactable = playerData.materialsOwned[texture.index] == '0';
                            });
                        }
                        else
                        {
                            OpenNotEnoughPanel();
                        }
                    }
                    else
                    {
                        confirmPanel.gameObject.SetActive(true);
                        buttonClickSfx.Play();
                        confirmPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", true);

                        confirmPanelTxt.text = $"Are you sure you want to buy Texture #{texture.index + 1} for {texture.price} dollar?";

                        confirmPanelOkButton.onClick.RemoveAllListeners();
                        confirmPanelOkButton.onClick.AddListener(() =>
                        {
                            confirmPanelOkButton.interactable = false;

                            iAPV5Manager.Buy(texture.id, (id) =>
                            {
                                confirmPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", false);
                                itemBoughtSfx.Play();
                                StartCoroutine(DelayedPanelClose(confirmPanel));

                                playerData.materialsOwned = playerData.materialsOwned.Remove(texture.index, 1).Insert(texture.index, "1");

                                AchievementManager.CheckAchievements(playerData, toastManager);

                                playerData.SaveData();

                                instance.transform.GetChild(2).gameObject.SetActive(playerData.materialsOwned[texture.index] == '1');
                                button.interactable = playerData.materialsOwned[texture.index] == '0';

                                confirmPanelOkButton.interactable = true;
                            }, () =>
                            {
                                confirmPanelOkButton.interactable = true;
                            });
                        });
                    }
                });
            }
        }
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
            if (confirmPanel.gameObject.activeSelf)
            {
                CloseConfirmPanel();
            }
            else if (notEnoughPanel.gameObject.activeSelf)
            {
                CloseNotEnoughPanel();
            }
            else
            {
                Back();
            }
        }
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
        StartCoroutine(SwitchScene("Submenu"));
    }

    private IEnumerator SwitchScene(string name)
    {
        crossfade.GetComponent<CanvasGroup>().blocksRaycasts = true;
        crossfade.SetBool("isOpen", true);
        yield return new WaitForSecondsRealtime(0.3f);
        SceneManager.LoadScene(name);
    }
}
