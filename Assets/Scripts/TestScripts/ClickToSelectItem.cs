using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToSelectItem : MonoBehaviour
{
    [SerializeField]
    GameObject      itemSpawnedPrefab;
    public bool     canSpawn;
    [SerializeField]
    Sprite          highlightedSprite;
    [SerializeField]
    Sprite          defaultItemSprite;
    //Testing for click to go to counter

    //Set the location where the item will go
    [SerializeField] 
    Transform       targetPosition;

    private void Start()
    {
        canSpawn = true;
    }
    private void OnMouseOver()
    {
        //if (!PlayerManager.instance.isHolding && canSpawn && Input.GetMouseButtonDown(0)) // If the object is draggable and Left mouse button is down
        //{
        //    Debug.Log("SPAWNED");
        //    canSpawn = false;
        //    PlayerManager.instance.isHolding = true;
        //    Vector3 spawnPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, transform.position.z);
        //    GameObject newItemInstance = Instantiate(itemSpawnedPrefab, spawnPosition, Quaternion.identity);


        //}

        //Sprite change to highlighted 
        //Since the Assets are a child of a gameObject, get the the child of the parent to access the assets (Children)
        if(this.transform.childCount > 0)
        {
            //Get all the child assets 
            for(int i = 0; i < this.transform.childCount; i++)
            {
                //Access the item 
                SpriteRenderer itemChild = this.transform.GetChild(i).GetComponent<SpriteRenderer>();
                //Change the current sprite to the highlighted sprite 
                itemChild.sprite = highlightedSprite;
            }
        }
        // in the case that there is no child, access the parent 
        else
        {
            this.transform.gameObject.GetComponent<SpriteRenderer>().sprite = highlightedSprite;
        }
        
        //If the item is clicked
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clicked");
            GameObject spawnedItem = Instantiate(itemSpawnedPrefab, targetPosition.position, Quaternion.identity);
        }

    }
    private void OnMouseExit()
    {
        //When mouse exited the item bounds, return the current sprite to the defaultItemSprite
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject itemChild = this.transform.GetChild(i).gameObject;
            itemChild.GetComponent<SpriteRenderer>().sprite = defaultItemSprite;

        }
    }

}
