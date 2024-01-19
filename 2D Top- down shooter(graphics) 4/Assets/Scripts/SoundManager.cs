using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace MyGame
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }
        [SerializeField] AudioMixer mixer;
        [SerializeField] Slider musicSlider;
        [SerializeField] Slider sfxSlider;
        const string MIXER_MUSIC = "MusicVolume";
        const string MIXER_SFX = "SFXVolume";
        float _musicVolume;
        float _sfxVolume;
        AudioSource audS;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.Log("SoundManager instance already exists. Destroying this one.");
                Destroy(gameObject);
            }
            audS = GetComponent<AudioSource>();
            // Загрузка сохраненных значений при запуске
            musicSlider.value = PlayerPrefs.GetFloat(MIXER_MUSIC, 0.75f);
            sfxSlider.value = PlayerPrefs.GetFloat(MIXER_SFX, 0.75f);

            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
        void Start()
        {
            // Установка громкости при запуске сцены
            SetMusicVolume(musicSlider.value);
            SetSFXVolume(sfxSlider.value);
        }
        public void PlayerSound(AudioClip value)
        {
            if (Instance == null)
            {
                Debug.LogError("SoundManager.Instance is null. Make sure the SoundManager script is set up correctly.");
                return;
            }

            if (audS == null)
            {
                Debug.LogError("AudioSource is null. Make sure it is assigned in the inspector.");
                return;
            }

            audS.pitch = Random.Range(0.9f, 1.1f);
            audS.PlayOneShot(value);
        }

        public void SetVolume(string name, float value)
        {
            mixer.SetFloat(name, Mathf.Log10(value) * 20);
            PlayerPrefs.SetFloat(name, value);
        }

        void SetMusicVolume(float value)
        {
            mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
            PlayerPrefs.SetFloat(MIXER_MUSIC, value); // Сохранение значения
        }

        void SetSFXVolume(float value)
        {
            mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
            PlayerPrefs.SetFloat(MIXER_SFX, value); // Сохранение значения
        }

        private void OnApplicationQuit()
        {
            PlayerPrefs.Save(); // Сохранение всех изменений при выходе из игры
        }
    }
}