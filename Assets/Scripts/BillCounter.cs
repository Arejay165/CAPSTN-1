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
       
        if (!TransitionManager.instances.noteBookTransform.gameObject.activeSelf && !TransitionManager.instances.changeTransform.gameObject.activeSelf && MathProblemManager.instance.GetGeneratedItemsWanted()[0].numValue !=0)//GameManager.instance.customer.willBuy)
        {
            
            GameManager.instance.customer.itemsImage[0].color = global::GameManager.instance.window.darkenImage;
            GameManager.instance.customer.itemsImage.RemoveAt(0);
            MathProblemManager.instance.GetCurrentItemsWanted().RemoveAt(0);
            MoodComponent mc = GameManager.instance.customer.GetComponent<MoodComponent>();
            mc.IncreaseCurrentMoodAmount(mc.correctBonusTime * 2);// 1 second

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
        //Disable customer bubble
        GameManager.instance.customer.panel.gameObject.SetActive(false);

        //show pabarya sheet
        TransitionManager.instances.MoveTransition(new Vector2(680f, 0f), 0.5f, TransitionManager.instances.changeTransform, TransitionManager.instances.changeTransform.gameObject, true);
   
    }
}
