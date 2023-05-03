using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mazo : MonoBehaviour
{

    public GameObject mazo;
    public GameObject cartas;

    public DataBase db;

    private GameObject cMazo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Volver(){
        cartas.SetActive(false);
        mazo.SetActive(true);
    }

    public void selCarta(GameObject g){
        cartas.SetActive(true);
        mazo.SetActive(false);
        cMazo = g;
        db.EliminarMazo(cMazo.GetComponent<AsignarCarta>().carta);
    }

    public void CartaEscogida(Carta c){
        cartas.SetActive(false);
        mazo.SetActive(true);
        cMazo.GetComponent<AsignarCarta>().NuevaCarta(c);
        db.AñadirMazo(c);
    }
}
