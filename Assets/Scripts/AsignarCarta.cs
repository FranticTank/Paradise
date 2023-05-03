using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AsignarCarta : MonoBehaviour
{

    public Carta carta;

    public TextMeshProUGUI nomText;
    public TextMeshProUGUI desText;
    public TextMeshProUGUI atkText;
    public TextMeshProUGUI defText;
    public TextMeshProUGUI oroText;
    public Image img;
    public int nMomento = 0;

    public GameObject Mazo;
    public GameObject Cartas;

    // Start is called before the first frame update
    void Start()
    {
        Asignar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NuevaCarta(Carta c){
        carta = c;
        Asignar();
    }

    private void Asignar(){
        nomText.text = carta.nombre;
        desText.text = carta.descripcion;
        atkText.text = carta.atk.ToString();
        defText.text = carta.def.ToString();
        oroText.text = carta.oro.ToString();
        img.sprite = carta.img;
    }
}
