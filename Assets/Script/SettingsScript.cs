using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    public TMPro.TMP_Dropdown resolutionDropdown;
    //public AudioSource audioSource;
    Resolution[] resolutions; // Membuat array resolusi

    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
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

    // Resolusi
    public void SetResolution(int ResolutionIndex) //Resolusi
    {
        Resolution resolution = resolutions[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetFullscreen(bool isFullscreen) //Fullscreen
    {
        Screen.fullScreen = isFullscreen;
    }

    // Grafik
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    // Audio
    //public void SetVolume(float volume) //Volume
    //{
    //    audioSource.volume = volume;
    //}
    //public void SetMuteAudio(bool isMuted) //Mute
    //{
    //    audioSource.mute = isMuted;
    //}

    // Print setting di Console
   // public void print(bool isPrint)
   // {
   //     if (isPrint == true)
   //     {
   //         Debug.Log("Setting saat ini : \n" + 
   //         "Resolusi = " + resolutionDropdown.value + "\n" +
   //         "Fullscreen = " + Screen.fullScreen + "\n" +
   //         "Grafik = " + QualitySettings.GetQualityLevel() + "\n" +
   //         "Volume = " + audioSource.volume + "\n" +
   //         "Muted = " + audioSource.mute);
   //     }
   //}

    //Set Active
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    //Quit
    public void QuitGame()
    {
        Application.Quit();
    }
}