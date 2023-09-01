using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Text itemDescription;
    public Text itemName;
    public Image itemIcon;
    public Transform tooltipHolder;
    public bool isEnabled;

    private static Tooltip _instance;

    public static Tooltip instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Tooltip>();
            }

            return _instance;
        }
    }

    public void EnableTooltip(int itemId)
    {
        if(!Inventory.instance.isMovingItem)
        {
            tooltipHolder.gameObject.SetActive(true);
            itemName.text = InventoryDatabase.instance.GetItemWithID(itemId).itemName;
            itemDescription.text = InventoryDatabase.instance.GetItemWithID(itemId).itemInfo;
            itemIcon.sprite = InventoryDatabase.instance.GetItemWithID(itemId).itemIcon;
            isEnabled = true;
        }
    }

    private void Update()
    {
        if(isEnabled)
        {
            tooltipHolder.SetPositionAndRotation(Input.mousePosition, Quaternion.identity);
        }
    }

    public void DisableTooltip()
    {
        tooltipHolder.gameObject.SetActive(false);
        isEnabled = false;
    }
}
