using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public Gun gun;

    void OnTriggerEnter(Collider other)
    {
        //applies powerup when player collides with it
        gun.UpgradeGun();
        Destroy(gameObject);
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.powerUpPickup);
    }
}
