using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Alien : MonoBehaviour
{
    public Transform target;
    public float navigationUpdate;
    public UnityEvent OnDestroy;
    private float navigationTime = 0;
    private NavMeshAgent agent;

    //vars for killing alien
    public Rigidbody head;
    public bool isAlive = true;

    private DeathParticles deathParticles;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            //set destination to target assigned to alien when spawned in GameManager
            if (target != null)
            {
                navigationTime += Time.deltaTime;
                if (navigationTime > navigationUpdate)
                {
                    agent.destination = target.position;
                    navigationTime = 0;
                }
            }
        }
    }

    //destroy alien when shot
    void OnTriggerEnter (Collider other)
    {
        //make sure Die() hasn't been called before
        if (isAlive && other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            Die();
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDeath);
        }
    }

    public void Die()
    {
        //kills alien and makes it fall apart
        isAlive = false;
        head.GetComponent<Animator>().enabled = false;
        head.isKinematic = false;
        head.useGravity = true;
        head.GetComponent<SphereCollider>().enabled = true;
        head.gameObject.transform.parent = null;
        head.velocity = new Vector3(0, 26.0f, 3.0f);

        //removes listeners and destroys object
        OnDestroy.Invoke();
        OnDestroy.RemoveAllListeners();
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDeath);
        head.GetComponent<SelfDestruct>().Initiate();

        if(deathParticles)
        {
            //makes blood splatter when alien dies
            deathParticles.transform.parent = null;
            deathParticles.Activate();
        }
        Destroy(gameObject);
    }

    //initializes death particles
    public DeathParticles GetDeathParticles()
    {
        if(deathParticles == null)
        {
            deathParticles = GetComponentInChildren<DeathParticles>();
        }
        return deathParticles;
    }
}
