using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class GameSoundController : MonoBehaviour
{
    private const string basicText = "????: *%";
    private const float basicVolume = 50.0f;
    private const string volumeValue = "Volume";

    [SerializeField] TextMeshProUGUI textUI;
    [SerializeField] GameObject mainAudioPrefab;

    private AudioSource mainAudioSource;
    private Slider slider;

    private float currentVolume;

    public void SaveVolumeData()
    {
        PlayerPrefs.SetFloat(volumeValue, currentVolume);
    }

    public void SaveVolumeData(float volume)
    {
        PlayerPrefs.SetFloat(volumeValue, volume);
    }

    public void LoadVolumeData()
    {
        currentVolume = PlayerPrefs.GetFloat(volumeValue);
    }

    private void Awake()
    {
        StartSound();
    }

    private void ChangeVolume()
    {
        currentVolume = slider.value;
        mainAudioSource.volume = currentVolume;
        ChangeVolumeText();
    }

    private void CreateAudioSource()
    {
        mainAudioSource = Instantiate(mainAudioPrefab).GetComponent<AudioSource>();
    }

    public void StartSound()
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

    private void SetVolume()
    {
        slider.value = currentVolume;
        ChangeVolume();
    }

    private void ChangeVolumeText()
    {
        int volumeInt = (int)(slider.value * 100.0f);
        string newVolumeText = basicText.Replace("*", volumeInt.ToString());
        textUI.text = newVolumeText;
    }
}
