using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    public GameObject player;
    public Transform elevator;
    private Animator arenaAnimator;
    private SphereCollider sphereCollider;

    // Start is called before the first frame update
    void Start()
    {
        //gets components from the arena
        arenaAnimator = GetComponent<Animator>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //activated when player touches collider
    void OnTriggerEnter(Collider other)
    {
        //disables camera movement
        Camera.main.transform.parent.gameObject.GetComponent<CameraMovement>().enabled = false;
        //sets player as a child as elevator
        player.transform.parent = elevator.transform;
        //disables marine movement
        player.GetComponent<PlayerController>().enabled = false;
        //play sound to tell player that elevator has arrived
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.elevatorArrived);
        //start elevator animation
        arenaAnimator.SetBool("OnElevator", true);
    }

    public void ActivatePlatform()
    {
        //start checking if player is on the elevator
        sphereCollider.enabled = true;
    }
}
