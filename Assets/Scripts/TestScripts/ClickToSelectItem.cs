using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToSelectItem : MonoBehaviour
{
    [SerializeField]
    GameObject   itemSpawnedPrefab;
    public bool         canSpawn;
    [SerializeField]
    Sprite       highlightedSprite;
    [SerializeField]
    Sprite       itemSprite;
    //Testing for click to go to counter

    //Set the location where the item will go
    [SerializeField] Transform targetLocation;

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
        if(this.transform.childCount > 0)
        {
            for(int i = 0; i < this.transform.childCount; i++)
            {
                SpriteRenderer itemChild = this.transform.GetChild(i).GetComponent<SpriteRenderer>();
                Vector2 defaultSize = itemChild.size;
                itemChild.sprite = highlightedSprite;
                itemChild.size = defaultSize;
            }
        }
        else
        {
            this.transform.gameObject.GetComponent<SpriteRenderer>().sprite = highlightedSprite;
        }
        
      
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clicked");
            GameObject spawnedItem = Instantiate(itemSpawnedPrefab, targetLocation.position, Quaternion.identity);
        }

    }
    private void OnMouseExit()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject itemChild = this.transform.GetChild(i).gameObject;
            itemChild.GetComponent<SpriteRenderer>().sprite = itemSprite;

        }
    }

}
