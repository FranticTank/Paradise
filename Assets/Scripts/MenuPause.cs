using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPause : MonoBehaviour
{
    public GameObject panelPause;

    private bool gameRunning = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeGameState();
        }
    }

    private void ChangeGameState()
    {
        gameRunning = !gameRunning;

        if (gameRunning)
        {
            panelPause.SetActive(false);
        }
        else{
            panelPause.SetActive(true);
        }
    }

    public void Salir(){
        panelPause.SetActive(false);
    }

}
