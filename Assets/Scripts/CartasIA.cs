using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CartasIA : MonoBehaviour
{

    public GameObject CartaPrefab;
    public GameObject cartaArbol;
    public GameObject cartaGuerrero;
    public GameObject cartaEscudero;
    public GameObject zonaMano;

    public GameObject PanelGanar;

    public List<Carta> CarIA = new List<Carta>();
    public List<Carta> CarIAPlanta = new List<Carta>();
    public List<Carta> CarIAHumanos = new List<Carta>();
    public List<Carta> CarIAGoblins = new List<Carta>();
    public List<Carta> CarIACriaturas = new List<Carta>();
    public List<Carta> CarIAAbismo = new List<Carta>();

    public List<GameObject> ManoIA = new List<GameObject>();

    public List<GameObject> CarTabIA = new List<GameObject>();

    public List<GameObject> Estrellas = new List<GameObject>();
    private int nivel;

    public CartasTablero ct;
    public DataBase db;

    public GameObject Icono;
    public TextMeshProUGUI VidaText;
    public TextMeshProUGUI OroText;
    public int oro = 30;
    public int vidaIA = 30;

    private int cartaComun = 1;
    private int cartaEpica = 3;
    private int cartaLegendaria = 5;

    // Start is called before the first frame update
    void Start()
    {
        VidaText.text = vidaIA.ToString();
        OroText.text = oro.ToString();
    }

    public void IA_Planta(Sprite img){
        CarIA = CarIAPlanta;
        Icono.GetComponent<Image>().sprite = img;
        EmpezarPartida();
        nivel = 0;
    }

    public void IA_Humanos(Sprite img){
        CarIA = CarIAHumanos;
        Icono.GetComponent<Image>().sprite = img;
        EmpezarPartida();
        nivel = 1;
    }

    public void IA_Goblins(Sprite img){
        CarIA = CarIAGoblins;
        Icono.GetComponent<Image>().sprite = img;
        EmpezarPartida();
        nivel = 2;
    }

    public void IA_Criaturas(Sprite img){
        CarIA = CarIACriaturas;
        Icono.GetComponent<Image>().sprite = img;
        EmpezarPartida();
        nivel = 3;
    }

    public void IA_Abismo(Sprite img){
        CarIA = CarIAAbismo;
        Icono.GetComponent<Image>().sprite = img;
        EmpezarPartida();
        nivel = 4;
    }

    public void TurnoIA(){
        //sacar las cartas al tablero
        //de las cartas puestas hacer las funciones
        //atacar i acabar turno
        ct.botonAcabar.GetComponent<Button>().interactable = false;

        if(ManoIA.Count <= 9){
            ObtenerCarta();
            ObtenerCarta();
        }

        for (int i = 0; i < ManoIA.Count; i++){
            if ((oro - ManoIA[i].GetComponent<AsignarCartaMano>().carta.oro) > 0){
                Debug.Log(ManoIA[i].GetComponent<AsignarCartaMano>().carta.nombre);
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
        
        OroText.text = oro.ToString();
        ct.botonAcabar.GetComponent<Button>().interactable = true;
        ct.EmpezarTurno();
    }

    private void EmpezarPartida(){
        //si se da mas de una vez a jugar lo hace mas veces.
        for (int i = 0; i <= 7; i++){
            ObtenerCarta();
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
            VidaText.text = vidaIA.ToString();
            if(vidaIA < 1){
                Estrellas[nivel].SetActive(true);
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
                Debug.Log(g.GetComponent<AsignarCartaMano>().carta.nombre);
                Debug.Log(CarTabIA.Count);
                CarTabIA.Remove(g);
                Destroy(g);
                Debug.Log(CarTabIA.Count);
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

    private void ObtenerCarta(){
        //hay que cambiar para que no se vean las cartas de la ia i solo cuando las pone en el tablero
        int x = Random.Range(0, CarIA.Count);
        GameObject c = Instantiate(CartaPrefab, Vector2.zero, Quaternion.identity);
        c.transform.SetParent(zonaMano.transform);
        c.transform.localScale = Vector3.one;
        ManoIA.Add(c);
        ManoIA[ManoIA.Count - 1].GetComponent<AsignarCartaMano>().Asignar(CarIA[x]);
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
                if(ManoIA.Count < 10){
                    ObtenerCarta();
                }      
                break;
            case "Kazulu":
                if(ManoIA.Count < 10){
                    ObtenerCarta();
                }      
                break;
            case "Hechicero Goblin":
                AumentaStats("Goblin", 1, 0);
                break;
            case "Mercader":
                if(ManoIA.Count < 10){
                    ObtenerCarta();
                }  
                break;
            case "Bosque Sagrado":
                SpawnObjeto(cartaArbol);
                SpawnObjeto(cartaArbol);
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
            case "Gobernador":
                SpawnObjeto(cartaGuerrero);
                SpawnObjeto(cartaEscudero);
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

    private void SpawnObjeto(GameObject g){
        GameObject a1 = Instantiate(g, Vector2.zero, Quaternion.identity);
        a1.transform.SetParent(this.transform);
        a1.transform.localScale = Vector3.one;
        CarTabIA.Add(a1);
    }

    private void AtacarIA(){
        int dif = 0;
        for (int i = 0; i < CarTabIA.Count; i++){
            Debug.Log("Index: " + i + " Tamaño: " + CarTabIA.Count);
            dif = CarTabIA.Count - ct.mazo.Count;
            if(CarTabIA[i].GetComponent<AsignarCartaMano>().carta.nombre == "Recolector Oro"){
                oro += 3;
            }
            else if(CarTabIA[i].GetComponent<AsignarCartaMano>().carta.nombre == "Fabrica Oro"){
                oro += 5;
            }
            else{//Llamar minMax

                if(ct.mazo.Count < 1) ct.Defender(CarTabIA[i]);
                else{

                    GameState currentState = new GameState
                    {
                        Jugador1 = new List<GameObject>(),
                        Jugador2 = new List<GameObject>(),
                        oro1 = oro,
                        oro2 = ct.mj.oro
                    };
                    for(int j = 0; j < CarTabIA.Count; j++){
                        currentState.Jugador1.Add(CarTabIA[j]);
                    }
                    for(int j = 0; j < ct.mazo.Count; j++){
                        currentState.Jugador2.Add(ct.mazo[j]);
                    }

                    GameObject aux = CarTabIA[i];
                    (int opcion, GameObject targetCard) = SelectBestMove(currentState, 2, aux);
                    Debug.Log("SelectBestMove: " + opcion + " " + targetCard.GetComponent<AsignarCartaMano>().carta.nombre);

                    if(opcion == 1) ct.Defender(CarTabIA[i]);
                    else if(opcion == 2) oro += CarTabIA[i].GetComponent<AsignarCartaMano>().aMomentaneo;
                }
            }
   
        }
        
    }

    public void BorrarTablero(){
        int aux = CarTabIA.Count;
        for(int i = 0; i < aux; i++){
            GameObject g = CarTabIA[i];
            Destroy(g);
        }
        aux = ManoIA.Count;
        for(int i = 0; i < aux; i++){
            GameObject g = ManoIA[i];
            Destroy(g);
        }
        CarTabIA.Clear();
        ManoIA.Clear();
        oro = 30;
        vidaIA = 30;
    }


    //Minimax

    public class GameState{
        public List<GameObject> Jugador1 { get; set; }//IA
        public List<GameObject> Jugador2 { get; set; }//Rival
        public int oro1 { get; set; }
        public int oro2 { get; set; }
    }

    static int EvaluarEstado(GameState state)
    {
        //La suma de las defensas, el ataque i el oro que tenga cada uno, ademas de la diferencia de cartas
        int resultado = 0;
        resultado = state.oro1 - state.oro2;
        for(int i = 0; i < state.Jugador1.Count; i++){
            resultado += state.Jugador1[i].GetComponent<AsignarCartaMano>().aMomentaneo + state.Jugador1[i].GetComponent<AsignarCartaMano>().dMomentaneo + state.Jugador1[i].GetComponent<AsignarCartaMano>().carta.oro;
        }
        for(int i = 0; i < state.Jugador2.Count; i++){
            resultado -= state.Jugador2[i].GetComponent<AsignarCartaMano>().aMomentaneo - state.Jugador2[i].GetComponent<AsignarCartaMano>().dMomentaneo - state.Jugador2[i].GetComponent<AsignarCartaMano>().carta.oro;
        }
        int diferencia = (state.Jugador1.Count - state.Jugador2.Count) * 2;
        resultado += diferencia;
        return resultado;
    }

    private int Minimax(GameState state, GameObject cardSelect, int depth, bool turnoIA){
        //Devuleve si no atacar, recolectar oro o atacar al rival. (0,1,2), tambien que carta ataca
        if (depth == 0){//cardSelect.GetComponent<AsignarCartaMano>().dMomentaneo <= 0  || state.Jugador1.Count < 1 || state.Jugador2.Count < 1
            return EvaluarEstado(state);
        }
        if (turnoIA){
            int bestScore = int.MinValue;
            for (int i = 0; i < state.Jugador2.Count; i++){
                //Ataca
                GameState nextState = GenerateNextState(state, cardSelect, state.Jugador2[i], true, false);
                int score = Minimax(nextState, cardSelect, depth - 1, false);
                if(score > bestScore) bestScore = score;
                //No ataca
                GameState noAttackState = GenerateNextState(state, cardSelect, null, true, false);
                score = Minimax(noAttackState, cardSelect, depth - 1, false);
                if(score > bestScore) bestScore = score;
                //Roba oro
                GameState goldState = GenerateNextState(state, cardSelect, null, true, true);
                score = Minimax(goldState, cardSelect, depth - 1, false);
                if(score > bestScore) bestScore = score;
            }
            return bestScore;
        }
        else{
            int bestScore = int.MaxValue;
            for (int i = 0; i < state.Jugador2.Count; i++){
                for (int j = 0; j < state.Jugador1.Count; j++){
                    GameState nextState = GenerateNextState(state, state.Jugador2[i], state.Jugador1[j], false, false);
                    int score = Minimax(nextState, cardSelect, depth - 1, true);
                    if(score < bestScore) bestScore = score;
                }
                GameState noAttackState = GenerateNextState(state, state.Jugador1[i], null, false, false);
                int scoreNoAttack = Minimax(noAttackState, cardSelect, depth - 1, true);
                if(scoreNoAttack < bestScore) bestScore = scoreNoAttack;

                GameState goldState = GenerateNextState(state, cardSelect, null, false, true);
                int scoreGold = Minimax(goldState, cardSelect, depth - 1, true);
                if(scoreGold < bestScore) bestScore = scoreGold;
            }
            return bestScore;
        }
    }
    
    public (int, GameObject) SelectBestMove(GameState state, int depth, GameObject cardSelect){
        int maxEval = int.MinValue;
        //attackingCard = null;
        int opcion = 0;
        GameObject targetCard = null;

        // Considerar todas las posibles acciones: atacar, no atacar y Robar oro
        for (int i = 0; i < state.Jugador2.Count; i++)
        {
            // Accion: Atacar   
            GameState attackState = GenerateNextState(state, cardSelect, state.Jugador2[i], true, false);
            int val1 = Minimax(attackState, cardSelect, depth, false);

            if (val1 > maxEval)
            {
                maxEval = val1;
                //attackingCard = card;
                opcion = 1;
                targetCard = state.Jugador2[i];
            }

            // Accion: No atacar
            GameState noAttackState = GenerateNextState(state, cardSelect, null, true, false);
            int val2 = Minimax(noAttackState, cardSelect, depth, false);

            if (val2 > maxEval)
            {
                maxEval = val2;
                //attackingCard = card;
                opcion = 0;
                targetCard = null;
            }

            //Robar oro
            GameState playCardState = GenerateNextState(state, cardSelect, null, true, true);
            int val3 = Minimax(playCardState, cardSelect, depth, false);

            if (val3 > maxEval)
            {
                maxEval = val3;
                //attackingCard = card;
                opcion = 2;
                targetCard = null;
            }
        }

        return (opcion, targetCard);
    }

    public GameState GenerateNextState(GameState currentState, GameObject cardSelect, GameObject targetCard, bool isPlayerTurn, bool robarOro){

        GameState nextState = new GameState
        {
            Jugador1 = new List<GameObject>(currentState.Jugador1),
            Jugador2 = new List<GameObject>(currentState.Jugador2), 
            oro1 = currentState.oro1,
            oro2 = currentState.oro2
        };


        if (targetCard != null)
        {
            if (isPlayerTurn)
            {
                // Actualizar la carta atacante
                targetCard.GetComponent<AsignarCartaMano>().dMomentaneo -= cardSelect.GetComponent<AsignarCartaMano>().aMomentaneo;

                // Si la carta atacante queda con 0 o menos de salud, se retira del tablero
                if (targetCard.GetComponent<AsignarCartaMano>().dMomentaneo <= 0)
                {
                    nextState.Jugador2.Remove(targetCard);
                }
            }
            else
            {
                // Actualizar la carta atacante
                targetCard.GetComponent<AsignarCartaMano>().dMomentaneo -= cardSelect.GetComponent<AsignarCartaMano>().aMomentaneo;

                // Si la carta atacante queda con 0 o menos de salud, se retira del tablero
                if (targetCard.GetComponent<AsignarCartaMano>().dMomentaneo <= 0)
                {
                    nextState.Jugador1.Remove(targetCard);
                }
            }
        }

        if (robarOro){

            if (isPlayerTurn)
            {
                nextState.oro1 += cardSelect.GetComponent<AsignarCartaMano>().aMomentaneo; 
            }
            else
            {
                //nextState.oro2 += targetCard.GetComponent<AsignarCartaMano>().aMomentaneo;
                nextState.oro2 += 3;
            }
        }

        return nextState;
    }

}

