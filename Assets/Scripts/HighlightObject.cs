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
        if (highlightObj)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = highlightObj;
        }
        
        // Destroy when not accepted by customer 
        if (PlayerManager.instance.isStaying && Input.GetMouseButton(0))
        {
            Destroy(gameObject, 0.2f);
        }
    }

    void OnMouseExit()
    {
        if (unhighlightObj)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = unhighlightObj;
        }
       
    }
}
