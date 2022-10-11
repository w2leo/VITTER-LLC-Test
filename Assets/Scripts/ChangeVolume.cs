using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Slider))]

public class ChangeVolume : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    private Slider slider;
    private const string basicText = "Звук: *%";

    private void Start()
    {
        slider = GetComponent<Slider>();
    }
    public void ChangeVolumeText()
    {
        int currentVolume = (int) (slider.value * 100.0f);
        string newText = basicText.Replace("*", currentVolume.ToString());
        text.text = newText;
    }
}
