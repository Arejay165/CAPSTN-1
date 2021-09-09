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
        HighlightItem();
        
        //If the item is clicked
        //Spawn one at a time
        if (Input.GetMouseButtonDown(0) && canSpawn)
        {
            SpawnItem();
            canSpawn = false;
        }
        if (Input.GetMouseButtonUp(0) && !canSpawn)
        {
            canSpawn = true;
        }

    }
    private void OnMouseExit()
    {
        RemoveHighlightItem();
    }

    void HighlightItem()
    {
        //Sprite change to highlighted 
        //Since the Assets are a child of a gameObject, get the the child of the parent to access the assets (Children)
        if (this.transform.childCount > 0)
        {
            //Get all the child assets 
            for (int i = 0; i < this.transform.childCount; i++)
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
    }

    void RemoveHighlightItem()
    {
        if(this.transform.childCount > 0)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                GameObject itemChild = this.transform.GetChild(i).gameObject;
                itemChild.GetComponent<SpriteRenderer>().sprite = defaultItemSprite;
            }
        }
        else
        {
            this.transform.gameObject.GetComponent<SpriteRenderer>().sprite = defaultItemSprite;
        }
       
    }

    void SpawnItem()
    {
        GameObject spawnedItem = Instantiate(itemSpawnedPrefab, targetPosition.position, Quaternion.identity);
    }
}
