using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemSpawner : MonoBehaviour
{
    public GameObject itemSpawnedPrefab;
    public bool canSpawn;

    private void Start()
    {
        canSpawn = true;
    }
    private void OnMouseOver()
    {
        if (!PlayerManager.instance.isHolding && canSpawn && Input.GetMouseButtonDown(0)) // If the object is draggable and Left mouse button is down
        {
            Debug.Log("SPAWNED");
            canSpawn = false;
            PlayerManager.instance.isHolding = true;
            Vector3 spawnPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, transform.position.z);
            GameObject newItemInstance = Instantiate(itemSpawnedPrefab, spawnPosition, Quaternion.identity);

            PlayerManager.instance.lastItemSpawner = this;
        }


    }


}
