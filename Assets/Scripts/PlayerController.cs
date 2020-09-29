using System.Collections;
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
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //move player character based on player input
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"),
            0, Input.GetAxis("Vertical"));
        characterController.SimpleMove(moveDirection * moveSpeed);

    }

    void FixedUpdate()
    {
        //make head bobble
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"),
            0, Input.GetAxis("Vertical"));
        if (moveDirection == Vector3.zero)
        {
            // TODO
        }
        else
        {
            head.AddForce(transform.right * 150, ForceMode.Acceleration);
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
}
