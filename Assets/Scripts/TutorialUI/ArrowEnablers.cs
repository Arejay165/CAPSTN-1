using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowEnablers : MonoBehaviour
{
    // Start is called before the first frame update

 


 
    private void OnEnable()
    {
           TutorialManager.instance.dialogueBox.anchoredPosition = TutorialManager.instance.convoTransform[1].anchoredPosition;
    }

    private void OnDisable()
    {
        TutorialManager.instance.dialogueBox.anchoredPosition = TutorialManager.instance.convoTransform[0].anchoredPosition;
    }


}
