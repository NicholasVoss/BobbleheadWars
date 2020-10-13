using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnBecameInvisible()
    {
        //destroy projectile if it goes off screen
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //destroy projectile when it collides with an object
        Destroy(gameObject);
    }
}
