using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartasTablero : MonoBehaviour
{

    public List<GameObject> mazo = new List<GameObject>();
    public List<GameObject> mazoCartasUsadas = new List<GameObject>();

    public ManoJugador mj;
    public CartasIA ci;
    public DataBase db;

    public GameObject cartaArbol;
    public GameObject cartaGuerrero;
    public GameObject cartaEscudero;
    public GameObject botonAcabar;
    public GameObject atacante;

    public GameObject PanelPerder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AñadirAlTablero(GameObject g){
        mj.EliminaMano(g);
        g.GetComponent<AsignarCartaMano>().puedeAtacar = true;
        Carta c = g.GetComponent<AsignarCartaMano>().carta;
        if (c.descripcion.Length > 0){
            Accion(c, g);
        }
        else{
            mazo.Add(g);
        }
    }

    public void AcabarTurno(){
        atacante = null;
        mj.InutilizarCartas();
        for(int i = 0; i < mazo.Count; i++){
            if(mazo[i].GetComponent<AsignarCartaMano>().carta.nombre == "Recolector Oro"){
                mj.oro += 3;
                mj.RefrescarOro();
            }
            else if(mazo[i].GetComponent<AsignarCartaMano>().carta.nombre == "Fabrica Oro"){
                mj.oro += 5;
                mj.RefrescarOro();
            }
        }
        ci.TurnoIA();
        //botonAcabar.GetComponent<Button>().interactable = false;
    }

    public void EmpezarTurno(){
        mj.oro += 5;
        mj.RefrescarOro();
        mj.Robar();
        mj.Robar();
        mj.UtilizarCartas();
        //botonAcabar.GetComponent<Button>().interactable = true;
        Debug.Log(botonAcabar.GetComponent<Button>().interactable);
        for(int i = 0; i < mazoCartasUsadas.Count; i++){
            mazoCartasUsadas[i].GetComponent<Button>().interactable = true;
            mazo.Add(mazoCartasUsadas[i]);
        }
        mazoCartasUsadas.Clear();
    }

    public void Atacar(GameObject g){
        Debug.Log(mj);
        if(g.GetComponent<AsignarCartaMano>().puedeAtacar){
            atacante = g;
        }     
    }

    public void Defender(GameObject g){
        //Desarrollar mas
        int dmg = g.GetComponent<AsignarCartaMano>().aMomentaneo;
        Debug.Log("atacas: "+dmg);
        if(g.GetComponent<AsignarCartaMano>().carta.nombre == "Gran Arbol"){
            ci.vidaIA += dmg;
            ci.VidaText.text = ci.vidaIA.ToString();
        }
        else if(g.GetComponent<AsignarCartaMano>().carta.nombre == "Pandilleros" || g.GetComponent<AsignarCartaMano>().carta.nombre == "Goblin Ladron"){
            int oroRobado = dmg;
            mj.oro -= oroRobado;
            if(mj.oro < 0){
                oroRobado += mj.oro;
                mj.oro = 0;
            }
            ci.oro += oroRobado;
            ci.OroText.text = ci.oro.ToString();
            //Se tiene que mostrar el oro
        }
        else if(g.GetComponent<AsignarCartaMano>().carta.nombre == "Oni"){
            g.GetComponent<AsignarCartaMano>().AumentoStats(1, 0);
        }
        else if(g.GetComponent<AsignarCartaMano>().carta.nombre == "Sr del Vacio"){
            g.GetComponent<AsignarCartaMano>().AumentoStats(0, 1);
        }
        else if(g.GetComponent<AsignarCartaMano>().carta.nombre == "Dios Goblin"){
            int x = Random.Range(0, 2);
            if(x == 0) g.GetComponent<AsignarCartaMano>().AumentoStats(1, 0);
            else g.GetComponent<AsignarCartaMano>().AumentoStats(0, 1);
        }

        if(mazo.Count == 0){//Ataca a ti
            if(g.GetComponent<AsignarCartaMano>().carta.nombre == "Vampiro"){
                ci.vidaIA += dmg;
            }
            mj.vida -= dmg;
            mj.RefrescarVida();
            if(mj.vida < 1){
                PanelPerder.SetActive(true);
                db.dinero += 1000;
                db.RefrescarDinero();
            }
        }
        else{//mirar que ataque con prioridad a alguien bajo de vida
            int x = Random.Range(0, mazo.Count);
            mazo[x].GetComponent<AsignarCartaMano>().dMomentaneo -= dmg;
            if(mazo[x].GetComponent<AsignarCartaMano>().dMomentaneo < 1){
                if(mazo[x].GetComponent<AsignarCartaMano>().puedeRevivir){
                    mazo[x].GetComponent<AsignarCartaMano>().puedeRevivir = false;
                    g.GetComponent<AsignarCartaMano>().aMomentaneo = 3;
                    g.GetComponent<AsignarCartaMano>().dMomentaneo = 2;
                    g.GetComponent<AsignarCartaMano>().ResetStats();
                }
                else{
                    Destroy(mazo[x]);
                    mazo.RemoveAt(x);
                }
            }
            else{
                if(g.GetComponent<AsignarCartaMano>().carta.nombre == "Sirena"){
                    mazo[x].GetComponent<AsignarCartaMano>().aMomentaneo -= 1;
                }
                mazo[x].GetComponent<AsignarCartaMano>().ResetStats();
            } 
        }      
        g.GetComponent<Button>().interactable = false;
    }

    public void AtacarMina(){
        if(atacante != null){
            int i = 0;
            bool encontrado = false;
            while(i < mazo.Count && !encontrado){
                if(mazo[i] == atacante){
                    encontrado = true;
                    mazo[i].GetComponent<Button>().interactable = false;
                    mj.oro += mazo[i].GetComponent<AsignarCartaMano>().aMomentaneo;
                    Debug.Log("atacar minaaaaa");
                    mj.RefrescarOro();
                    mazoCartasUsadas.Add(mazo[i]);
                    mazo.RemoveAt(i);
                }
                else i++;
            }
            atacante = null;
        }
    }

    public void CartaUsada(bool atkDirecto){
        int i = 0;
        bool encontrado = false;
        while(i < mazo.Count && !encontrado){
            if(mazo[i] == atacante){
                encontrado = true;
                mazo[i].GetComponent<Button>().interactable = false;

                if((atkDirecto && mazo[i].GetComponent<AsignarCartaMano>().carta.nombre == "Vampiro") || mazo[i].GetComponent<AsignarCartaMano>().carta.nombre == "Gran Arbol"){
                    mj.vida += mazo[i].GetComponent<AsignarCartaMano>().aMomentaneo;
                }
                else if(mazo[i].GetComponent<AsignarCartaMano>().carta.nombre == "Oni"){
                    mazo[i].GetComponent<AsignarCartaMano>().AumentoStats(1, 0);
                }
                else if(mazo[i].GetComponent<AsignarCartaMano>().carta.nombre == "Sr del Vacio"){
                    mazo[i].GetComponent<AsignarCartaMano>().AumentoStats(0, 1);
                }
                else if(mazo[i].GetComponent<AsignarCartaMano>().carta.nombre == "Dios Goblin"){
                    int x = Random.Range(0, 2);
                    if(x == 0) mazo[i].GetComponent<AsignarCartaMano>().AumentoStats(1, 0);
                    else mazo[i].GetComponent<AsignarCartaMano>().AumentoStats(0, 1);
                }
                else if(mazo[i].GetComponent<AsignarCartaMano>().carta.nombre == "Pandilleros" || mazo[i].GetComponent<AsignarCartaMano>().carta.nombre == "Goblin Ladron"){
                    int oroRobado = mazo[i].GetComponent<AsignarCartaMano>().aMomentaneo;
                    ci.oro -= oroRobado;
                    if(ci.oro < 0){
                        oroRobado += ci.oro;
                        ci.oro = 0;
                    }
                    mj.oro += oroRobado;
                    mj.RefrescarOro();
                }
                mazoCartasUsadas.Add(mazo[i]);
                mazo.RemoveAt(i);
            }
            else i++;
        }
        atacante = null;
    }

    public void RestarStats(int nCartas, int atk, int def){
        if(nCartas > mazo.Count) nCartas = mazo.Count;
        int x = Random.Range(0, mazo.Count);
        for(int i = 0; i < nCartas; i++){
            mazo[x].GetComponent<AsignarCartaMano>().AumentoStats(-2, 0);
            int y = Random.Range(0, mazo.Count);
            while(y == x){
                y = Random.Range(0, mazo.Count);
            }
            x = y;
        }
    }


    private void Accion(Carta c, GameObject g){
        switch (c.nombre)
        {
            case "Florecimiento":
                AumentaStats("Natura", 1, 1);
                Destroy(g);
                break;
            case "Eclipse":
                AumentaStats("Criatura", 1, 1);
                AumentaStats("Abismo", 1, 1);
                Destroy(g);
                break;
            case "Goblin Explorador":
                mazo.Add(g);
                mj.Robar();
                break;
            case "Kazulu":
                mazo.Add(g);
                mj.Robar();
                break;
            case "Hechicero Goblin":
                AumentaStats("Goblin", 1, 0);
                mazo.Add(g);
                break;
            case "Mercader":
                mazo.Add(g);
                mj.Robar();
                break;
            case "Bosque Sagrado":
                mazo.Add(g);
                SpawnObjeto(cartaArbol);
                SpawnObjeto(cartaArbol);
                break;
            case "Susurra Amapolas":
                mazo.Add(g);
                AumentaStatsNombre("Amapola", 3, 0);
                break;
            case "Comandante":
                mazo.Add(g);
                AumentaStats("Humano", 1, 1);
                break; 
            case "Rey Goblin":
                mazo.Add(g);
                AumentaStats("Goblin", 1, 1);
                break;  
            case "Protector Abismo":
                mazo.Add(g);
                AumentaStats("Abismo", 0, 1);
                break;
            case "Kappa":
                mazo.Add(g);
                AumentaStats("Criatura", 1, 0);
                break;
            case "Hadas":
                mazo.Add(g);
                ci.RestarStats(2, 2, 0);
                break;
            case "Gobernador":
                mazo.Add(g);
                SpawnObjeto(cartaGuerrero);
                SpawnObjeto(cartaEscudero);
                break;
            default:
                mazo.Add(g);
                break;
        }
    }


    private void AumentaStats(string tipo, int atk, int def){
        for(int i = 0; i < mazo.Count; i++){
            if (mazo[i].GetComponent<AsignarCartaMano>().carta.tipoCarta.ToString() == tipo){
                mazo[i].GetComponent<AsignarCartaMano>().AumentoStats(atk, def);
            }
        }
    }

    private void AumentaStatsNombre(string n, int atk, int def){
        for(int i = 0; i < mazo.Count; i++){
            Debug.Log(mazo[i].GetComponent<AsignarCartaMano>().carta);
            if (mazo[i].GetComponent<AsignarCartaMano>().carta.nombre == n){
                mazo[i].GetComponent<AsignarCartaMano>().AumentoStats(atk, def);
            }
        }
    }

    private void SpawnObjeto(GameObject g){
        GameObject a1 = Instantiate(g, Vector2.zero, Quaternion.identity);
        a1.transform.SetParent(this.transform);
        a1.transform.localScale = Vector3.one;
        mazo.Add(a1);
    }

    public void BorrarTablero(){
        mj.VaciarMano();
        int aux = mazo.Count;
        for(int i = 0; i < aux; i++){
            GameObject g = mazo[i];
            Destroy(g);
        }
        mazo.Clear();
    }
}
