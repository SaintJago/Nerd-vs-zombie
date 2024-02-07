using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteScript : MonoBehaviour
{
    public void OnLevelComplete()
    {
        // Возобновить игру
        PauseManager.ResumeGame();
        // Если уровень является последним разблокированным уровнем, увеличиваем количество разблокированных уровней и сохраняем это в PlayerPrefs
        if (LevelSelectionMenuManager.currLevel == LevelSelectionMenuManager.unlockedLevels)
        {
            LevelSelectionMenuManager.unlockedLevels++;
            PlayerPrefs.SetInt("unlockedLevels", LevelSelectionMenuManager.unlockedLevels);
        }

        // Загружаем сцену меню
        SceneManager.LoadScene("menu");
    }
}
