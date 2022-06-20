using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using PlayFab;
using PlayFab.ClientModels;
using System.Threading;

public class TouchingObjectsCubos : MonoBehaviour, IDataPersistence
{
    public AudioSource collisionSound;
    public AudioSource collisionError;
    public ParticleSystem particleAnimation;
    public GameObject colisionador;
    public GameObject cuboCogido;
    private int counterCorrect = 0;
    private int counterMistake = 0;
    public GameObject mesa;
    public GameObject suelo;
    private Vector3 validDirection = Vector3.up;  // What you consider to be upwards
    private float contactThreshold = 30;          // Acceptable difference in degrees
    private string nombreJuego = "Cubos";
    private bool correcto;
    private bool error;
    private string lastSaved = null;

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
    private void OnCollisionEnter (Collision collision1)
    {
        float distance = Vector3.Distance(colisionador.transform.position, cuboCogido.gameObject.transform.position);
        float distance2 = Vector3.Distance(cuboCogido.transform.position, collision1.gameObject.transform.position);
        if (collision1.gameObject == colisionador)
        {
            for (int k = 0; k < collision1.contacts.Length; k++)
            {
                if (Vector3.Angle(collision1.contacts[k].normal, validDirection) <= contactThreshold && distance < 0.15)
                {
                    if (collision1.gameObject.name != lastSaved)
                    {
                        correcto = true;
                        lastSaved = collision1.gameObject.name;
                    }
                }
            }
        }


        else if (collision1.gameObject != colisionador && collision1.gameObject != mesa && collision1.gameObject != suelo && distance2 < 0.15)
        {

            for (int k = 0; k < collision1.contacts.Length; k++)
            {
                    if (Vector3.Angle(collision1.contacts[k].normal, validDirection) <= contactThreshold)
                    {
                        if (collision1.gameObject.name != lastSaved )
                        {
                            error = true;
                            lastSaved = collision1.gameObject.name;
                        }

                    }

            }

        }

    }
    private void OnTriggerEnter(Collider col)
    {
        if (correcto)
        {
            var em = particleAnimation.emission;
            em.enabled = true;
            particleAnimation.Play();
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
    }

}