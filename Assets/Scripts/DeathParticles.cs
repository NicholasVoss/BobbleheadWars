using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathParticles : MonoBehaviour
{
    private ParticleSystem deathParticles;
    private bool didStart = false;
    // Start is called before the first frame update
    void Start()
    {
        deathParticles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //delete particles after they are done playing
        if(didStart && deathParticles.isStopped)
        {
            Destroy(gameObject);
        }
    }

    public void Activate()
    {
        //activate particles
        didStart = true;
        deathParticles.Play();
    }

    //sets death floor and gets particle system if it hasnt already
    public void SetDeathFloor (GameObject deathFloor)
    {
        if(deathParticles == null)
        {
            deathParticles = GetComponent<ParticleSystem>();
        }
        deathParticles.collision.SetPlane(0, deathFloor.transform);
    }
}
