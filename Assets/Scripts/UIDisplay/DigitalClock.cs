using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class DigitalClock : MonoBehaviour
{
    
    public TextMeshProUGUI timeValueUI;
    public bool isCountdowning = false;
    [SerializeField] Color defaultColor;
    float defaultSize;


    // Start is called before the first frame update
    void Start()
    {
         defaultColor = timeValueUI.color;
        
    }
    private void OnEnable()
    {
        timeValueUI.color = defaultColor;
        timeValueUI.alpha = 255f;

        defaultSize = timeValueUI.fontSize;
        isCountdowning = false;
        StopAllCoroutines();
    }

    private void OnDisable()
    {
        timeValueUI.fontSize = defaultSize;
        timeValueUI.color = defaultColor;
    }
    // Update is called once per frame
    void Update()
    {
        if (DayAndNightCycle.instance)
        {
        
            
            
            if (!isCountdowning)
            {
                isCountdowning = true;
                StartCoroutine(Countdown());
                
                
                
            }
            
        }
    }

    IEnumerator Countdown()
    {
        float descendingTime = DayAndNightCycle.instance.GetEndTime() - DayAndNightCycle.instance.GetGameTime();
       
        Vector3 OriginalScale = timeValueUI.transform.localScale;
        Vector3 modifiedScale;
        float sizeTweenSpeed;
        float colorTweenSpeed;
        if (descendingTime <= 10)
        {
            modifiedScale = new Vector3(OriginalScale.x + 0.5f, OriginalScale.y + 0.5f, OriginalScale.z + 0.5f);
            sizeTweenSpeed = 0.2f;
            colorTweenSpeed = 2f;
            timeValueUI.DOColor(defaultColor, colorTweenSpeed).From(Color.red);

        }
        else
        {
            modifiedScale = new Vector3(OriginalScale.x + 0.25f, OriginalScale.y + 0.25f, OriginalScale.z + 0.25f);
            sizeTweenSpeed = 0.25f;
            
        }
        timeValueUI.GetComponent<RectTransform>().DORewind();
        DOTween.Sequence().Append(timeValueUI.transform.DOScale(modifiedScale, sizeTweenSpeed).SetEase(Ease.Linear)).Append(timeValueUI.transform.DOScale(OriginalScale, 0.2f).SetEase(Ease.Linear));
        timeValueUI.text = descendingTime.ToString("N0") + "s";

        yield return new WaitForSeconds(1f);

        isCountdowning = false;
    

    }
}
