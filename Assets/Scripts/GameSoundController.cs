using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class GameSoundController : MonoBehaviour
{
    [Header("Constants")]
    private const string basicText = "Звук: *%";
    private const float basicVolume = 50.0f;
    private const string volumeValue = "Volume";

    [Header("Components")]
    [SerializeField] TextMeshProUGUI textUI;
    [SerializeField] GameObject mainAudioPrefab;
    private AudioSource mainAudioSource;
    private Slider slider;

    [Header("Fields")]
    private float currentVolume;

    public void ChangeVolume()
    {
        currentVolume = slider.value;
        mainAudioSource.volume = currentVolume;
        ChangeVolumeText();
    }
    private void CreateAudioSource()
    {
        mainAudioSource = Instantiate(mainAudioPrefab).GetComponent<AudioSource>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadVolumeData();
        SetVolume();
    }

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(volumeValue))
        {
            SaveVolumeData(basicVolume);
        }
        slider = GetComponent<Slider>();
        if (!SetAudioSource())
        {
            CreateAudioSource();
        }
        LoadVolumeData();
        SetVolume();
    }

    private void OnDestroy()
    {
        SaveVolumeData(currentVolume);
    }

    private bool SetAudioSource()
    {
        var mainSound = FindObjectOfType<MainSoundComponent>();
        if (mainSound != null)
        {
            return mainSound.TryGetComponent<AudioSource>(out mainAudioSource);
        }
        return false;
    }

    private void LoadVolumeData()
    {
        currentVolume = PlayerPrefs.GetFloat(volumeValue);
    }

    private void SetVolume()
    {
        slider.value = currentVolume;
        ChangeVolume();
    }

    private void SaveVolumeData(float volume)
    {
        PlayerPrefs.SetFloat(volumeValue, volume);
    }

    private void ChangeVolumeText()
    {
        int volumeInt = (int)(slider.value * 100.0f);
        string newVolumeText = basicText.Replace("*", volumeInt.ToString());
        textUI.text = newVolumeText;
    }
}
