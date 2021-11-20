using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CheatManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject cheatUI;
    public TMP_InputField inputfield;
    public string cheatCode = "MATHDAYAAKO";
    public string playerInputString;
   // public AudioSource ingame

    private void Start()
    {
        inputfield.onSubmit.AddListener(CheatEntered);
       
    }

    public void OnEnable()
    {
        inputfield.Select();
    }

    public void CheatEntered(string input)
    {
        playerInputString = input;
        if (playerInputString == cheatCode)
        {
            Debug.Log("You've guessed correctly");
            //Day closed proceed to day 2 
            Scoring.instance.isSkip = true;
            AudioManager.instance.stopMusic(1);
            AudioManager.instance.stopMusic(3);

            GameManager.instance.StartCoroutine(GameManager.instance.DayEnd());
        }
        else
        {
            //GameManager.instance.
            UIManager.instance.Resume();
        }

        playerInputString = "";
        inputfield.text = "";
    }

    public void EnterCheatCode()
    {
        playerInputString = inputfield.text;
         
        if(playerInputString == cheatCode)
        {
            Debug.Log("You've guessed correctly");
            //Day closed proceed to day 2 
            Scoring.instance.isSkip = true;
            AudioManager.instance.stopMusic(1);
            AudioManager.instance.stopMusic(3);
        
            GameManager.instance.StartCoroutine(GameManager.instance.DayEnd());
        }
        else
        {
            //GameManager.instance.
            UIManager.instance.Resume();
        }

        playerInputString = "";
        inputfield.text = "";
    }




}
