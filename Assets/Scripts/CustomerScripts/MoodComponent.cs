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


    Color currentColorMood = Color.green;
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
        if (!pauseDecrease)
        {
            pauseDecrease = true;
            dcm.delayedMoodImage.gameObject.SetActive(true);
            //set color to green
            //dcm.delayedMoodImage.color = new Color(0f, 255f, 0f, 1f);
            dcm.delayedMoodImage.color = Color.white;
            float desiredAmount = ((currentMoodAmount + p_increase) / maxMoodAmount);
            if (desiredAmount > 1 )
            {
                desiredAmount = 1;
            }

            dcm.delayedMoodImage.fillAmount = desiredAmount;
            StartCoroutine(MoodIncreased(p_increase));
        }
        
    }

    public void DeductCurrentMoodAmount(float p_deduction)
    {
        //CAN ONLY DEDUCT IF ITS NOT BEING DEDUCTED ALREADY
        if (!pauseDecrease)
        {
            float desiredAmount = currentMoodAmount - p_deduction;
            if (desiredAmount < 0)
            {
                desiredAmount = 0;
            }
            else
            {
                pauseDecrease = true;
            }
            
            dcm.delayedMoodImage.fillAmount = currentMoodAmount / maxMoodAmount;
            dcm.delayedMoodImage.gameObject.SetActive(true);
            //set color to red
            //dcm.delayedMoodImage.color = new Color(255f, 0f, 0f, 1f);
            dcm.delayedMoodImage.color = Color.white;
           
            
            dcm.moodImage.fillAmount = desiredAmount / maxMoodAmount;
       //     Debug.Log("TEST: " + currentMoodAmount);
            StartCoroutine(MoodDeducted(p_deduction));
        }
      
    }
    #endregion
    public IEnumerator MoodDeducted(float p_deduction)
    {
        int blink = 3;
        for (int i =0; i < blink; i++)
        {
            dcm.gameObject.SetActive(false);
            yield return new WaitForSeconds((0.5f / (float)blink) * 0.25f);
            dcm.gameObject.SetActive(true);
            yield return new WaitForSeconds((0.5f / (float)blink) * 0.75f);
        }
     
       // yield return new WaitForSeconds(1f);
     //   Debug.Log("TEST: " + currentMoodAmount);
        //SCALE TOWARDS THE CURRENT MOOD DISPLAY
        Tween myTween = dcm.delayedMoodImage.DOFillAmount((currentMoodAmount - p_deduction) / maxMoodAmount,0.5f);// animation sequence
        currentMoodAmount -= p_deduction;

        //Transparent fade out
        dcm.delayedMoodImage.DOFade(0.0f, 0.5f);
        yield return myTween.WaitForCompletion(); // Wait to finish

        dcm.delayedMoodImage.fillAmount = currentMoodAmount / maxMoodAmount;
        dcm.moodImage.fillAmount = currentMoodAmount / maxMoodAmount;

        //reset
        //make delayedmoodimage visible again
        //color
        //dcm.delayedMoodImage.color = new Color(dcm.delayedMoodImage.color.r, dcm.delayedMoodImage.color.g, dcm.delayedMoodImage.color.b, 1f);
        dcm.delayedMoodImage.color = Color.white;
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
        //dcm.delayedMoodImage.color = new Color(dcm.delayedMoodImage.color.r, dcm.delayedMoodImage.color.g, dcm.delayedMoodImage.color.b, 1f);
        dcm.delayedMoodImage.color = Color.white;
        dcm.delayedMoodImage.gameObject.SetActive(false);
        pauseDecrease = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        InitializeMood();
        dcm.moodImage.color = Color.green;
       
       
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
            StartCoroutine(GameManager.instance.customer.ThoughtBubbleDisappear());

            //Disable customer mood bar
            GameManager.instance.customer.moodPanel.SetActive(false);
            //animation
            DOTween.Sequence().Append(GameManager.instance.customer.gameObject.transform.DOMove(GameManager.instance.customerSpawner.outShopPoint.position, 1f, false));
            Destroy(GameManager.instance.customer.gameObject, 1.5f);
            GameManager.instance.customer = null;

            AudioManager.instance.playSound(11);
        }


    }
    void InitializeMood()
    {
        currentMoodAmount = maxMoodAmount;
        dcm.delayedMoodImage.gameObject.SetActive(false);
        customerSpriteRenderer = this.GetComponent<SpriteRenderer>();
        dcm.gameObject.transform.parent.gameObject.GetComponent<Canvas>().worldCamera = PlayerManager.instance.UICamera;
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
                    currentColorMood = Color.green;
                    dcm.moodImage.color = currentColorMood;
                
                }
            }
        }
        if (currentMoodAmount <= neutralMoodValue && currentMoodAmount >= angryMoodValue)
        {
            // Debug.Log("Neutral");
            if (neutralSprite)
            {
                if (customerSpriteRenderer.sprite != neutralSprite)
                {
                    customerSpriteRenderer.sprite = neutralSprite;
                    currentColorMood = Color.yellow;
                    dcm.moodImage.color = currentColorMood;

                    if (customerSpriteRenderer.sprite == neutralSprite)
                    {
                        AudioManager.instance.playSound(14);
                        Debug.Log("Play Audio Neutral");
                    }
                        
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
                    currentColorMood = Color.red;
                    dcm.moodImage.color = currentColorMood;

                    if (customerSpriteRenderer.sprite == angrySprite)
                    {
                        AudioManager.instance.playSound(15);
                        Debug.Log("Play Audio Angry");
                    }
                       
                }

            }
        }

        
    }

   
}
