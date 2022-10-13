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
        // Load from player prefs
    }

    private void SaveData()
    {
        // Save data to Player prefs
    }

    private void OnDestroy()
    {
        SaveData();
    }
}
