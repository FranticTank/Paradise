using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CartasIA : MonoBehaviour
{

    public GameObject[] ManoIA = new GameObject[10];
    public GameObject CartaPrefab;
    public GameObject cartaArbol;
    public GameObject zonaMano;

    public GameObject PanelGanar;

    public List<Carta> CarIA = new List<Carta>();
    public List<Carta> CarIAPlanta = new List<Carta>();
    public List<Carta> CarIAHumanos = new List<Carta>();
    public List<Carta> CarIAGoblins = new List<Carta>();
    public List<Carta> CarIACriaturas = new List<Carta>();
    public List<Carta> CarIAAbismo = new List<Carta>();

    public List<GameObject> CarTabIA = new List<GameObject>();

    public CartasTablero ct;
    public DataBase db;

    public TextMeshProUGUI Vida;
    public int oro = 40;
    public int vidaIA = 30;
    private int nMano = 0;

    // Start is called before the first frame update
    void Start()
    {
        Vida.text = vidaIA.ToString();
    }

    public void IA_Planta(){
        CarIA = CarIAPlanta;
        EmpezarPartida();
    }

    public void IA_Humanos(){
        CarIA = CarIAHumanos;
        EmpezarPartida();
    }

    public void IA_Goblins(){
        CarIA = CarIAGoblins;
        EmpezarPartida();
    }

    public void IA_Criaturas(){
        CarIA = CarIACriaturas;
        EmpezarPartida();
    }

    public void IA_Abismo(){
        CarIA = CarIAAbismo;
        EmpezarPartida();
    }

    public void TurnoIA(){
        //sacar las cartas al tablero
        //de las cartas puestas hacer las funciones
        //atacar i acabar turno
        ct.botonAcabar.GetComponent<Button>().interactable = false;

        if(nMano <= 9){
            nMano++;
            ObtenerCarta(nMano);
            nMano++;
            ObtenerCarta(nMano);
        }

        int aux = nMano;
        for (int i = 0; i < aux; i++){
            if ((oro - ManoIA[i].GetComponent<AsignarCartaMano>().carta.oro) > 0){
                Debug.Log("Tirar cartas");
                oro -= ManoIA[i].GetComponent<AsignarCartaMano>().carta.oro;
                //Se tiene que quitar de ManoIA
                if(ManoIA[i].GetComponent<AsignarCartaMano>().carta.hechizo){
                    if(CarTabIA.Count > 1){
                        //hacer hechizo
                        Accion(ManoIA[i].GetComponent<AsignarCartaMano>().carta, ManoIA[i]);
                    }
                }
                else{   
                    Debug.Log(ManoIA[i].GetComponent<AsignarCartaMano>().carta.nombre);  
                    ManoIA[i].transform.SetParent(this.transform);
                    ManoIA[i].transform.localScale = Vector3.one;
                    CarTabIA.Add(ManoIA[i]);
                    Accion(ManoIA[i].GetComponent<AsignarCartaMano>().carta, ManoIA[i]);
                    nMano--;
                }
            }
        }

        for (int i = 0; i < CarTabIA.Count; i++){
            CarTabIA[i].GetComponent<Button>().interactable = true;
        }

        if(CarTabIA != null){
            Debug.Log("AtacarIA");
            AtacarIA();
        }
        
        ct.botonAcabar.GetComponent<Button>().interactable = true;
        ct.EmpezarTurno();
    }

    private void EmpezarPartida(){
        //si se da mas de una vez a jugar lo hace mas veces.
        nMano = 7;
        for (int i = 0; i <= nMano; i++){
            Debug.Log("carta" + i);
            ObtenerCarta(i);
        }
    }

    public void DefenderPersonaje(){
        bool atacable = true;
        int i = 0;
        while(i < CarTabIA.Count && atacable){
            if (CarTabIA[i].GetComponent<Button>().interactable == true) atacable = false;
            i++;
        }

        if(atacable){
            int dmg = ct.atacante.GetComponent<AsignarCartaMano>().aMomentaneo;
            Debug.Log("atacas a la ia");
            vidaIA -= dmg;
            Vida.text = vidaIA.ToString();
            if(vidaIA < 1){
                PanelGanar.SetActive(true);
                db.dinero += 2000;
                Debug.Log("Has ganado");
            }
            ct.CartaUsada(true);
        }   
    }

    public void Defender(GameObject g){
        int dmg = ct.atacante.GetComponent<AsignarCartaMano>().aMomentaneo;
        Debug.Log("atacas a una carta de la ia");
        g.GetComponent<AsignarCartaMano>().dMomentaneo -= dmg;
        if(g.GetComponent<AsignarCartaMano>().dMomentaneo < 1){
            if(g.GetComponent<AsignarCartaMano>().puedeRevivir){
                g.GetComponent<AsignarCartaMano>().puedeRevivir = false;
                g.GetComponent<AsignarCartaMano>().aMomentaneo = 3;
                g.GetComponent<AsignarCartaMano>().dMomentaneo = 2;
                g.GetComponent<AsignarCartaMano>().ResetStats();
            }
            else{
                CarTabIA.Remove(g);
                Destroy(g);
            }
        }
        else{
            if (ct.atacante.GetComponent<AsignarCartaMano>().carta.nombre == "Sirena"){
                g.GetComponent<AsignarCartaMano>().aMomentaneo -= 1;
            }
            g.GetComponent<AsignarCartaMano>().ResetStats();
        } 
        
        ct.CartaUsada(false);
    }

    public void RestarStats(int nCartas, int atk, int def){
        if(nCartas > CarTabIA.Count) nCartas = CarTabIA.Count;
        int x = Random.Range(0, CarTabIA.Count);
        for(int i = 0; i < nCartas; i++){
            CarTabIA[x].GetComponent<AsignarCartaMano>().AumentoStats(-atk, -def);
            int y = Random.Range(0, CarTabIA.Count);
            while(y == x){
                y = Random.Range(0, CarTabIA.Count);
            }
            x = y;
        }
    }

    private void ObtenerCarta(int i){
        //hay que cambiar para que no se vean las cartas de la ia i solo cuando las pone en el tablero
        int x = Random.Range(0, CarIA.Count);
        GameObject c = Instantiate(CartaPrefab, Vector2.zero, Quaternion.identity);
        c.transform.SetParent(zonaMano.transform);
        c.transform.localScale = Vector3.one;
        ManoIA[i] = c;
        ManoIA[i].GetComponent<AsignarCartaMano>().Asignar(CarIA[x]);
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
                if(nMano <= 10){
                    nMano++;
                    ObtenerCarta(nMano);
                }      
                break;
            case "Kazulu":
                if(nMano <= 10){
                    nMano++;
                    ObtenerCarta(nMano);
                }      
                break;
            case "Hechicero Goblin":
                AumentaStats("Goblin", 1, 0);
                break;
            case "Mercader":
                if(nMano <= 10){
                    nMano++;
                    ObtenerCarta(nMano);
                }  
                break;
            case "Bosque Sagrado":
                SpawnArbol();
                break;
            case "Susurra Amapolas":
                AumentaStatsNombre("Amapola", 3, 0);
                break;
            case "Comandante":
                AumentaStats("Humano", 1, 1);
                break;
            case "Rey Goblin":
                AumentaStats("Goblin", 1, 1);
                break;
            case "Protector Abismo":
                AumentaStats("Abismo", 0, 1);
                break;
            case "Kappa":
                AumentaStats("Criatura", 1, 0);
                break;  
            case "Hadas":
                ct.RestarStats(2, 2, 0);
                break;
            default:
                break;
        }
    }


    private void AumentaStats(string tipo, int atk, int def){
        for(int i = 0; i < CarTabIA.Count; i++){
            if (CarTabIA[i].GetComponent<AsignarCartaMano>().carta.tipoCarta.ToString() == tipo){
                CarTabIA[i].GetComponent<AsignarCartaMano>().AumentoStats(atk, def);
            }
        }
    }

    private void AumentaStatsNombre(string n, int atk, int def){
        for(int i = 0; i < CarTabIA.Count; i++){
            Debug.Log(CarTabIA[i].GetComponent<AsignarCartaMano>().carta);
            if (CarTabIA[i].GetComponent<AsignarCartaMano>().carta.nombre == n){
                CarTabIA[i].GetComponent<AsignarCartaMano>().AumentoStats(atk, def);
            }
        }
    }

    private void SpawnArbol(){
        GameObject a1 = Instantiate(cartaArbol, Vector2.zero, Quaternion.identity);
        a1.transform.SetParent(this.transform);
        a1.transform.localScale = Vector3.one;
        CarTabIA.Add(a1);
        GameObject a2 = Instantiate(cartaArbol, Vector2.zero, Quaternion.identity);
        a2.transform.SetParent(this.transform);
        a2.transform.localScale = Vector3.one;
        CarTabIA.Add(a2);
    }

    private int MinMax(int posCarta){
        //Devuleve si no atacar, recolectar oro o atacar al rival. (0,1,2)
        int mejorMov = 0;
        for (int i = 0; i < 3; i++){

        }
        return mejorMov;
    }

    private void AtacarIA(){
        int dif = CarTabIA.Count - ct.mazo.Count;
        for (int i = 0; i < CarTabIA.Count; i++){//Mirar si vale la pena atacar o dejar alguna para defender
            if(CarTabIA[i].GetComponent<AsignarCartaMano>().carta.nombre == "Recolector Oro"){
                oro += 3;
            }
            else if(CarTabIA[i].GetComponent<AsignarCartaMano>().carta.nombre == "Fabrica Oro"){
                oro += 5;
            }
            //Llamar minMax
            int opcion = MinMax(i);

            if(opcion == 1){//ataca mina
                oro += CarTabIA[i].GetComponent<AsignarCartaMano>().aMomentaneo;
            }
            else if(opcion == 2){

            }




            if(ct.mazo.Count < 1){
                ATK(i);
            }
            else{
                if(dif > 0){
                    if(Random.value < 0.8f){
                        ATK(i);
                    }
                }
                else{
                    if(Random.value < 0.3f){
                        ATK(i);
                    }
                }
            }
            //Esperar 1 segundo entre ataques     
        }
        
    }
    private void ATK(int i){
        if (oro > 50 && Random.value < 0.9f){
            ct.Defender(CarTabIA[i]);
        }
        else if(oro > 30 && Random.value < 0.6f){
            ct.Defender(CarTabIA[i]);
        }
        else if(oro > 10 && Random.value < 0.3f){
            ct.Defender(CarTabIA[i]);
        }
        else {
            //atacar mina
            oro += CarTabIA[i].GetComponent<AsignarCartaMano>().aMomentaneo;
            //CarTabIA[i].GetComponent<Button>().interactable = false;
        }
        
    }

    public void BorrarTablero(){
        int aux = CarTabIA.Count;
        for(int i = 0; i < aux; i++){
            GameObject g = CarTabIA[i];
            Destroy(g);
        }
        CarTabIA.Clear();
        for(int i = 0; i < 10; i++){
            if(ManoIA[i] != null){
                GameObject o = ManoIA[i];
                Destroy(o);
            } 
        }
        oro = 40;
        vidaIA = 30;
    }
}

