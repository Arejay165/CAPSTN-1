using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MoodComponent : MonoBehaviour
{
    [SerializeField]
    float currentMoodAmount;
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

    #endregion

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
            currentMoodAmount -= moodDecay;
            ChangeCustomerMoodSprite();
            if (currentMoodAmount <= 0) {
                //If order sheet is active
                if (TransitionManager.instances.noteBookTransform.gameObject.activeSelf)
                {
                    //Hide it 
                    TransitionManager.instances.MoveTransition(new Vector2(-523f, 1386f), 0.5f, TransitionManager.instances.noteBookTransform, TransitionManager.instances.noteBookTransform.gameObject, false);
                }
                if (TransitionManager.instances.changeTransform.gameObject.activeSelf)
                {
                    TransitionManager.instances.MoveTransition(new Vector2(-523f, 1386f), 0.5f, TransitionManager.instances.changeTransform, TransitionManager.instances.changeTransform.gameObject, false);
                }
                GameManager.instance.customerSpawner.StartCoroutine(GameManager.instance.customerSpawner.SpawnRate());
                //Customer despawn
                if (GameManager.instance.customer)
                {
                    //animation
                    DOTween.Sequence().Append(GameManager.instance.customer.gameObject.transform.DOMove(GameManager.instance.customerSpawner.outShopPoint.position, 1f, false));
                    Destroy(GameManager.instance.customer.gameObject, 1.5f);
                    GameManager.instance.customer = null;
                }
            }

        }
        

        
    }
    void InitializeMood()
    {
        currentMoodAmount = maxMoodAmount;
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
            Debug.Log("Happy");
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
            Debug.Log("Neutral");
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
            Debug.Log("Angry");
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
