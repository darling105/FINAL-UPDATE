using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    [Header("Item Infomation")]
    public string itemName;
    public Sprite itemIcon;
    [TextArea] public string itemDescription;
    public int itemID;
}
