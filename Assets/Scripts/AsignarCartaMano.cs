using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AsignarCartaMano : MonoBehaviour
{

    public TextMeshProUGUI nomText;
    public TextMeshProUGUI desText;
    public TextMeshProUGUI atkText;
    public TextMeshProUGUI defText;
    public TextMeshProUGUI oroText;
    public Image img;

    public Carta carta;

    public int aMomentaneo;
    public int dMomentaneo;

    public bool puedeAtacar;
    public bool puedeRevivir;

    // Start is called before the first frame update
    void Start()
    {
        Asignar(carta);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Asignar(Carta c)
    {
        carta = c;
        nomText.text = c.nombre;
        desText.text = c.descripcion;
        atkText.text = c.atk.ToString();
        defText.text = c.def.ToString();
        oroText.text = c.oro.ToString();
        img.sprite = c.img;

        aMomentaneo = c.atk;
        dMomentaneo = c.def;

        if(c.nombre == "Fenix"){
            puedeRevivir = true;
        }
    }

    public void ResetStats(){
        atkText.text = aMomentaneo.ToString();
        defText.text = dMomentaneo.ToString();
    }

    public void AumentoStats(int atk, int def){
        aMomentaneo += atk;
        dMomentaneo += def;
        atkText.text = aMomentaneo.ToString();
        defText.text = dMomentaneo.ToString();
    }
}
