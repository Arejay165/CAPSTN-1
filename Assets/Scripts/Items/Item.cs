using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public int price;
    public int numValue;
    public int denValue;
    public Sprite itemSprite;
    public float quantity;

}
