using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
public class ClickToSelectItem : MonoBehaviour
{
    [SerializeField]
    GameObject      itemSpawnedPrefab;
    [SerializeField]
    SpriteRenderer itemSR;
    public bool            canSpawn;
    [SerializeField]
    Sprite          highlightedSprite;
    [SerializeField]
    Sprite          defaultItemSprite;
    //Testing for click to go to counter

    //Set the location where the item will go
    [SerializeField] 
    Transform       targetPosition;
    [SerializeField]
    float animationTravelTime;
    [SerializeField] ParticleSystem hintingSparklePFX;
    [SerializeField] ParticleSystem radiatePFX;

    private void Start()
    {
        canSpawn = true;
    }

    private void OnMouseOver()
    {
        HighlightItem();

        //If the item is clicked
        //Spawn one at a time
        if (GameManager.instance.isPlaying)
        {
            if (!CursorManager.instance.isLooping)
            {
               
                CursorManager.instance.SetActiveCursorAnimation(CursorType.HoverItem);
            }
       
            if (Input.GetMouseButtonDown(0) && canSpawn)
            {
                CursorManager.instance.PlayCursorAnimation(CursorType.ClickItem, CursorType.Arrow);
                if (GameManager.instance.orderSheetShowing)
                {
                    Debug.Log("Order Sheet is Active");
                    
                    return;
                }
                SpawnItem();
                canSpawn = false;

                if (TutorialManager.instance)
                {
                    if (TutorialManager.instance.tutorialQuestActive && TutorialManager.instance.canTutorial)
                    {
                        if (TutorialManager.instance.tutorials.IndexOf(TutorialManager.instance.currentTutorial) == 4)
                        {
                            TutorialManager.instance.ToggleTutorialQuest();
                            TutorialManager.instance.StartTimeline();
                        }

                    }
                }
                
                
               
                
             
            }
            if (Input.GetMouseButtonUp(0) && !canSpawn)
            {
                canSpawn = true;
            }
        }
       

    }
    private void OnMouseExit()
    {
    
        CursorManager.instance.SetActiveCursorAnimation(CursorType.Arrow);
        RemoveHighlightItem();
    }

    void HighlightItem()
    {
        //Sprite change to highlighted 
       
        //Change the current sprite to the highlighted sprite 
        if (highlightedSprite != null && itemSR != null) itemSR.sprite = highlightedSprite;
        if (hintingSparklePFX && !hintingSparklePFX.isPlaying) hintingSparklePFX.Play();
        if (radiatePFX && !radiatePFX.isPlaying) radiatePFX.Play();
        
    }

    void RemoveHighlightItem()
    {
        
        if (defaultItemSprite != null && itemSR != null) itemSR.sprite = defaultItemSprite;
        if (hintingSparklePFX) hintingSparklePFX.Stop();
        if (radiatePFX) radiatePFX.Stop();
        
       
    }

    void SpawnItem()
    {
        GameObject spawnedItem = Instantiate(itemSpawnedPrefab, this.transform.position, Quaternion.identity);
        //Animate to go to the counter 
        spawnedItem.GetComponent<Rigidbody2D>().transform.DOMove(targetPosition.position, 1.0f);

        spawnedItem.GetComponent<Rigidbody2D>().transform.DOMove(targetPosition.position, animationTravelTime);
    }
}
