using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowEnablers : MonoBehaviour
{
    // Start is called before the first frame update

    public TutorialManager tutorial;

    public TutorialPhrase nextPhase;


 
    private void OnEnable()
    {
     //   if (tutorial.counter <= tutorial.setOfInstructions.Count)
       if(tutorial.counter <= tutorial.tutorialTexts[tutorial.tutorialCounter].instructions.Count/* && !tutorial.tutorialTexts[tutorial.tutorialCounter].hasArrow[tutorial.counter]*/)
            tutorial.tutorialPhrase = nextPhase;
        else
            gameObject.SetActive(false);
    }

    private void OnDisable()
    {
       
    }


}
