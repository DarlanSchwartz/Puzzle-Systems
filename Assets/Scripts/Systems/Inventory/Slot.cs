using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour  , IPointerClickHandler , IPointerEnterHandler , IPointerExitHandler , IDragHandler
{
    public Item currentItem;
    public Image itemIcon;

    public void OnPointerClick(PointerEventData eventData)
    {
        Inventory.instance.PlaceItem(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Inventory.instance.StartMovingItem(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(currentItem!=null)
        {
            Tooltip.instance.EnableTooltip(currentItem.itemId);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.instance.DisableTooltip();
    }

    public void Clear()
    {
        currentItem = null;
        itemIcon.sprite = null;
        itemIcon.enabled = false;
    }

    public void SetItem(Item item)
    {
        currentItem = item;
        itemIcon.sprite = item.itemIcon;
        itemIcon.enabled = true;
    }

    public bool isEmpty()
    {
        return currentItem == null;
    }

    
}
