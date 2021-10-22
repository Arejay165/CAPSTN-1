using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                GameManager.instance.customerSpawner.StartCoroutine(GameManager.instance.customerSpawner.SpawnRate());
                Destroy(this.gameObject);
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
