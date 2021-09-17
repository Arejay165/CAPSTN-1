using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TutorialPanel : MonoBehaviour
{
   
    public TutorialSection tutorialSection;
    public List<GameObject> tutorialTexts = new List<GameObject>();
    public int currentIndex = 0;
   
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        //For Composition Structure (UIManager doesnt need to know if he has this specific instance, this instance will just add itself)
        if (UIManager.instance)
        {
            UIManager.instance.AddTutorialPanel(gameObject);
        }
        else
        {
            Debug.Log("No UIManager was found in the Canvas");
        }
        
    }

    private void OnEnable()
    {
        if (TutorialManager.instance)
        {
          
            //PlayerManager.OnMouseClick += SelectTutorialPanel;
        }
        ShowText();
    
    }

    public void OnDisable()
    {
        if (TutorialManager.instance)
        {
          
            //PlayerManager.OnMouseClick -= SelectTutorialPanel;
        }
        currentIndex = 0;

    }
    
    private void ShowText()
    {
        foreach (GameObject selectedTutorialText in tutorialTexts)
        {
            selectedTutorialText.SetActive(false);
        }
        tutorialTexts[currentIndex].SetActive(true);
     
    }

    public void SelectTutorialPanel()
    {
        Debug.Log("NEXT FRAME");
        if (currentIndex >= 0 && currentIndex < tutorialTexts.Count - 1) //If there are still tutorialTexts left to show
        {
            currentIndex++;
            ShowText();
        }
        else // If there are no tutorialTexts left to show
        {

            TutorialManager.instance.TutorialDialogueEnd();

        }


    }
    

}
