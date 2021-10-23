using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ArrowAnimation : MonoBehaviour
{
    public RectTransform rectTransform;
    public bool moveUp;
    public float endPosition;

    private void OnEnable()
    {
        rectTransform = gameObject.transform.GetComponent<RectTransform>();
        // rectTransform.DOAnchorPosY(rectTransform.localPosition.y + 20, 1, false).SetLoops(4, LoopType.Yoyo).SetSpeedBased();
        if(moveUp)
        rectTransform.DOAnchorPosY(endPosition, 1, true).SetLoops(-1, LoopType.Yoyo);
        else
            rectTransform.DOAnchorPosX(endPosition, 1, true).SetLoops(-1, LoopType.Yoyo);
    }
    private void OnDisable()
    {
        gameObject.SetActive(false);
    }
}
