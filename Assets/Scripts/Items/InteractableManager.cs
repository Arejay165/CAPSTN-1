using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static InteractableManager instances;
    public List<ClickToSelectItem> clickToSelectItems;
    public BillCounter cashBox;
    void Start()
    {
        if(instances == null)
        {
            instances = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnController(bool state)
    {
        foreach(ClickToSelectItem items in clickToSelectItems)
        {
            items.canSpawn = state;
        }
    }

    
}
