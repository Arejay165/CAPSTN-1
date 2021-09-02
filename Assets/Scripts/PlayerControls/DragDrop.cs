using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour
{
    //Code Reference: Clipper: https://www.youtube.com/watch?v=SJMayMl4lj8
    public bool     canBeDragged = true;
    public bool     isBeingDragged = false;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerManager.instance.currentSelectedItem != null)
        {
            Destroy(PlayerManager.instance.currentSelectedItem);
            PlayerManager.instance.lastItemSpawner.canSpawn = false;
            
        }

        PlayerManager.instance.currentSelectedItem = gameObject;


    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.instance.isHolding && Input.GetMouseButton(0))
        {
            this.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
            if (Input.GetMouseButtonUp(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

            if(PlayerManager.instance.isStaying == true)
            {
                Debug.Log("Item Placed in Counter: " + gameObject.name);
                GameObject item = gameObject;
                Customer customer = GameManager.instance.customer;
                for (int i = 0; i < GameManager.instance.customer.itemsWanted.Count; i++)
                {
                    if (item.GetComponent<ItemDescription>().item.itemName == customer.itemsWanted[i].itemName)
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
                      

                        break;
                    }
                    else
                    {
                        Debug.Log("Wrong Item");
                    }

                }
            }
                Debug.Log("delete");
            
                Destroy(PlayerManager.instance.currentSelectedItem);
                PlayerManager.instance.currentSelectedItem = null;
                PlayerManager.instance.isHolding = false;
                if (GameManager.instance.orderSheetShowing)
                {
                    PlayerManager.instance.lastItemSpawner.canSpawn = false;
                }
                else
                {
                    PlayerManager.instance.lastItemSpawner.canSpawn = true;
                }
       
                PlayerManager.instance.lastItemSpawner = null;
        }
    }


    private void OnMouseOver()
    {

        

    }
    private void OnMouseUp()
    {
        Debug.Log("mouse up");
        Destroy(PlayerManager.instance.currentSelectedItem);
        PlayerManager.instance.currentSelectedItem = null;
        PlayerManager.instance.isHolding = false;
        PlayerManager.instance.lastItemSpawner.canSpawn = true;
        PlayerManager.instance.lastItemSpawner = null;

    }

}
