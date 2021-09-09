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

    public void SetCustomer(Customer currentCustomer)
    {
        customer = currentCustomer;
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
            PlayerManager.instance.isStaying = true;
            
            if (customer)
            {
                for(int i = 0; i < customer.itemsWanted.Count; i++)
                {
                    if(itemInCounter.item.itemName == customer.itemsWanted[i].itemName)
                    {
                        
                        Scoring.instance.addScore(100);
                        Scoring.instance.starCheck();
                        customer.itemInCart.Add(customer.itemsWanted[i]);
                        customer.itemsWanted.RemoveAt(i);
                        customer.itemSprites[i].color = GameManager.instance.window.darkenImage;
                        customer.itemSprites.RemoveAt(i);
                        if (customer.itemSprites.Count <= 0)
                        {
                            TransitionManager.instances.MoveTransition(new Vector2(507.0f, 0), 1f, TransitionManager.instances.noteBookTransform, GameManager.instance.testCalculator.transform.root.gameObject, true);
                        }
                        PlayerManager.instance.isStaying = false;
                        Destroy(itemInCounter.gameObject, 0.2f);
                        break;
                    }
                    else
                    {
                        Debug.Log("Wrong Item");
                        PlayerManager.instance.isStaying = true;
                        
                    }
                }
            }
            
        }
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if(collision.gameObject.tag == "Item")
    //    {
    //        PlayerManager.instance.isStaying = true;
    //        Debug.Log("Item");

    //        if (Input.GetMouseButtonUp(0))
    //        {

    //        }


    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    PlayerManager.instance.isStaying = false;
    //}


}
