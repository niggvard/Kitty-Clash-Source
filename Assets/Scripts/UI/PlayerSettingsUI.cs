using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSettingsUI : MonoBehaviour
{
    public static bool isVibrationOff;

    [SerializeField] private GameObject panel;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider fxSlider;
    [SerializeField] private Toggle vibrationToggle;

    private void Start()
    {
        float fxVolume = PlayerPrefs.GetFloat("fxVolume", 0f);
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 0f);
        isVibrationOff = PlayerPrefs.GetInt("Vibration", 0) == 1 ? true : false;

        musicSlider.value = musicVolume;
        fxSlider.value = fxVolume;
        vibrationToggle.isOn = isVibrationOff;

        mixer.SetFloat("fxVolume", fxVolume);
        mixer.SetFloat("musicVolume", musicVolume);
    }

    public void OnSliderChange(string key)
    {
        if (key == "fxVolume")
        {
            mixer.SetFloat(key, fxSlider.value);
            PlayerPrefs.SetFloat(key, fxSlider.value);
        }
        else
        {
            mixer.SetFloat(key, musicSlider.value);
            PlayerPrefs.SetFloat(key, musicSlider.value);
        }
    }

    public void SetVibration()
    {
        isVibrationOff = vibrationToggle.isOn;
        PlayerPrefs.SetInt("Vibration", isVibrationOff ? 1 : 0);
    }

    public void OnOpenButton()
    {
        bool newState = !panel.activeInHierarchy;
        panel.SetActive(newState);

        //if (SceneManager.GetActiveScene().name != "Main Menu")
        //    Time.timeScale = newState ? 0 : 1;
    }
}
