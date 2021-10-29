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

    public void NameEntered(string p_name)
    {
        PlayerManager.instance.playerName = p_name;
        GameManager.instance.StartCoroutine(GameManager.instance.DayStart());
    }

    public void NameEnteredButton()
    {
    
        PlayerManager.instance.playerName = nameValue.text;
        GameManager.instance.StartCoroutine(GameManager.instance.DayStart());
    }


    
}
