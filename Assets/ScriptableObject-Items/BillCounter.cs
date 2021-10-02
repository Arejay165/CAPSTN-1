using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BillCounter : MonoBehaviour, IPointerDownHandler
{
  
  

    private void Start()
    {
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {
       
        if (!TransitionManager.instances.changeTransform.gameObject.activeSelf && MathProblemManager.instance.GetGeneratedItemsWanted()[0].numValue !=0)//GameManager.instance.customer.willBuy)
        {

            GameManager.instance.customer.itemsImage[0].color = global::GameManager.instance.window.darkenImage;
            GameManager.instance.customer.itemsImage.RemoveAt(0);
            MathProblemManager.instance.GetCurrentItemsWanted().RemoveAt(0);
            
            ChangeUI();
            // TUTORIAL
            //if (TutorialManager.instance)
            //{
            //    if (TutorialManager.instance.tutorialQuestActive)
            //    {
            //        if (TutorialManager.instance.tutorials.IndexOf(TutorialManager.instance.currentTutorial) == 4)
            //        {
            //            TutorialManager.instance.ToggleTutorialQuest();
            //            TutorialManager.instance.StartTimeline();
            //        }
            //    }
            //}
        }
    }

    void ChangeUI()
    {
        
        TransitionManager.instances.MoveTransition(new Vector2(-523f, 0), 1f, TransitionManager.instances.changeTransform, TransitionManager.instances.changeTransform.gameObject, true);
   
    }
}
