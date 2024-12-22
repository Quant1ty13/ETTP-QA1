using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ApplyChanges : MonoBehaviour
{
    // Resolution Size
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    public Resolution resolutionchange;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;

    // Music
    public AudioSource music;
    public Slider musicSlider;
    public float musicvolume;

    public void SetMusicVolume(float volume) { musicvolume = volume; }
    public void SetResolution(int resolutionIndex) { Resolution resolution = filteredResolutions[resolutionIndex]; resolutionchange = resolution; }
    public void SetFullScreen(bool fs) { FullScreen = fs; }

    // Checkbox
    public Toggle checkbox;
    public bool FullScreen;

    private void Start()
    {
        music = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();

        // Resetting values
        music.volume = 1;
        musicSlider.value = 1;
        music.volume = PlayerPrefs.GetFloat("MusicVolume");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        checkbox.isOn = FullScreen;

        // Resolution Size
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = (float)Screen.currentResolution.refreshRateRatio.value;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if ((float)resolutions[i].refreshRateRatio.value == currentRefreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        filteredResolutions.Sort((a, b) => {
            if (a.width != b.width)
                return b.width.CompareTo(a.width);
            else
                return b.height.CompareTo(a.height);
        });

        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRateRatio.value.ToString("0.##") + " Hz"; // Ondalık basamak sınırlandı
            options.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height && (float)filteredResolutions[i].refreshRateRatio.value == currentRefreshRate) // double'dan float'a dönüştürüldü
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex = 0;
        resolutionDropdown.RefreshShownValue();
        SetResolution(currentResolutionIndex);
    }

    public void ApplyChange()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicvolume);
        music.volume = PlayerPrefs.GetFloat("MusicVolume");
        Screen.SetResolution(resolutionchange.width, resolutionchange.height, FullScreen);
    }
}
