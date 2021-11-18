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

    public void OnEnable()
    {
        inputField.onSubmit.AddListener(NameEntered);
    }
    public void NameEntered(string p_name)
    {
        PlayerManager.instance.playerName = p_name;
        UIManager.instance.StartCoroutine(Scoring.instance.ShutterDownEffect(UIManager.instance.playerNameUI, StartGame));

        
    }

    public void StartGame()
    {
        GameManager.instance.StartCoroutine(GameManager.instance.DayStart());
    }

    public void NameEnteredButton()
    {
    
        PlayerManager.instance.playerName = nameValue.text;
        UIManager.instance.StartCoroutine(Scoring.instance.ShutterDownEffect(UIManager.instance.playerNameUI, StartGame));

    }


    
}
