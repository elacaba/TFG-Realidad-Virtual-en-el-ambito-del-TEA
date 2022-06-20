using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    public ParticleSystem particleAnimation;
    public GameObject colisionador;
    public GameObject cilindro;
    public GameObject anilloCogido;
    private int contador = 0;
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
        float distance = Vector3.Distance(cilindro.transform.position, anilloCogido.gameObject.transform.position);
        float distance2 = Vector3.Distance(colisionador.transform.position, anilloCogido.gameObject.transform.position);
        if (collision.gameObject == colisionador && distance < 0.35 && distance2 < 0.1 && contador == 0)
        {
                contador = 1;
                var em = particleAnimation.emission;
                em.enabled = true;
                particleAnimation.Play();
            }
        
    }
}
