using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Playables;


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

    public PlayableDirector director;
    public GameObject controlPanel;

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
       
        director.played += Director_Played;
        director.stopped += Director_Stopped;
    }

    public void OnEnable()
    {
        // OnTutorialSectionEnded += TutorialPanelStart;
        if (PlayerManager.instance)
        {

            PlayerManager.OnMouseClick += StartTimeline;
        }
    }
    public void OnDisable()
    {
        // OnTutorialSectionEnded -= TutorialPanelStart;
        if (PlayerManager.instance)
        {

            PlayerManager.OnMouseClick -= StartTimeline;
        }
    }
    public void Start()
    {

        currentTutorialSection = (TutorialSection)0;
        
        


    }
    public void Director_Played(PlayableDirector obj)
    {
        controlPanel.SetActive(false);
    }

    private void Director_Stopped(PlayableDirector obj)
    {
        controlPanel.SetActive(true);
    }

    public void StartTimeline()
    {
        Debug.Log("ITS STARTING");
        
        director.Play();
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
