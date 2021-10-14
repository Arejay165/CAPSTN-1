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
        if (tutorial.counter <= tutorial.setOfInstructions.Count)
            tutorial.tutorialPhrase = nextPhase;
        else
            gameObject.SetActive(false);
    }

    private void OnDisable()
    {
       
    }


}
