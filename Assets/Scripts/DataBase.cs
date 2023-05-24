using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DataBase : MonoBehaviour
{

    public List<Carta> mazo = new List<Carta>();
    //public List<Carta> cartasObtenidas = new List<Carta>();
    public GameObject[] cartas;//todas las cartas i ir poniendo en enable
 
    public Carta[] cartasComunes;//20: 0-19
    public int nCC;
    public Carta[] cartasEpicas;//10: 20-29
    public int nCE;
    public Carta[] cartasLegendarias;//6: 30-35
    public int nCL;
    
    public GameObject CartaPrefab;

    public GameObject PanelCartaElegida;
    public GameObject Summon;
    public GameObject POV;

    public bool jugar = false;
    public TextMeshProUGUI Tdinero;
    public int dinero;
    // Start is called before the first frame update
    void Start()
    {
        RefrescarDinero();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefrescarDinero(){
        Tdinero.text = dinero.ToString();
    }

    public void eliminarCartaMazo(Carta c){
        mazo.Remove(c);
    }

    public void Tirar(){
        //videoTiradaLeg.SetActive(true);
        //StartCoroutine(Espera());
        int n;
        if (dinero >= 100){
            dinero -= 100;
            Tdinero.text = dinero.ToString();
            if(Random.value < 0.7f){
                int carId = Random.Range(0, nCC);
                Instanciar(cartasComunes[carId]);
                Debug.Log(cartasComunes[carId]);
                if(cartas[carId].GetComponent<Button>().interactable == false){
                    cartas[carId].GetComponent<Button>().interactable = true;
                }
                else dinero += 50;
                n = carId;
            }
            else if(Random.value < 0.95f){
                int carId = Random.Range(0, nCE);
                Instanciar(cartasEpicas[carId]);
                Debug.Log(cartasEpicas[carId]);
                if(cartas[carId+20].GetComponent<Button>().interactable == false){
                    cartas[carId+20].GetComponent<Button>().interactable = true;
                }
                else dinero += 50;
                n = carId+20;
            }
            else{
                int carId = Random.Range(0, nCL);
                Instanciar(cartasLegendarias[carId]);
                Debug.Log(cartasLegendarias[carId]);
                if(cartas[carId+30].GetComponent<Button>().interactable == false){
                    cartas[carId+30].GetComponent<Button>().interactable = true;
                }
                else dinero += 50;
                n = carId+30;
            }
        }
        PanelCartaElegida.SetActive(true);
        Summon.SetActive(false);
    }

    private void Instanciar(Carta c){
        GameObject g = Instantiate(CartaPrefab, Vector2.zero, Quaternion.identity);
        g.transform.SetParent(POV.transform);
        g.transform.localScale = Vector3.one;
        g.GetComponent<AsignarCartaMano>().Asignar(c);
    }

    /*IEnumerator Espera(){
        yield return new WaitForSeconds(6);
        videoTiradaLeg.SetActive(false);
        PanelCartaElegida.SetActive(true);
        Summon.SetActive(false);
    }*/

    public void AñadirMazo(Carta c){
        mazo.Add(c);
        if(mazo.Count > 7){
            jugar = true;
        }
    }

    public void EliminarMazo(Carta c){
        if(mazo.Contains(c)){
            mazo.Remove(c);
        }
        if(mazo.Count < 7 && jugar){
            jugar = false;
        }
    }
}
