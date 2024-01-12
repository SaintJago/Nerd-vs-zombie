using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Localization.Settings;

public class DeathPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] AudioClip popSound;

    private void Start()
    {
        WaveSpawner wsP = FindObjectOfType<WaveSpawner>();
        var waveString = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI", "Current wave");
        waveString.Completed += op => 
        {
          scoreText.text = op.Result + ": " + (wsP.currentWaveIndex + 1).ToString();
        };
    }

    public void Restart()
    {
        Time.timeScale = 1; // возобновляет игру
        SoundManager.instance.PlayerSound(popSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
