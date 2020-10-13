using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //vars for launcing bullets
    public GameObject bulletPrefab;
    public Transform launchPosition;

    //vars for powerups
    public bool isUpgraded;
    public float upgradeTime = 10.0f;
    private float currentTime;

    //var for sound effect
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //shoots when mouse is clicked
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsInvoking("fireBullet"))
            {
                InvokeRepeating("fireBullet", 0f, 0.1f);
            }
        }
        //stop shooting if mouse isn't pressed
        if (Input.GetMouseButtonUp(0))
        {
            CancelInvoke("fireBullet");
        }

        //disable powerup if a certain time has passed
        currentTime += Time.deltaTime;
        if(currentTime > upgradeTime && isUpgraded == true)
        {
            isUpgraded = false;
        }
    }

    void fireBullet()
    {
        //fire bullet
        Rigidbody bullet = createBullet();
        bullet.velocity = transform.parent.forward * 100;

        //shoot 3 bullets at once if upgraded
        if (isUpgraded)
        {
            Rigidbody bullet2 = createBullet();
            bullet2.velocity = (transform.right + transform.forward / 0.5f) * 100;
            Rigidbody bullet3 = createBullet();
            bullet3.velocity = ((transform.right * -1) + transform.forward / 0.5f) * 100;
        }

        //play sound effects
        if (isUpgraded)
        {
            audioSource.PlayOneShot(SoundManager.Instance.upgradedGunFire);
        }
        else
        {
            audioSource.PlayOneShot(SoundManager.Instance.gunFire);
        }
    }

    private Rigidbody createBullet()
    {
        //create a bullet
        GameObject bullet = Instantiate(bulletPrefab) as GameObject;
        bullet.transform.position = launchPosition.position;
        return bullet.GetComponent<Rigidbody>();

    }

    public void UpgradeGun()
    {
        //upgrade gun to shoot 3 bullets at once
        isUpgraded = true;
        currentTime = 0;
    }
}
