using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions; // needed for Regex
using TMPro;
public class PlayerNameUI : MonoBehaviour
{
    public TextMeshProUGUI nameValue;
    public TMP_InputField inputField;
    public bool firstpress = false;
    public void OnEnable()
    {
        inputField.onSubmit.AddListener(NameEntered);
    }
    public void NameEntered(string p_name)
    {
        if (!firstpress)
        {
            firstpress = true;
            PlayerManager.instance.playerName = p_name;
            UIManager.instance.StartCoroutine(Scoring.instance.QuickShutterEffect(UIManager.instance.playerNameUI, UIManager.instance.PlayIntro));
        }


        
    }

    public void NameEnteredButton()
    {
        if (!firstpress)
        {
            firstpress = true;
            PlayerManager.instance.playerName = nameValue.text;
            UIManager.instance.StartCoroutine(Scoring.instance.QuickShutterEffect(UIManager.instance.playerNameUI, UIManager.instance.PlayIntro));
        }
    }


    
}
