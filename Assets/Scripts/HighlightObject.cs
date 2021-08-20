using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightObject : MonoBehaviour
{
    public Sprite unhighlightObj;
    public Sprite highlightObj;

    // Start is called before the first frame update
    void Start()
    {
       unhighlightObj = this.gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    private void OnMouseOver()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = highlightObj;
    }

    void OnMouseExit()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = unhighlightObj;
    }
}
