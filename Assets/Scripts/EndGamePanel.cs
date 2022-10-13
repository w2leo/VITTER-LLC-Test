using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Transform menuPanels;
    [SerializeField] Button buttonClose;
 
    private const string winState = "œŒ¡≈ƒ¿";
    private const string looseState = "œŒ–¿∆≈Õ»≈";

    public void ActivateEndGame(bool state)
    {
        menuPanels.gameObject.SetActive(true);
        gameObject.SetActive(true);
        buttonClose.gameObject.SetActive(false);
        if (state)
        {
            text.text = winState;
        }
        else
        {
            text.text = looseState;
        }
    }
}
