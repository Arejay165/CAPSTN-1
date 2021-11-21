using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

public class Customer : MonoBehaviour
{
    //public List<Item> items;
    //public Item cash;

    //public List<Item> itemsWanted = new List<Item>();
    public List<Image> itemsImage = new List<Image>();
   // public GameObject itemPrefab;
    public Transform panel;
    public GameObject moodPanel;
    public TestCalculator displayOrder;
    public bool saved = false;
    public bool disappearing = false;
    public Vector2 savedSize;
    public bool canAcceptItem;
    //public bool willBuy;

    //private int maxInventory;
    //public float randomExtraMoney;

    //private int RNG;

    //int placeHolder = 0;
    //public Sprite exchangeSprite;

    //public int GetMaxInventory()
    //{
    //    return maxInventory;
    //}

    void OnEnable()
    {

        //Instatiating Customer Order 

        //float identifier = Random.Range(0, 100);

        //Debug.Log(identifier);
        //if (identifier > 1)
        //{
        //    willBuy = true;
        //}
        //else
        //{
        //    willBuy = false;
        //}


        //if (willBuy)
        //{
        //    maxInventory = Random.Range(1, 4);
        //    //Instatiating Customer Order 
        //    // For display 
        //    for (int i = 0; i < maxInventory; i++)
        //    {
        //        GameObject obj = Instantiate(itemPrefab);
        //        itemSprites.Add(obj.GetComponent<Image>());
        //        obj.transform.SetParent(panel);
        //        obj.GetComponent<Image>().preserveAspect = true;
        //    }
        //    RNG_item();
        //}
        //else
        //{
        //    //Not buying 

        //    //Test for whole number answer for Division 
        //    //dividend / divisor = quotient (answer of player)
        //    // Reverse the division to multiplication
        //    // divisor * quotient = dividend
        //    int divisor = Random.Range(2, 51); // use to multiply to the quotient to always be whole number 
        //    int quotient = Random.Range(1, 50); // possible answers 
        //    int dividend = divisor * quotient; // determine the dividend

        //    cash.numValue = dividend;//Random.Range(60, 500);
        //    cash.denValue = divisor;//cash.numValue / Random.Range(2, 50);
        //    cash.price = quotient;//cash.numValue / cash.denValue;
        //    GameObject obj = Instantiate(itemPrefab);
        //    itemSprites.Add(obj.GetComponent<Image>());
        //    obj.transform.SetParent(panel);
        //    obj.GetComponent<Image>().preserveAspect = true;
        //    obj.GetComponent<Image>().sprite = exchangeSprite;
        //    itemsWanted.Add(cash);

        //}

        // For display 
        
        foreach (Item selectedItem in MathProblemManager.instance.GetCurrentItemsWanted(true))
        {
           
            GameObject itemUI = Instantiate(MathProblemManager.instance.itemIconTemplate);
      
            itemUI.transform.SetParent(panel);
            itemUI.GetComponent<Image>().preserveAspect = true;
            itemUI.GetComponent<Image>().sprite = selectedItem.itemSprite;
            itemUI.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            itemsImage.Add(itemUI.GetComponent<Image>());
            var tc = itemUI.GetComponent<Image>().color;
            tc.a = 0f;
            itemUI.GetComponent<Image>().color = tc;
            AudioManager.instance.playSound(12);
        }
        // LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)HorizontalLayoutGroup.transform);
        //Canvas.ForceUpdateCanvases();
        //StartCoroutine(WaitUntilEndOfFrame());
        //Canvas.ForceUpdateCanvases();
        var tempColor = panel.gameObject.GetComponent<Image>().color;
        tempColor.a = 0f;
        panel.gameObject.GetComponent<Image>().color = tempColor;
        //Debug.Log(new Vector2(panel.gameObject.GetComponent<RectTransform>().rect.width, panel.gameObject.GetComponent<RectTransform>().rect.height));
        StartCoroutine(SaveRectSize());
        


    }

    IEnumerator SaveRectSize()
    {
       
        yield return new WaitForSeconds(0.05f);
        saved = true;
        //Debug.Log(new Vector2(panel.gameObject.GetComponent<RectTransform>().rect.width, panel.gameObject.GetComponent<RectTransform>().rect.height));
        //savedSize = new Vector2(panel.gameObject.GetComponent<RectTransform>().rect.width, panel.gameObject.GetComponent<RectTransform>().rect.height);
        savedSize = panel.gameObject.GetComponent<RectTransform>().sizeDelta;
        panel.gameObject.GetComponent<ContentSizeFitter>().enabled = false;
        panel.gameObject.GetComponent<HorizontalLayoutGroup>().enabled = false;

   
  

        foreach (Image selectedImage in itemsImage)
        {
            selectedImage.gameObject.SetActive(false);
            //var tc = selectedImage.gameObject.GetComponent<Image>().color;
            //tc.a = 1f;
            //selectedImage.gameObject.GetComponent<Image>().color = tc;
        }
        
        if (disappearing)
        {
            StartCoroutine(ThoughtBubbleDisappear());
        }
    }



    
    public IEnumerator ThoughtBubbleAppear()
    {
        float duration = 0.25f;
        //Sequence sequence = DOTween.Sequence();
        panel.gameObject.SetActive(true);

        //Move
    //    panel.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(savedSize.x,4f);
        panel.gameObject.GetComponent<RectTransform>().DOSizeDelta(new Vector2(savedSize.x, savedSize.y), duration);

        //FadeIn
        // sequence.Append(
        panel.gameObject.GetComponent<Image>().DOFade(1.0f, duration);//);
        //sequence.Play();
       
        //yield return sequence.WaitForCompletion(); // Wait to finish
        //yield return new WaitForSeconds(duration * 0.5f);
        
        foreach (Image selectedImage in itemsImage)
        {
            yield return new WaitForSeconds(duration / itemsImage.Count);
            if (selectedImage != null)
            {
                selectedImage.DOFade(1.0f, duration / itemsImage.Count);
                selectedImage.gameObject.SetActive(true);
            }
           
            
        }
        if(panel != null)
        {
            panel.gameObject.GetComponent<ContentSizeFitter>().enabled = true;
            panel.gameObject.GetComponent<HorizontalLayoutGroup>().enabled = true;
        }

        canAcceptItem = true;

    }
    public IEnumerator ThoughtBubbleDisappear()
    {
        float duration = 0.25f;
        if (saved)
        {
            panel.gameObject.GetComponent<ContentSizeFitter>().enabled = false;
            panel.gameObject.GetComponent<HorizontalLayoutGroup>().enabled = false;
            panel.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f);
            foreach (Image selectedImage in itemsImage)
            {
                selectedImage.gameObject.SetActive(false);
            }

          
            Sequence sequence = DOTween.Sequence();

            panel.gameObject.SetActive(false);



            //Move
            sequence.Append(panel.gameObject.GetComponent<RectTransform>().DOSizeDelta(new Vector2(0f, 0f), duration));

            //FadeIn
            sequence.Append(panel.gameObject.GetComponent<Image>().DOFade(0.0f, duration));
            sequence.Play();
        }
        else if (!saved)
        {
            disappearing = true;
        }

        canAcceptItem = false;
        yield return new WaitForSeconds(duration);
      
    }
}

   