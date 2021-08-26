using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DisplayCustomerOrder : MonoBehaviour
{
    Customer            customer;
    [SerializeField]
    GameObject          itemDisplay;
    [SerializeField]
    Transform           displayPanel;
    [SerializeField]
    TextMeshProUGUI     priceText;



    private void Start()
    {
        displayPanel = this.transform;
    }
    public void DisplayOrderList()
    {
        customer = GameManager.instance.customer;
       
        Debug.Log("Display Order");
        for(int i = 0; i < customer.GetMaxInventory(); i++)
        { 
            
            GameObject order = Instantiate(itemDisplay);
            order.transform.SetParent(displayPanel);
            order.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = customer.itemInCart[i].itemSprite;
            order.transform.GetChild(0).gameObject.GetComponent<Image>().preserveAspect = true;
            order.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "P" + customer.itemInCart[i].price.ToString();
            order.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = customer.itemInCart[i].price.ToString()  +"x"; 
                
            
            
 

        }
    }

}
