using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemList : MonoBehaviour
{
    public Item item;
    public Image itemSprite_List;
    public TextMeshProUGUI itemName_List;
    public TextMeshProUGUI itemPrice_List;

    // Start is called before the first frame update
    void Start()
    {
        itemSprite_List.sprite = item.itemSprite;
        itemSprite_List.preserveAspect = true; 
        itemName_List.GetComponent<TextMeshProUGUI>().text = item.itemName;
        itemPrice_List.GetComponent<TextMeshProUGUI>().text = "P" + item.price.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
