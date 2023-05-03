using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{
    public GameObject Menu;
    public GameObject Partida;
    public GameObject Summon;
    public GameObject Mazo;
    public GameObject SelecNv;
    public GameObject PanelTirada;
    public GameObject PanelPerder;
    public GameObject PanelGanar;

    public ManoJugador manoJugador;
    public DataBase db;

    // Start is called before the first frame update
    void Start()
    {
        Summon.SetActive(false);
        Mazo.SetActive(false);
        Menu.SetActive(true);
        Partida.SetActive(false);
        SelecNv.SetActive(false);
    }

    public void Volver(){
        Summon.SetActive(false);
        Mazo.SetActive(false);
        Menu.SetActive(true);
        SelecNv.SetActive(false);
    }

    public void Jugar(){
        if(db.jugar){
            SelecNv.SetActive(true);
            Menu.SetActive(false); 
        } 
    }

    public void NivelSeleccionado(){
        manoJugador.EmpezarPartida();
        Partida.SetActive(true);
        SelecNv.SetActive(false);
    }

    public void GoSummon(){
        Summon.SetActive(true);
        Menu.SetActive(false);
        PanelTirada.SetActive(false);
    }

    public void GoCartas(){
        Mazo.SetActive(true);
        Menu.SetActive(false);
    }

    public void VolverMenu(){
        Menu.SetActive(true);
        Partida.SetActive(false);
        PanelGanar.SetActive(false);
        PanelPerder.SetActive(false);
    }

    public void Salir(){
        Application.Quit();
    }

}
