using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class MoodComponent : MonoBehaviour
{
    [SerializeField]
    float currentMoodAmount;
    [SerializeField]
    DisplayCustomerMood dcm;
    [SerializeField]
    float maxMoodAmount;
    [SerializeField]
    float moodDecay;
    [SerializeField]
    float timeForDecay;
    [SerializeField]
    float neutralMoodValue;
    [SerializeField]
    float angryMoodValue;
    [SerializeField]
    Sprite happySprite;
    [SerializeField]
    Sprite neutralSprite;
    [SerializeField]
    Sprite angrySprite;
    [SerializeField]
    SpriteRenderer customerSpriteRenderer;
    [SerializeField]
    public float correctBonusTime;
    public float penaltyTime;
    bool pauseDecrease = false;
    #region Getter Setters

    public float GetCurrentMoodAmount()
    {
        return currentMoodAmount;
    }

    public float GetMaxMoodAmount()
    {
        return maxMoodAmount;
    }

    public void SetMaxMoodAmount(float maxMoodValue)
    {
        maxMoodAmount = maxMoodValue;
    }
    
    public void SetCurrentMoodAmount(float currentMoodValue) 
    {
        currentMoodAmount = currentMoodValue;
        
    }

    public void IncreaseCurrentMoodAmount(float p_increase)
    {
        pauseDecrease = true;
        dcm.delayedMoodImage.gameObject.SetActive(true);
        //set color to green
        dcm.delayedMoodImage.color = new Color(0f, 255f, 0f, 1f);
        dcm.delayedMoodImage.fillAmount = (currentMoodAmount + p_increase) / maxMoodAmount;
      
        dcm.delayedMoodImage.fillAmount = ((currentMoodAmount+p_increase)/maxMoodAmount);
        StartCoroutine(MoodIncreased(p_increase));
    }

    public void DeductCurrentMoodAmount(float p_deduction)
    {
        //CAN ONLY DEDUCT IF ITS NOT BEING DEDUCTED ALREADY
        if (!pauseDecrease)
        {
            pauseDecrease = true;
            dcm.delayedMoodImage.gameObject.SetActive(true);
            //set color to red
            dcm.delayedMoodImage.color = new Color(255f, 0f, 0f, 1f);

            currentMoodAmount -= p_deduction;
            dcm.moodImage.fillAmount = currentMoodAmount / maxMoodAmount;
       //     Debug.Log("TEST: " + currentMoodAmount);
            StartCoroutine(MoodDeducted());
        }
      
    }
    #endregion
    public IEnumerator MoodDeducted()
    {
        
        yield return new WaitForSeconds(1f);
     //   Debug.Log("TEST: " + currentMoodAmount);
        //SCALE TOWARDS THE CURRENT MOOD DISPLAY
        Tween myTween = dcm.delayedMoodImage.DOFillAmount(currentMoodAmount/maxMoodAmount,0.5f);// animation sequence

        //Transparent fade out
        dcm.delayedMoodImage.DOFade(0.0f, 0.5f);
        yield return myTween.WaitForCompletion(); // Wait to finish

        dcm.delayedMoodImage.fillAmount = currentMoodAmount/maxMoodAmount;

        //reset
        //make delayedmoodimage visible again
        dcm.delayedMoodImage.color = new Color(dcm.delayedMoodImage.color.r, dcm.delayedMoodImage.color.g, dcm.delayedMoodImage.color.b, 1f);
        dcm.delayedMoodImage.gameObject.SetActive(false);
        pauseDecrease = false ;
    }

    public IEnumerator MoodIncreased(float p_increaseAmount)
    {
        
        yield return new WaitForSeconds(1f);
        
        //SCALE TOWARDS THE CURRENT MOOD DISPLAY
        Tween myTween = dcm.moodImage.DOFillAmount((currentMoodAmount+p_increaseAmount )/ maxMoodAmount, 0.5f);// animation sequence

        //Transparent fade out
        dcm.delayedMoodImage.DOFade(0.0f, 0.5f);
        yield return myTween.WaitForCompletion(); // Wait to finish
      
        currentMoodAmount += p_increaseAmount;
       
        dcm.moodImage.fillAmount = currentMoodAmount / maxMoodAmount;
        dcm.delayedMoodImage.fillAmount = currentMoodAmount / maxMoodAmount;

        //reset
        //make delayedmoodimage visible again
        dcm.delayedMoodImage.color = new Color(dcm.delayedMoodImage.color.r, dcm.delayedMoodImage.color.g, dcm.delayedMoodImage.color.b, 1f);
        dcm.delayedMoodImage.gameObject.SetActive(false);
        pauseDecrease = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        InitializeMood();

    }

    // Update is called once per frame
    void Update()
    {
        //ChangeCustomerMoodSprite();
    }

    IEnumerator CustomerMoodDecay()
    {
        while (currentMoodAmount > 0)
        {
            yield return new WaitForSeconds(timeForDecay);
            
            if (!pauseDecrease)
            {
                if(GameManager.instance.customer != null)
                {
                    if (GameManager.instance.customer.moodPanel != null)
                    {
                        if (GameManager.instance.customer.moodPanel.activeSelf)
                        {
                            if (dcm.moodImage != null)
                            {
                                currentMoodAmount -= moodDecay;
                                dcm.DisplayMood();

                                ChangeCustomerMoodSprite();
                                if (currentMoodAmount <= 0)
                                {
                                    //If order sheet is active
                                    if (TransitionManager.instances.noteBookTransform.gameObject.activeSelf)
                                    {
                                        //Hide it 
                                        TransitionManager.instances.MoveTransition(new Vector2(-523f, 1386f), 0.5f, TransitionManager.instances.noteBookTransform, TransitionManager.instances.noteBookTransform.gameObject, false);
                                        MoodComponent mc = GameManager.instance.customer.GetComponent<MoodComponent>();
                                        mc.IncreaseCurrentMoodAmount(mc.correctBonusTime * 6); //3 seconds
                                    }
                                    if (TransitionManager.instances.changeTransform.gameObject.activeSelf)
                                    {
                                        //add bonus mood time
                                        MoodComponent mc = GameManager.instance.customer.GetComponent<MoodComponent>();
                                        mc.IncreaseCurrentMoodAmount(mc.correctBonusTime * 6); //3 seconds
                                        TransitionManager.instances.MoveTransition(new Vector2(-523f, 1386f), 0.5f, TransitionManager.instances.changeTransform, TransitionManager.instances.changeTransform.gameObject, false);
                                    }

                                }



                            }
                        }
                    }
              
                }
               
                
               
            }

           

        }
        GameManager.instance.customerSpawner.StartCoroutine(GameManager.instance.customerSpawner.SpawnRate());
        //Customer despawn
        if (GameManager.instance.customer)
        {

            //Disable customer bubble
            GameManager.instance.customer.panel.gameObject.SetActive(false);

            //Disable customer mood bar
            GameManager.instance.customer.moodPanel.SetActive(false);
            //animation
            DOTween.Sequence().Append(GameManager.instance.customer.gameObject.transform.DOMove(GameManager.instance.customerSpawner.outShopPoint.position, 1f, false));
            Destroy(GameManager.instance.customer.gameObject, 1.5f);
            GameManager.instance.customer = null;
        }


    }
    void InitializeMood()
    {
        currentMoodAmount = maxMoodAmount;
        dcm.delayedMoodImage.gameObject.SetActive(false);
        customerSpriteRenderer = this.GetComponent<SpriteRenderer>();
        if (customerSpriteRenderer)
        {
            if (happySprite)
            {
                customerSpriteRenderer.sprite = happySprite;
            }
        }
        StartCoroutine(CustomerMoodDecay());
    }

    void ChangeCustomerMoodSprite()
    {
        if(currentMoodAmount >= maxMoodAmount)
        {
           // Debug.Log("Happy");
            if (happySprite)
            {
                if (customerSpriteRenderer.sprite != happySprite) 
                {

                    customerSpriteRenderer.sprite = happySprite;
                
                }
            }
        }
        if (currentMoodAmount <= neutralMoodValue)
        {
           // Debug.Log("Neutral");
            if (neutralSprite)
            {
                if (customerSpriteRenderer.sprite != neutralSprite)
                {
                    customerSpriteRenderer.sprite = neutralSprite;
                }
                
            }
        }

        if(currentMoodAmount <= angryMoodValue)
        {
          //  Debug.Log("Angry");
            if (angrySprite)
            {
                if (customerSpriteRenderer.sprite != angrySprite)
                {
                    customerSpriteRenderer.sprite = angrySprite;
                }

            }
        }
    }
}
