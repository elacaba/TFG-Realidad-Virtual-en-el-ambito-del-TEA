using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using PlayFab;
using PlayFab.ClientModels;
using System.Threading;

public class TouchingObjects : MonoBehaviour, IDataPersistence
{
    public AudioSource collisionSound;
    public AudioSource collisionError;
    public GameObject colisionador;
    private static System.Timers.Timer timer;
    public GameObject cilindro;
    public GameObject anilloCogido;
    private int counterCorrect = 0;
    private int counterMistake = 0;
    private int contador = 0;
    private string nombreColisionador = null;
    public BoxCollider superficieCollision;
    private Vector3 validDirection = Vector3.up;  // What you consider to be upwards
    private float contactThreshold = 30;          // Acceptable difference in degrees
    private string nombreJuego = "Aros";
    private bool correcto;
    private bool error;

    // Start is called before the first frame update
    void Start()
    {
        counterCorrect = 0;
        counterMistake = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadData(GameData data)
    {
        this.counterMistake = data.respuestasIncorrectas;
        this.counterCorrect = data.respuestasCorrectas;
    }

    public void SaveData(ref GameData data)
    {
        data.respuestasCorrectas = data.respuestasCorrectas + this.counterCorrect;
        data.respuestasIncorrectas = data.respuestasIncorrectas + this.counterMistake;
        data.NombreJuego = this.nombreJuego;
    }

    private void OnCollisionEnter(Collision collision1)
    {
        float distance = Vector3.Distance(cilindro.transform.position, anilloCogido.gameObject.transform.position);
        float distance2 = Vector3.Distance(colisionador.transform.position, anilloCogido.gameObject.transform.position);
        float distance3 = Vector3.Distance(collision1.gameObject.transform.position, anilloCogido.gameObject.transform.position);
        if (collision1.gameObject == colisionador && distance < 0.35 && distance2 < 0.1 && nombreColisionador != colisionador.name)
        {
            for (int k = 0; k < collision1.contacts.Length; k++)
            {
                if (Vector3.Angle(collision1.contacts[k].normal, validDirection) <= contactThreshold)
                {
                    correcto = true;
                }
            }
        }
        else if (collision1.gameObject != colisionador && collision1.gameObject != cilindro && distance < 0.35 && distance3 < 0.1 && contador == 0 && collision1.gameObject.name != "Floor_Normal_02" && collision1.gameObject.name != nombreColisionador)
        {
            for (int k = 0; k < collision1.contacts.Length; k++)
            {
                if (Vector3.Angle(collision1.contacts[k].normal, validDirection) <= contactThreshold)
                {
                    error = true;
                }
            }
        }

    }
    private void OnTriggerEnter(Collider col)
    {
        if (correcto)
        {
            collisionSound.Play();
            OnPlayerCorrect();
            correcto = false;
        }
        if (error)
        {
            collisionError.Play();
            OnPlayerError();
            error = false;
        }

    }

    private void OnPlayerCorrect()
    {
        this.counterCorrect++;
    }

    private void OnPlayerError()
    {
        this.counterMistake++;
        contador = 0;
    }
    void delaySeconds()
    {
        timer = new System.Timers.Timer(10000);
        timer.Start();
    }

}