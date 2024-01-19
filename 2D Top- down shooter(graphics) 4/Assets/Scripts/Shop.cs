using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;// Добавьте это
using MyGame;

public class Shop : MonoBehaviour
{
    [SerializeField] Button[] buyButtons; // Ваши кнопки покупки
    [SerializeField] TextMeshProUGUI[] boughtTexts; // Текстовые поля для отображения состояния покупки
    [SerializeField] int[] prices; // Цены на ваши товары
    //[SerializeField] Button shopButton; // Ваша кнопка магазина

    [SerializeField] GameObject shopPanel; // Панель магазина

    public delegate void BuySeconPosition();
    public event BuySeconPosition buySeconPosition;

    public static Shop Instance;
    [SerializeField] Player player;


    [SerializeField] AudioClip popSound, succesBuyClip; // Звуки

    private void Awake()
    {
        Instance = this;
        DeleteShopData(); // Удалить все сохраненные данные магазина
    }

    private void Start()
    {
        InitializeShop();
        //shopButton.onClick.AddListener(OpenShop); // Добавьте это
    }

   // void OpenShop()
    //{
        //shopPanel.SetActive(true);
        //Check();
       // SoundManager.Instance.PlayerSound(popSound);
        //Time.timeScale = 0;
        //Cursor.visible = true;
    //}

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
            SoundManager.Instance.PlayerSound(popSound);
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
            else if (Player.Instance.currentMoney < prices[i])
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
        if (Player.Instance.currentMoney >= prices[index])
        {
            MarkAsBought(index);
            Player.Instance.AddMoney(-prices[index]);
            Check();
        }
    }

    void MarkAsBought(int index)
    {
        buyButtons[index].interactable = false;
        PlayerPrefs.SetInt("Position" + index, 1);
        PlayerPrefs.Save(); // Сохранить изменения

        if (index == 2 && buySeconPosition != null) buySeconPosition.Invoke();
        if (index == 4 && buySeconPosition != null)
        {
            buySeconPosition.Invoke();

            // Проверяем, что у игрока есть дрон
            if (PlayerPrefs.GetInt("Position4") == 1)
            {
                Player.Instance.droneInstance.SetActive(true);
                // Устанавливаем цель для дрона
                DroneMovement droneMovement = Player.Instance.droneInstance.GetComponent<DroneMovement>();
                if (droneMovement != null)
                {
                    droneMovement.target = Player.Instance.transform;
                }
            }
        }
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
