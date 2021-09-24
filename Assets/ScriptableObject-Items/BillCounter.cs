using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BillCounter : MonoBehaviour, IPointerDownHandler
{
  
  public  bool isChangeUIActive;
    

    private void Start()
    {
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //TUTORIAL
        //if (!isChangeUIActive && !GameManager.instance.customer.willBuy)
        //{
        //    isChangeUIActive = true;
        //    ChangeUI();
            
        //    //if (TutorialManager.instance)
        //    //{
        //    //    if (TutorialManager.instance.tutorialQuestActive)
        //    //    {
        //    //        if (TutorialManager.instance.tutorials.IndexOf(TutorialManager.instance.currentTutorial) == 4)
        //    //        {
        //    //            TutorialManager.instance.ToggleTutorialQuest();
        //    //            TutorialManager.instance.StartTimeline();
        //    //        }
        //    //    }
        //    //}
        //}
    }

    void ChangeUI()
    {
        
        TransitionManager.instances.MoveTransition(new Vector2(-523f, 0), 1f, TransitionManager.instances.changeTransform, TransitionManager.instances.changeTransform.gameObject, true);

    }
}
