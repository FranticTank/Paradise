using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform zona = null;
    public bool eliminar = false;

    public void OnBeginDrag(PointerEventData eventData){
        zona = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData){
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData){
        this.transform.SetParent(zona);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if(eliminar){
            Destroy(this);
        }
    }
}
