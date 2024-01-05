using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject languagePanel; // Панель для кнопок языка
    public GameObject languageButtonPrefab; // Префаб кнопки языка

    public void LoadLevel()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

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
            button.GetComponentInChildren<Text>().text = locale.name;
            button.GetComponent<Button>().onClick.AddListener(() => ChangeLanguage(locale.Identifier.Code));
        }

        // Показать панель
        languagePanel.SetActive(true);
    }

    public void ChangeLanguage(string localeIdentifier)
    {
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            if (locale.Identifier.Code == localeIdentifier)
            {
                LocalizationSettings.SelectedLocale = locale;
                break;
            }
        }
    }
}
