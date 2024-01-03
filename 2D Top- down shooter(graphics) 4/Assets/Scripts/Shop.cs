using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    [SerializeField] Button[] buyButtons;
    [SerializeField] TextMeshProUGUI[] boughtTexts;
    [SerializeField] int[] prices;

    [SerializeField] GameObject shopPanel;

    public delegate void BuySeconPosition();
    public event BuySeconPosition buySeconPosition;

    public static Shop instance;

    [SerializeField] AudioClip popSound, succesBuyClip;

    private void Awake()
    {
        instance = this;
        DeleteShopData(); // Удалить все сохраненные данные магазина
    }

    private void Start()
    {
        InitializeShop();
    }

    private void Update()
    {
        HandleShopPanelToggle();
    }

    void InitializeShop()
    {
        for (int i = 0; i < buyButtons.Length; i++)
        {
            int position = PlayerPrefs.GetInt("Position" + i, 0);
            if (position == 1)
            {
                MarkAsBought(i);
            }
        }

        Check();
    }

    void HandleShopPanelToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            shopPanel.SetActive(!shopPanel.activeInHierarchy);
            Check();

            SoundManager.instance.PlayerSound(popSound);

            if (shopPanel.activeInHierarchy) Time.timeScale = 0;
            else Time.timeScale = 1; Cursor.visible = false;
        }
    }

    void Check()
    {
        for (int i = 0; i < buyButtons.Length; i++)
        {
            if (PlayerPrefs.GetInt("Position" + i) == 1)
            {
                SetButtonState(i, false, "Купленно");
            }
            else if (Player.instance.currentMoney < prices[i])
            {
                SetButtonState(i, false, "Мало монет");
            }
            else
            {
                SetButtonState(i, true, "Купить");
            }
        }
    }

    void SetButtonState(int index, bool interactable, string text)
    {
        buyButtons[index].interactable = interactable;
        boughtTexts[index].text = text;
    }

    public void Buy(int index)
    {
        if (Player.instance.currentMoney >= prices[index])
        {
            MarkAsBought(index);
            Player.instance.AddMoney(-prices[index]);
            Check();
        }
    }

    void MarkAsBought(int index)
    {
        buyButtons[index].interactable = false;
        boughtTexts[index].text = "Купленно";
        PlayerPrefs.SetInt("Position" + index, 1);
        PlayerPrefs.Save(); // Сохранить изменения

        if (index == 2 && buySeconPosition != null) buySeconPosition.Invoke();
    }

    void DeleteShopData()
    {
        for (int i = 0; i < buyButtons.Length; i++)
        {
            PlayerPrefs.DeleteKey("Position" + i);
        }
    }

    [ContextMenu("Delete Player Prefs")]
    void DeletePlayerPrefs() => PlayerPrefs.DeleteAll();
}
