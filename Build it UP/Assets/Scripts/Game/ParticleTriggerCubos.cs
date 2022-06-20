using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTriggerCubos : MonoBehaviour
{
    public ParticleSystem particleAnimation;
    public GameObject colisionador;
    public GameObject cuboCogido;
    private Vector3 validDirection = Vector3.up;  // What you consider to be upwards
    private float contactThreshold = 30;          // Acceptable difference in degrees
    public bool correcto = false;
    private string lastSaved = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        float distance = Vector3.Distance(colisionador.transform.position, cuboCogido.gameObject.transform.position);
        if (collision.gameObject == colisionador)
        {
            for (int k = 0; k < collision.contacts.Length; k++)
            {
                if (Vector3.Angle(collision.contacts[k].normal, validDirection) <= contactThreshold && distance < 0.15)
                {
                    if (collision.gameObject.name != lastSaved)
                    {
                        correcto = true;
                    }
                }

            }
        }
        lastSaved = collision.gameObject.name;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (correcto)
        {
            var em = particleAnimation.emission;
            em.enabled = true;
            particleAnimation.Play();
            correcto = false;
        }

    }
}
