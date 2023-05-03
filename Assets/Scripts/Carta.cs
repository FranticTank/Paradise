using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nueva Carta", menuName = "Carta")]
public class Carta : ScriptableObject
{

    public string nombre;
    public string descripcion;
    public int atk;
    public int def;
    public int oro;
    public int num;
    public Sprite img;
    public bool hechizo;

    public enum Tipo
    {
        Humano, Goblin, Criatura, Natura, Abismo, Nada
    }
    public Tipo tipoCarta;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
