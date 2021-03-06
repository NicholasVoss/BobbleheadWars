﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 50.0f;
    private CharacterController characterController;
    public Rigidbody head;
    public LayerMask layerMask;
    private Vector3 currentLookTarget = Vector3.zero;
    public Animator bodyAnimator;
    public float[] hitForce;

    //vars for taking damage
    public float timeBetweenHits = 2.5f;
    private bool isHit = false;
    private float timeSinceHit = 0;
    private int hitNumber = -1;

    //vars for killing marine
    public Rigidbody marineBody;
    private bool isDead = false;

    //var for particles
    private DeathParticles deathParticles;

    // Start is called before the first frame update
    void Start()
    {
        //get the players character controller
        characterController = GetComponent<CharacterController>();

        //find death particles
        deathParticles = gameObject.GetComponentInChildren<DeathParticles>();
    }

    // Update is called once per frame
    void Update()
    {
        //move player character based on player input
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        characterController.SimpleMove(moveDirection * moveSpeed);

        //reset invincibility after taking damage
        if(isHit)
        {
            timeSinceHit += Time.deltaTime;
            if(timeSinceHit > timeBetweenHits)
            {
                isHit = false;
                timeSinceHit = 0;
            }
        }
    }

    void FixedUpdate()
    {
        //make head bobble
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"),
            0, Input.GetAxis("Vertical"));
        if (moveDirection == Vector3.zero)
        {
            bodyAnimator.SetBool("IsMoving", false);
        }
        else
        {
            head.AddForce(transform.right * 150, ForceMode.Acceleration);
            bodyAnimator.SetBool("IsMoving", true);
        }

        //draw ray from camera to mouse
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        UnityEngine.Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);

        //find where the mouse is pointing
        if(Physics.Raycast(ray, out hit, 1000, layerMask,
            QueryTriggerInteraction.Ignore))
        {
            if (hit.point != currentLookTarget)
            {
                currentLookTarget = hit.point;
            }
        }

        //set the target position to the place the mouse is pointing
        Vector3 targetPosition = new Vector3(hit.point.x,
            transform.position.y, hit.point.z);
        //turn player towards mouse
        Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation,
            rotation, Time.deltaTime * 10.0f);
    }

    void OnTriggerEnter(Collider other)
    {
        //check if player collided with enemies
        Alien alien = other.gameObject.GetComponent<Alien>();
        if(alien != null)
        {
            if(!isHit)
            {
                //damage player
                hitNumber += 1;
                CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
                //check if player will survive the damage
                if(hitNumber < hitForce.Length) 
                {
                    //shake camera
                    cameraShake.intensity = hitForce[hitNumber];
                    cameraShake.Shake();
                }
                else //kill player
                {
                    Die();
                }
                isHit = true;
                //play hit sound effect
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.hurt);
            }
            alien.Die();
        }
    }

    public void Die()
    {
        //make body fall to the floor
        bodyAnimator.SetBool("IsMoving", false);
        marineBody.transform.parent = null;
        marineBody.isKinematic = false;
        marineBody.useGravity = true;
        marineBody.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        marineBody.gameObject.GetComponent<Gun>().CancelInvoke("fireBullet");
        marineBody.gameObject.GetComponent<Gun>().enabled = false;

        //make head fall to the floor and make blood splatter
        Destroy(head.gameObject.GetComponent<HingeJoint>());
        head.transform.parent = null;
        head.useGravity = true;
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.marineDeath);
        deathParticles.Activate();
        Destroy(gameObject);
    }
}
