using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public RectTransform noteBookTransform;
    public RectTransform changeTransform;
    public static TransitionManager instances;
    void Start()
    {
        if(instances == null)
        {
            instances = this;
        }
        else
        {
            return;
        }
    }

    public void MoveTransition(Vector2 position, float time, RectTransform rectTransform, GameObject obj, bool state)
    {
        StartCoroutine(MoveAnimation(position, time, rectTransform, obj, state));
    }

    IEnumerator MoveAnimation(Vector2 position, float time, RectTransform rectTransform, GameObject obj, bool state)
    {
        obj.SetActive(true); // order sheet or pabarya
        Tween myTween = rectTransform.DOAnchorPos(position, time); // animation sequence
        yield return myTween.WaitForCompletion(); // Wait to finish
        obj.SetActive(state); // State 

    }
    
    public void Initialized()
    {

    }
}
