using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeCalculator : MonoBehaviour
{
    // Start is called before the first frame update
    public Text numeratorText;
    public Text denominatorText;
    public BillCounter billCounter;
    public InputField changeInputField;
    public Item cash;
    private void OnEnable()
    {
        GameManager.instance.orderSheetShowing = true;
        cash = GameManager.instance.customer.itemsWanted[0];
        numeratorText.text = cash.numValue.ToString();
        denominatorText.text = cash.denValue.ToString();
    }

    private void OnDisable()
    {
        billCounter.isChangeUIActive = false;
        changeInputField.text = "";
        changeInputField.Select();
      
    }

    public void OnPriceInputted()
    {

        string playerInputString = changeInputField.text;

        float playerInputValue = -1;

        if (float.TryParse(playerInputString, out float inputVal)) // convert string to float
        {

            playerInputValue = inputVal;
        }
        if (playerInputValue != -1)
        {
            if (playerInputValue == cash.price)
            {
                //Answer is correct
                ChangeOrderFinish();
                Scoring.instance.addScore(100);
                Debug.Log("Is Correct");
            }
            else
            {
                Debug.Log("Isincorrect");
                changeInputField.text = "";
                changeInputField.Select();
            }
        }
        }

    public void ChangeOrderFinish()
    {
        TransitionManager.instances.MoveTransition(new Vector2(523f, 1386f), 1f, TransitionManager.instances.changeTransform, gameObject.transform.parent.gameObject, false);
        if (GameManager.instance.customer)
        {
            Destroy(GameManager.instance.customer.gameObject);
        }
        GameManager.instance.customerSpawner.SpawnCustomer();
    }
}
