using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class Settings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public string selectedLanguage;
    public string[] languages;
    Resolution[] resolutions;
    public Dropdown resolutionDropdown;
    private void Start()
    {

        selectedLanguage = languages[0];
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i< resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int p_resolutionIndex)
    {
        Resolution resolution = resolutions[p_resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float p_volume)
    {
        audioMixer.SetFloat("volume", p_volume); ;
    }

    public void SetQuality(int p_qualityIndex)
    {
        QualitySettings.SetQualityLevel(p_qualityIndex);
    }

    public void SetFullScreen(bool p_isFullscreen)
    {
        Screen.fullScreen = p_isFullscreen;
    }

    public void SetLanguage(int p_languageIndex)
    {
        selectedLanguage = languages[p_languageIndex];
    }

 
}
