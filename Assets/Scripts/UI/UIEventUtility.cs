using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

public class UIEventUtility : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler , IPointerDownHandler
{
    public UnityEvent OnMouseEnter; 
    public UnityEvent OnMouseExit;
    public UnityEvent OnMouseClick;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnMouseClick.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit.Invoke();
    }


}
