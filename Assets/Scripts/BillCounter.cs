using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BillCounter : MonoBehaviour, IPointerDownHandler
{

    public bool clickable = true;
    [SerializeField]
    SpriteRenderer itemSR;
    [SerializeField]
    Sprite defaultItemSprite;
    [SerializeField] ParticleSystem hintingSparklePFX;
    [SerializeField] ParticleSystem radiatePFX;
    [SerializeField]
    Sprite highlightedSprite;
    private void Start()
    {
        
    }
    private void OnEnable()
    {

        //Register OnGameStart Event in GameManager
    
        GameManager.OnGameEnd += OnGameEnded;
    }

    private void OnDisable()
    {
        //Deregister OnGameStart Event in GameManager

        GameManager.OnGameEnd -= OnGameEnded;





    }

    void OnGameEnded()
    {
        RemoveHighlightItem();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (clickable)
        {
            if (!TransitionManager.instances.noteBookTransform.gameObject.activeSelf && !TransitionManager.instances.changeTransform.gameObject.activeSelf)
            {
                if (MathProblemManager.instance.GetGeneratedItemsWanted()[0].numValue != 0)//GameManager.instance.customer.willBuy)
                {
                    if (GameManager.instance.customer != null)
                    {
                        GameManager.instance.customer.itemsImage[0].color = global::GameManager.instance.window.darkenImage;
                        GameManager.instance.customer.itemsImage.RemoveAt(0);
                        MathProblemManager.instance.GetCurrentItemsWanted().RemoveAt(0);
                        MoodComponent mc = GameManager.instance.customer.GetComponent<MoodComponent>();
                        mc.IncreaseCurrentMoodAmount(mc.correctBonusTime * 2);// 1 second

                        ChangeUI();
                        StartCoroutine(Cooldown());
                    }

                }

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


                else if (MathProblemManager.instance.GetGeneratedItemsWanted()[0].numValue == 0)
                {
                    if (GameManager.instance.customer != null)
                    {
                        Debug.Log("I PRESSED BIL LBOX");
                        MoodComponent mc = GameManager.instance.customer.GetComponent<MoodComponent>();
                        mc.DeductCurrentMoodAmount(mc.penaltyTime);
                        Scoring.instance.ResetMultiplier();
                        AudioManager.instance.playSound(3);
                        PlayerManager.instance.CamShake(PlayerManager.instance.GameCamera.gameObject, 0.2f, 0.05f, 15f);
                        StartCoroutine(Cooldown());
                    }
                }
            }
        }
        
       
    }
    private void OnMouseOver()
    {


        //If the item is clicked
        //Spawn one at a time
        if (GameManager.instance.sheetOpen == false)
        {
            if (GameManager.instance.isPlaying)
            {
                HighlightItem();
                if (!CursorManager.instance.isLooping)
                {

                    CursorManager.instance.PlayCursorAnimation(CursorType.HoverItem);
                }
            }
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
        if (hintingSparklePFX && !hintingSparklePFX.isPlaying)
        {
            hintingSparklePFX.Play();
      
        }
        if (radiatePFX && !radiatePFX.isPlaying)
        {
            radiatePFX.Play();
      
        }

    }

    void RemoveHighlightItem()
    {

        if (defaultItemSprite != null && itemSR != null) itemSR.sprite = defaultItemSprite;

        if (hintingSparklePFX) hintingSparklePFX.Stop();
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
    void ChangeUI()
    {
        //Disable customer bubble
        StartCoroutine(GameManager.instance.customer.ThoughtBubbleDisappear());

        //show pabarya sheet
        TransitionManager.instances.MoveTransition(new Vector2(680f, 0f), 0.5f, TransitionManager.instances.changeTransform, TransitionManager.instances.changeTransform.gameObject, true);
   
    }

    IEnumerator Cooldown()
    {
        clickable = false;
        yield return new WaitForSeconds(0.15f);
        clickable = true;
    }
}
