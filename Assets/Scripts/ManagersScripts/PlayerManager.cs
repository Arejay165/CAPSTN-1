using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public bool isHolding;
    
    public GameObject currentSelectedItem;
    public ClickToSelectItem lastItemSpawner;
    public bool isStaying = false;
    public string playerName;

    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void NameEntered(string p_name)
    {
        playerName = p_name;
        GameManager.instance.StartCoroutine(GameManager.instance.DayStart());
    }
 
    public void Shake(GameObject p_object,float p_length, float p_power, float p_rotationMultiplier)
    {
        

        StartCoroutine(Shaking(p_object, p_object.transform.position,p_object.transform.rotation, p_power, p_power * p_rotationMultiplier, p_rotationMultiplier, p_power / p_length, p_length,0.01f));

    }
    IEnumerator Shaking(GameObject p_object, Vector3 p_defaultPos, Quaternion p_defaultRotation,float p_shakePower, float p_shakeRotation, float p_rotationMultiplier, float p_shakeFadeTime, float p_length, float p_duration)
    {

        bool isShaking = true;
        float shakeTime = 0;
        while (isShaking)
        {
            if (shakeTime < p_length)
            {
                float yAmount = UnityEngine.Random.Range(-1, 1) * p_shakePower;
                float xAmount = UnityEngine.Random.Range(-1, 1) * p_shakePower;
                p_shakeRotation = Mathf.MoveTowards(p_shakeRotation, 0f, p_shakeFadeTime * p_rotationMultiplier * Time.deltaTime);
                p_object.transform.position += new Vector3(xAmount, yAmount, 0f);
                p_object.transform.rotation = Quaternion.Euler(0f, 0f, p_shakeRotation * UnityEngine.Random.Range(-1, 1));
                p_shakePower = Mathf.MoveTowards(p_shakePower, 0f, p_shakeFadeTime * Time.deltaTime);
                yield return new WaitForSeconds(p_duration);
                shakeTime+= p_duration;
                
                //StartCoroutine(Shaking(p_shakePower, p_shakeRotation, p_rotationMultiplier, p_shakeFadeTime, p_duration));
            }
            else
            {
                
                p_object.transform.position = p_defaultPos;
                p_object.transform.rotation = p_defaultRotation;
                isShaking = false;
            }

        }



    }
   
   


    // Start is called before the first frame update
    void Start()
    {
        isHolding = false;

    }

    // Update is called once per frame
    void Update()
    {
        //TUTORIAL
        //if (!TutorialManager.instance.tutorialQuestActive)
        //{
        //    if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
        //    {
        //        TutorialManager.instance.StartTimeline();
                
        //    }
        //}

    }


}
