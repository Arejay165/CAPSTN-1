using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerScript : MonoBehaviour
{
    //public CustomerWants item;

    //public DragDrop item;
    public GameObject   itemWant;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DragDrop>().gameObject.name == "Asset-SodaBottle")
        {
            Scoring.instance.addScore(100);
            itemWant.SetActive(false);
            Destroy(collision.gameObject);
        }
    }
}
