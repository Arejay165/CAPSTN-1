using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[System.Serializable]
public enum TutorialSection
{
    none = -1,
    tutorialStart,
    tutorialDrag,
    tutorialtest,
    reset,
    

}
public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    public TutorialSection currentTutorialSection;
    public static event Action OnTutorialSectionStarted;
    public static event Action OnTutorialDialogueEnded;
    public static event Action OnTutorialSectionEnded;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

    }

    public void OnEnable()
    {
       // OnTutorialSectionEnded += TutorialPanelStart;

    }
    public void OnDisable()
    {
       // OnTutorialSectionEnded -= TutorialPanelStart;
    }
    public void Start()
    {

        currentTutorialSection = (TutorialSection)0;
        
        


    }
    public void TutorialSectionStart()
    {
        OnTutorialSectionStarted.Invoke();
    }
   
    public void TutorialDialogueEnd()
    {

        OnTutorialDialogueEnded.Invoke();


    }
    public void TutorialSectionEnd()
    {
       
        Debug.Log((int)currentTutorialSection + " - " + (UIManager.instance.tutorialUIs.Count));
        if ((int)currentTutorialSection >= 0 && (int)currentTutorialSection < UIManager.instance.tutorialUIs.Count)
        {

            int i = (int)currentTutorialSection;
            i++;
            currentTutorialSection = (TutorialSection)i;
            Debug.Log("NEW! " + (int)currentTutorialSection + " - " + (UIManager.instance.tutorialUIs.Count));
            OnTutorialSectionEnded.Invoke();
        }
        else
        {
            currentTutorialSection = (TutorialSection)0;
        }

    }

}
