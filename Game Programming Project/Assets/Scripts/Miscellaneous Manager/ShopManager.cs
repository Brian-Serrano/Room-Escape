using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [HideInInspector] public static ShopManager instance;

    private GameObject[] slots;

    public GameObject clickingBehindAvoider;

    public GameObject confirm;
    public GameObject insufficient;
    public TMP_Text confirmText;

    [HideInInspector] public int number;
    [HideInInspector] public int coinNumber;
    [HideInInspector] public int index;

    void Start()
    {
        instance = this;

        slots = GameObject.FindGameObjectsWithTag("Shop");

        for(int i = 0; i < slots.Length; i++)
        {
            Button button = slots[i].GetComponent<Button>();
            string numberString = slots[i].name.Substring("Texture ".Length);
            int number = int.Parse(numberString);
            GameObject coin = slots[i].transform.GetChild(0).GetChild(0).gameObject;
            string coinString = coin.GetComponent<TMP_Text>().text;
            int coinNumber = int.Parse(coinString);
            if (CheckBoughtItems(number))
            {
                int index = i;
                button.onClick.AddListener(() => ShowConfirm(number, coinNumber, index));
            }
            else
            {
                slots[i].transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    private void ShowConfirm(int number, int coinNumber, int index)
    {
        SFXManager.instance.PlaySFX(0);
        if (TemporaryData.instance.coins >= coinNumber)
        {
            PanelAnimation.instance.AnimatePanel("Open", confirm, 0.25f, clickingBehindAvoider);
            confirmText.text = "Are you sure you want to buy Texture #" + number + " for " + coinNumber + " coins";
            this.number = number;
            this.coinNumber = coinNumber;
            this.index = index;
        }
        else
        {
            PanelAnimation.instance.AnimatePanel("Open", insufficient, 0.25f, clickingBehindAvoider);
        }
    }

    private bool CheckBoughtItems(int number)
    {
        return !(TemporaryData.instance.ownedMaterials[number - 1] == 1);
    }

    public void SetBought(int index)
    {
        slots[index].GetComponent<Button>().onClick.RemoveAllListeners();
        slots[index].transform.GetChild(1).gameObject.SetActive(true);
    }
}
