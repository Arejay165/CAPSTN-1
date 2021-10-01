using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DetectItemInWindow : MonoBehaviour
{

    [SerializeField] private Scoring score;
    [SerializeField] public Color darkenImage;
    // Start is called before the first frame update
    void Start()
    {
        global::GameManager.instance.window = this;
        
    }
    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.gameObject.tag == "Item")
        {
            collision.gameObject.transform.SetParent(this.gameObject.transform);
            ItemDescription itemInCounter = collision.GetComponent<ItemDescription>();
   
            if (GameManager.instance.customer)
            {
                for(int i = 0; i < MathProblemManager.instance.GetCurrentItemsWanted().Count; i++)
                {
                    if(itemInCounter.item.itemName == MathProblemManager.instance.GetItemInCurrentItemsWanted(i).itemName)
                    {
                        
                            GameManager.instance.customer.itemsImage[i].color = global::GameManager.instance.window.darkenImage;
                            GameManager.instance.customer.itemsImage.RemoveAt(i);
                            MathProblemManager.instance.GetCurrentItemsWanted().RemoveAt(i);
                  
                            if (MathProblemManager.instance.GetCurrentItemsWanted(false).Count <= 0)
                            {

                                TransitionManager.instances.MoveTransition(new Vector2(507.0f, 0), 1f, TransitionManager.instances.noteBookTransform, TransitionManager.instances.noteBookTransform.gameObject, true);
                            }
                            
                            Destroy(itemInCounter.gameObject, 0.2f);
                            Debug.Log("DESTROYING THRU DETECT CORRECT");
                            PlayerManager.instance.lastItemSpawner = null;
                            break;
                        
                       
                    }
                    else
                    {
                        Debug.Log("Wrong Item");
                        
                    

                    }
                }
            }
            
        }
    }
}
