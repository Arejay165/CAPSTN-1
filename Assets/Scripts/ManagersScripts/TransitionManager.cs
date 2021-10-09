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
        obj.SetActive(true);
        Tween myTween = rectTransform.DOAnchorPos(position, time);
        yield return myTween.WaitForCompletion();
        obj.SetActive(state);

    }
    
    public void Initialized()
    {

    }
}
