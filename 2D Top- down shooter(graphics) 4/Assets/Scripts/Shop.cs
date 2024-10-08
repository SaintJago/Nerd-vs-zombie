﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;// Добавьте это

public class Shop : MonoBehaviour
{
    [SerializeField] Button[] buyButtons; // Ваши кнопки покупки
    [SerializeField] TextMeshProUGUI[] boughtTexts; // Текстовые поля для отображения состояния покупки
    [SerializeField] int[] prices; // Цены на ваши товары

    [SerializeField] GameObject shopPanel; // Панель магазина

    public delegate void BuySeconPosition();
    public event BuySeconPosition buySeconPosition;

    public static Shop instance;

    [SerializeField] AudioClip popSound, succesBuyClip; // Звуки

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
        if (shopPanel.activeInHierarchy)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.visible = false;
        }
    }
  }


    void Check()
    {
        for (int i = 0; i < buyButtons.Length; i++)
        {
            if (PlayerPrefs.GetInt("Position" + i) == 1)
            {
                SetButtonState(i, false, "Bought");
            }
            else if (Player.instance.currentMoney < prices[i])
            {
                SetButtonState(i, false, "NotEnoughCoins");
            }
            else
            {
                SetButtonState(i, true, "Buy");
            }
        }
    }

    async void SetButtonState(int index, bool interactable, string key)
    {
        buyButtons[index].interactable = interactable;
        var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI", key);
        boughtTexts[index].text = await op.Task;
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
