using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DetectItemInWindow : MonoBehaviour
{

    [SerializeField] private Scoring score;
    [SerializeField] public Color darkenImage;
    //public bool invincibility = false;
    // Start is called before the first frame update
    void Start()
    {
        global::GameManager.instance.window = this;
        
    }
    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (invincibility == false)
        //{
            if (collision.gameObject.tag == "Item")
            {
                collision.gameObject.transform.SetParent(this.gameObject.transform);
                ItemDescription itemInCounter = collision.GetComponent<ItemDescription>();

                if (GameManager.instance.customer != null)
                {
                    MoodComponent mc = GameManager.instance.customer.GetComponent<MoodComponent>();
                    for (int i = 0; i < MathProblemManager.instance.GetCurrentItemsWanted().Count;)
                    {
                        if (itemInCounter.item.itemName == MathProblemManager.instance.GetItemInCurrentItemsWanted(i).itemName)
                        {
                        if (GameManager.instance.customer.canAcceptItem)
                        {
                            mc.IncreaseCurrentMoodAmount(mc.correctBonusTime * 2);// 1 second
                            GameManager.instance.customer.itemsImage[i].color = global::GameManager.instance.window.darkenImage;
                            GameManager.instance.customer.itemsImage.RemoveAt(i);
                            MathProblemManager.instance.GetCurrentItemsWanted().RemoveAt(i);
                            if (PlayerManager.instance.lastItemSpawner)
                            {
                                PlayerManager.instance.lastItemSpawner.canSpawn = true;
                            }
                            if (MathProblemManager.instance.GetCurrentItemsWanted().Count <= 0)
                            {
                                //Disable customer bubble
                                StartCoroutine(GameManager.instance.customer.ThoughtBubbleDisappear());


                                //ordersheet 
                                TransitionManager.instances.MoveTransition(new Vector2(680f, 0f), 0.5f, TransitionManager.instances.noteBookTransform, TransitionManager.instances.noteBookTransform.gameObject, true);
                            }
                            AudioManager.instance.playSound(2);
                            Destroy(itemInCounter.gameObject, 0.2f);
                            //    Debug.Log("DESTROYING THRU DETECT CORRECT");
                            PlayerManager.instance.lastItemSpawner = null;
                            break;

                        }


                    }
                    i++;
                    if (i >= MathProblemManager.instance.GetCurrentItemsWanted().Count)
                    {
                        Debug.Log("Wrong Item");
                        Destroy(itemInCounter.gameObject, 0.2f);
                        mc.DeductCurrentMoodAmount(mc.penaltyTime);// 1 second
                        Scoring.instance.ResetMultiplier();
                        AudioManager.instance.playSound(3);
                        PlayerManager.instance.CamShake(PlayerManager.instance.GameCamera.gameObject, 0.2f, 0.05f, 15f);
                    }

                }
            }
            else
            {
                Debug.Log("Wrong Item");
                AudioManager.instance.playSound(3);
                Destroy(itemInCounter.gameObject, 0.2f);
            }
        }
    }

    //IEnumerator Cooldown()
    //{
    //    invincibility = true;
    //    yield return new WaitForSeconds(0.45f);
    //    invincibility = false;
    //}
}
