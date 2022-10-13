using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppInit : MonoBehaviour
{
    [SerializeField] GameSoundController soundController;

    private void Start()
    {
        DontDestroyOnLoad(this);
        LoadData();
        soundController.StartSound();
    }

    private void LoadData()
    {
        soundController.LoadVolumeData();
    }

    private void SaveData()
    {
        soundController.SaveVolumeData();
    }

    private void OnDestroy()
    {
        SaveData();
    }
}
