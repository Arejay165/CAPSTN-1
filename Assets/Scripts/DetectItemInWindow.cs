using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DetectItemInWindow : MonoBehaviour
{

    [SerializeField] private Scoring score;
    [SerializeField] Customer customer;
    [SerializeField] public Color darkenImage;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.window = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Item")
        {
            PlayerManager.instance.isStaying = true;
            Debug.Log("Item");

            if (Input.GetMouseButtonUp(0))
            {
                
            }
            
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerManager.instance.isStaying = false;
    }


}
