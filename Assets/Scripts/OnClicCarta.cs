using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClicCarta : MonoBehaviour
{
    public CartasTablero ct;
    public CartasIA ci;
    // Start is called before the first frame update
    void Start()
    {
        ct = GameObject.Find("CartasJugador").GetComponent<CartasTablero>();
        ci = GameObject.Find("CartasRival").GetComponent<CartasIA>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LlamaAtacar(GameObject g){
        ct.Atacar(g);
        Debug.Log(g.GetComponent<AsignarCartaMano>().nomText);
    }

    public void LlamaDefender(GameObject g){
        ci.Defender(g);
    }
}
