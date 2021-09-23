using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Playables;



public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    public bool tutorialQuestActive = false;
    public bool canTutorial = true;
    public PlayableDirector currentTutorial;

    public List<PlayableDirector> tutorials;
    

 
   // public GameObject controlPanel;

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

    
    public void Start()
    {

      
    



    }

    public void StartTimeline()
    {
       
        if (canTutorial)
        {
            if (currentTutorial)
            {
                currentTutorial.gameObject.SetActive(false);
                //if (tutorialQuestActive)
                //{
                //    ToggleTutorialQuest();
                //}



            }

            if (tutorials.IndexOf(currentTutorial) < tutorials.Count - 1)
            {
                int i = 0;
                if (currentTutorial)
                {
                    i = tutorials.IndexOf(currentTutorial);
                    i++;
                }


                currentTutorial = tutorials[i];
                currentTutorial.gameObject.SetActive(true);
                currentTutorial.Play();
            }
            else
            {

                currentTutorial.gameObject.SetActive(false);
                currentTutorial = null;
                canTutorial = false;

            }


            //currentTutorial.timelineClip.gameObject.SetActive(false);
        }



    }

    public void ToggleTutorialQuest()
    {
      
        //tutorialQuestActive = true;
        tutorialQuestActive = tutorialQuestActive ? false : true;
    }


}
