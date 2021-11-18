using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _uiResolutionDropdown = default;
    [SerializeField] private TMP_Dropdown _uiWindowDropdown = default;

    [SerializeField] private Slider _uiMusicSlider = default;
    [SerializeField] private Slider _uiSoundSlider = default;

    [SerializeField] private Button _uiBackButton;

    private void Start()
    {
        SetResolutionOptions();
        SetMusicVolume();
        SetSoundVolume();
    }

    private void OnEnable()
    {
        _uiBackButton.onClick.AddListener(OpenMainMenu);
        _uiMusicSlider.onValueChanged.AddListener(MusicVolumeChanged);
        _uiSoundSlider.onValueChanged.AddListener(SoundVolumeChanged);
        _uiResolutionDropdown.onValueChanged.AddListener(ResolutionChanged);
        _uiWindowDropdown.onValueChanged.AddListener(ResolutionChanged);
    }

    private void OnDisable()
    {
        _uiBackButton.onClick.RemoveListener(OpenMainMenu);
        _uiMusicSlider.onValueChanged.RemoveListener(MusicVolumeChanged);
        _uiSoundSlider.onValueChanged.RemoveListener(SoundVolumeChanged);
        _uiResolutionDropdown.onValueChanged.RemoveListener(ResolutionChanged);
        _uiWindowDropdown.onValueChanged.RemoveListener(ResolutionChanged);
    }

    private void SetMusicVolume()
    {
        _uiMusicSlider.value = PlayerPrefs.GetInt("music", 100) / 100;
    }

    private void SetSoundVolume()
    {
        _uiSoundSlider.value = PlayerPrefs.GetInt("sound", 100) / 100;
    }

    private void SetResolutionOptions()
    {
        if (_uiResolutionDropdown.options.Count > 0 || _uiWindowDropdown.options.Count > 0)
        {
            return;
        }

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        Resolution[] userSupportedResolution = Screen.resolutions;
        int indexRes = -1;
        for (int i = 0; i < userSupportedResolution.Length; i++)
        {
            if (Screen.currentResolution.Equals(userSupportedResolution[i]))
            {
                indexRes = i;
            }
            TMP_Dropdown.OptionData data =
                new TMP_Dropdown.OptionData($"{userSupportedResolution[i].width} x {userSupportedResolution[i].height} x {userSupportedResolution[i].refreshRate}Hz");
            options.Add(data);
        }
        _uiResolutionDropdown.AddOptions(options);
        _uiResolutionDropdown.SetValueWithoutNotify(indexRes);

        List<TMP_Dropdown.OptionData> windowOptions = new List<TMP_Dropdown.OptionData>();
        foreach(string name in Enum.GetNames(typeof(FullScreenMode)))
        {
            windowOptions.Add(new TMP_Dropdown.OptionData(name));
        }
        _uiWindowDropdown.AddOptions(windowOptions);
        _uiWindowDropdown.SetValueWithoutNotify((int)Screen.fullScreenMode);
    }

    private void ResolutionChanged(int i)
    {
        Resolution[] res = Screen.resolutions;
        Screen.SetResolution(res[_uiResolutionDropdown.value].width,
            res[_uiResolutionDropdown.value].height, (FullScreenMode)_uiWindowDropdown.value);
    }

    private void MusicVolumeChanged(float value)
    {
        PlayerPrefs.SetInt("music", (int) (value * 100f));
    }

    private void SoundVolumeChanged(float value)
    {
        PlayerPrefs.SetInt("sound", (int)(value * 100f));
    }

    private void OpenMainMenu()
    {
        this.gameObject.SetActive(false);
    }
}
