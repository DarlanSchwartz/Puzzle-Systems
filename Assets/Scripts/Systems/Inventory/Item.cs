using UnityEngine;

[CreateAssetMenu(fileName = "NewItem",menuName ="Inventory Item", order = 0)]
public class Item : ScriptableObject
{
    public Sprite itemIcon;
    public string itemName;
    public string itemInfo;
    public int itemId = -1;
    public ItemType type = ItemType.None;
    public string prefabName;
}

public enum ItemType {None,Keycard,Key}
