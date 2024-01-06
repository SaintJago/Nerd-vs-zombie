using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class MainMenu : MonoBehaviour
{
    public GameObject languagePanel; // Панель для кнопок языка
    public GameObject languageButtonPrefab; // Префаб кнопки языка

    // Словарь для хранения локализованных имен языков
    private Dictionary<string, string> localizedLanguageNames = new Dictionary<string, string>
    {
        {"en", "English"},
        {"ru", "Русский"},
        {"uk", "Українська"}
    };

    // Загрузка уровня
    public void LoadLevel()
    {
        SceneManager.LoadScene("Level1");
    }

    // Выход из игры
    public void ExitGame()
    {
        Application.Quit();
    }

    // Открытие панели языка
    public void OpenLanguagePanel()
    {
        // Удалить все текущие кнопки
        foreach (Transform child in languagePanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Добавить кнопки для каждого доступного языка
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            GameObject button = Instantiate(languageButtonPrefab, languagePanel.transform);
            string languageCode = locale.Identifier.Code;
            string languageName = localizedLanguageNames.ContainsKey(languageCode) ? localizedLanguageNames[languageCode] : locale.name;
            button.GetComponentInChildren<TextMeshProUGUI>().text = languageName; // Установка текста кнопки
            button.GetComponent<Button>().onClick.AddListener(() => ChangeLanguage(locale.Identifier.Code));
        }

        // Показать панель
        languagePanel.SetActive(true);
    }

    // Изменение языка
    public void ChangeLanguage(string localeIdentifier)
    {
        var locale = LocalizationSettings.AvailableLocales.Locales.FirstOrDefault(l => l.Identifier.Code == localeIdentifier);
        if (locale != null)
        {
            LocalizationSettings.SelectedLocale = locale;
        }
        languagePanel.SetActive(false);
    }
}
