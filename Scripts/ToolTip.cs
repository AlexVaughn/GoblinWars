using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    GameObject tooltipPrefab;
    [SerializeField]
    [TextAreaAttribute]
    string description;
    GameObject tooltip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip = Instantiate(tooltipPrefab, transform.parent.parent);
        tooltip.GetComponent<LiveToolTip>().SetText(description);
    }

    // This method is called when the mouse pointer exits the GameObject
    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(tooltip);
    }
}