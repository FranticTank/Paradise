using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManoJugador : MonoBehaviour
{

    //public List<Carta> mano = new List<Carta>();
    public GameObject[] Cmano = new GameObject[10];
    public GameObject CartaPrefab;
    public TextMeshProUGUI oroText;
    public TextMeshProUGUI vidaText;
    public DataBase db;

    public int nMano = 0;
    public int oro = 0;
    public int vida = 30;

    void Start()
    {
        oro = 30;
        vida = 30;
        RefrescarOro();
        RefrescarVida();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefrescarOro(){
        oroText.text = oro.ToString();
    }
    public void RefrescarVida(){
        vidaText.text = vida.ToString();
    }

    public void EmpezarPartida(){
        nMano = 7;
        for (int i = 0; i <= nMano; i++){
            ObtenerCarta(i);
        }
    }

    public void Robar(){
        Debug.Log("Robando");
        if(nMano <= 10){
            nMano += 1;
            ObtenerCarta(nMano);
        }
        
    }

    public void EliminaMano(GameObject g){
        Debug.Log("elimina");
        bool encontrado = false;
        int i = 0;
        while (i <= nMano && !encontrado){
            if(Cmano[i].Equals(g)){
                Cmano[i] = Cmano[nMano];
                Cmano[nMano] = null;
                encontrado = true;
            }
            i++;
        }
        nMano -= 1;
    }

    public void InutilizarCartas(){
        for (int i = 0; i <= nMano; i++){
            Cmano[i].GetComponent<Draggable>().enabled = false;
        }
    }

    public void UtilizarCartas(){
        for (int i = 0; i <= nMano; i++){
            Cmano[i].GetComponent<Draggable>().enabled = true;
        }
    }

    private void ObtenerCarta(int i){
        int x = Random.Range(0, db.mazo.Count);
        GameObject c = Instantiate(CartaPrefab, Vector2.zero, Quaternion.identity);
        c.transform.SetParent(this.transform);
        c.transform.localScale = Vector3.one;
        Cmano[i] = c;
        Cmano[i].GetComponent<AsignarCartaMano>().Asignar(db.mazo[x]);
    }

    public void VaciarMano(){
        for(int i = 0; i < 10; i++){
            if(Cmano[i] != null){
                GameObject g = Cmano[i];
                Destroy(g);
            } 
        }
    }
}
