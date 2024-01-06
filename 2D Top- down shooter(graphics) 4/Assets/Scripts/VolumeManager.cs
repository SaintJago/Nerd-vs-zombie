using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    private static VolumeManager instance;

    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string MusicPref = "MusicPref";
    private static readonly string SoundEffectsPref = "SoundEffectsPref";
    private int firstPlayInt;
    public Slider musicSlider, soundEffectsSlider;
    private float musicFloat, soundEffectsFloat;
    public List<AudioSource> musicAudios;
    public List<AudioSource> soundEffectsAudios;

    private void Awake()
    {
      Debug.Log("VolumeManager Awake");
      if (instance == null)
      {
          instance = this;
          DontDestroyOnLoad(gameObject);
      }
      else
      {
          Destroy(gameObject);
          Debug.Log("VolumeManager Destroyed");
      }
    }

    // Start is called before the first frame update
    void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

        if (firstPlayInt == 0) 
        {
            musicFloat = 0.25f;
            soundEffectsFloat = 0.75f;
            musicSlider.value = musicFloat;
            soundEffectsSlider.value = soundEffectsFloat;
            PlayerPrefs.SetFloat(MusicPref, musicFloat);
            PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsFloat);
            PlayerPrefs.SetInt(FirstPlay, -1);
            musicSlider.onValueChanged.AddListener(delegate { UpdateSound(); });
            soundEffectsSlider.onValueChanged.AddListener(delegate { UpdateSound(); });
        }
        else
        {
            musicFloat = PlayerPrefs.GetFloat(MusicPref);
            musicSlider.value = musicFloat;
            soundEffectsFloat = PlayerPrefs.GetFloat(SoundEffectsPref);
            soundEffectsSlider.value = soundEffectsFloat;
        }
    }

    public void SaveSoundSettings() 
    {
        PlayerPrefs.SetFloat(MusicPref, musicSlider.value);
        PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsSlider.value);
    }

    void OnApplicationFocus(bool inFocus) 
    { 
        if (!inFocus)
        {
            SaveSoundSettings();
        }
    }

    public void UpdateSound() 
    {
        foreach (var audio in musicAudios)
        {
            audio.volume = musicSlider.value;
        }

        foreach (var audio in soundEffectsAudios)
        {
            audio.volume = soundEffectsSlider.value;
        }
    }
}
