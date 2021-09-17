using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BillCounter : MonoBehaviour, IPointerDownHandler
{
  
  public  bool isChangeUIActive;
    

    private void Start()
    {
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("SKDALISMDALKDMA");
        if (!isChangeUIActive && !GameManager.instance.customer.willBuy)
        {
            isChangeUIActive = true;
            ChangeUI();
            Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
        }
    }

    void ChangeUI()
    {
        TransitionManager.instances.MoveTransition(new Vector2(523f, 0), 1f, TransitionManager.instances.changeTransform, GameManager.instance.testCalculator.transform.root.gameObject, true);

    }
}
