using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaWall : MonoBehaviour
{
    private Animator arenaAnimator;

    // Start is called before the first frame update
    void Start()
    {
        //retrive the animator for the arena
        GameObject arena = transform.parent.gameObject;
        arenaAnimator = arena.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //when player gets close to wall, lower the wall
    void OnTriggerEnter(Collider other)
    {
        arenaAnimator.SetBool("IsLowered", true);
    }

    //when player moves away from wall, raise the wall
    void OnTriggerExit(Collider other)
    {
        arenaAnimator.SetBool("IsLowered", false);
    }
}
