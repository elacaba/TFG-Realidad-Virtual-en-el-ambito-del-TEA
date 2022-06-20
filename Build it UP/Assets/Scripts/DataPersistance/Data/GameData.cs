using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{
    // Start is called before the first frame update
    public int respuestasCorrectas;
    public int respuestasIncorrectas;
    public string entityID;
    public string entityType;
    public string emailMonitor;
    public string NombreJuego;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameData()
    {
        this.respuestasCorrectas = 0;
        this.respuestasIncorrectas = 0;
        this.entityType = null;
        this.entityID = null;
        this.emailMonitor = null;
        this.NombreJuego = null;
    }
}
