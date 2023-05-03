using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZonaDrop : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string tipo = "Tablero";

    public CartasTablero ct;
    public ManoJugador mj;

    public void OnDrop(PointerEventData eventData){
        Draggable drag = eventData.pointerDrag.GetComponent<Draggable>();
        if(drag != null && tipo == "Tablero"){
            int oro = mj.oro - eventData.pointerDrag.GetComponent<AsignarCartaMano>().carta.oro;
            if(oro >= 0){
                mj.oro = oro;
                mj.RefrescarOro();
                drag.zona = this.transform;
                GameObject g = eventData.pointerDrag;
                ct.AñadirAlTablero(g);
                drag.eliminar = true;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData){

    }

    public void OnPointerExit(PointerEventData eventData){
        
    }
}
