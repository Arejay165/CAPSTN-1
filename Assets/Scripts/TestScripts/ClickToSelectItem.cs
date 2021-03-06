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


    private void OnEnable()
    {

        //Register OnGameStart Event in GameManager
        GameManager.OnGameStart += OnGameStarted;
        GameManager.OnGameEnd += OnGameEnded;
    }

    private void OnDisable()
    {
        //Deregister OnGameStart Event in GameManager
        GameManager.OnGameStart -= OnGameStarted;
        GameManager.OnGameEnd -= OnGameEnded;





    }


    void OnGameStarted()
    {
        CursorManager.instance.SetActiveCursorAnimation(CursorType.Arrow);
        RemoveHighlightItem();

        if (!TutorialManager.instance.canTutorial || TutorialManager.instance == null)
            canSpawn = true;
        else
            canSpawn = false;
    }
    void OnGameEnded()
    {
        RemoveHighlightItem();
    }
    private void OnMouseOver()
    {
       

        //If the item is clicked
        //Spawn one at a time
        if (GameManager.instance.isPlaying)
        {
            HighlightItem();
            if (!CursorManager.instance.isLooping)
            {
            
                CursorManager.instance.PlayCursorAnimation(CursorType.HoverItem);
            }
       
            if (Input.GetMouseButtonDown(0) && canSpawn)
            {
               
             //   Debug.Log("Item clicked");

                //CursorManager.instance.PlayCursorAnimation(CursorType.ClickItem, CursorType.Arrow);
                if (!TransitionManager.instances.noteBookTransform.gameObject.activeSelf && !TransitionManager.instances.changeTransform.gameObject.activeSelf)
                {
                    SpawnItem();
                    canSpawn = false;
                    StartCoroutine(Cooldown());
                }
                
                //canSpawn = false;

                //if (TutorialManager.instance)
                //{
                //    if (TutorialManager.instance.tutorialQuestActive && TutorialManager.instance.canTutorial)
                //    {
                //        if (TutorialManager.instance.tutorials.IndexOf(TutorialManager.instance.currentTutorial) == 4)
                //        {
                //            TutorialManager.instance.ToggleTutorialQuest();
                //            TutorialManager.instance.StartTimeline();
                //        }

                //    }
                //}
                
                
               
                
             
            }
            //if (Input.GetMouseButtonUp(0) && !canSpawn && !TutorialManager.instance.tutorial.gameObject.activeSelf)
            //{
            //    canSpawn = true;
            //}
         
        }


    }
    private void OnMouseExit()
    {
    
        //CursorManager.instance.SetActiveCursorAnimation(CursorType.Arrow);
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
     
        if (hintingSparklePFX ) hintingSparklePFX.Stop();
        CursorManager.instance.PlayCursorAnimation(CursorType.Arrow);
        StartCoroutine(SmoothStopRadiate());



    }

    IEnumerator SmoothStopRadiate()
    {
        if (radiatePFX != null)
        {
            if (radiatePFX.isPlaying)
            {
                var particles = new ParticleSystem.Particle[radiatePFX.main.maxParticles];
                var currentAmount = radiatePFX.GetParticles(particles);

                if (currentAmount <= 0)
                {
                    radiatePFX.Stop();
                }
                else
                {
                    for (int i = 0; i < currentAmount; i++)
                    {
                        if (particles[i].remainingLifetime < 0.35f)
                        {
                            radiatePFX.Stop();
                        }
                    }
                }

                yield return new WaitForSeconds(0.25f); // initially 0.5
                StartCoroutine(SmoothStopRadiate());
            }
        }
       
       
        

    }

    void SpawnItem()
    {
        if (UIManager.instance.inGameUI.activeInHierarchy)
        {
            GameObject spawnedItem = Instantiate(itemSpawnedPrefab, this.transform.position, Quaternion.identity);
            //Animate to go to the counter 
            spawnedItem.GetComponent<Rigidbody2D>().transform.DOMove(targetPosition.position, animationTravelTime);
        }
        
    }

    IEnumerator Cooldown()
    {
        canSpawn = false;
        yield return new WaitForSeconds(0.15f);
        canSpawn = true;
    }

}
