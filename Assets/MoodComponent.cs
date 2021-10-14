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
        
    }

    IEnumerator CustomerMoodDecay()
    {
        while (currentMoodAmount > 0)
        {
            yield return new WaitForSeconds(timeForDecay);
            currentMoodAmount -= moodDecay;
        }
        
    }
    void InitializeMood()
    {
        currentMoodAmount = maxMoodAmount;
        StartCoroutine(CustomerMoodDecay());
    }
}
