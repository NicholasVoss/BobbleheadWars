using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //variable to hold the space marine object
    public GameObject player;

    //variables used for spawning enemies
    public GameObject[] spawnPoints;
    public GameObject alien;
    public int maxAliensOnScreen;
    public int totalAliens;
    public float minSpawnTime;
    public float maxSpawnTime;
    public int aliensPerSpawn;
    private int aliensOnScreen = 0;
    private float generatedSpawnTime = 0;
    private float currentSpawnTime = 0;

    //vars for spawning powerups
    public GameObject upgradePrefab;
    public Gun gun;
    public float upgradeMaxTimeSpawn = 7.5f;
    private bool spawnedUpgrade = false;
    private float actualUpgradeTime = 0;
    private float currentUpgradeTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        actualUpgradeTime = UnityEngine.Random.Range(upgradeMaxTimeSpawn - 3.0f, upgradeMaxTimeSpawn);
        actualUpgradeTime = Mathf.Abs(actualUpgradeTime);
    }

    // Update is called once per frame
    void Update()
    {
        //check if enough time has passed to spawn upgrade
        currentUpgradeTime += Time.deltaTime;
        if(currentUpgradeTime > actualUpgradeTime)
        {
            //if there isnt a powerup spawned, spawn one
            if(!spawnedUpgrade)
            {
                //select spawn point
                int randomNumber = UnityEngine.Random.Range(0, spawnPoints.Length - 1);
                GameObject spawnLocation = spawnPoints[randomNumber];

                //spawn powerup
                GameObject upgrade = Instantiate(upgradePrefab) as GameObject;
                Upgrade upgradeScript = upgrade.GetComponent<Upgrade>();
                upgradeScript.gun = gun;
                upgrade.transform.position = spawnLocation.transform.position;
                spawnedUpgrade = true;

                //play sound effect
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.powerUpAppear);
            }
        }

        //find if enough time has passed to spawn an enemy
        currentSpawnTime += Time.deltaTime;
        if (currentSpawnTime > generatedSpawnTime)
        {
            currentSpawnTime = 0;
            generatedSpawnTime = UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);

            //check if more enemies can be added without going over the max limit
            if (aliensPerSpawn > 0 && aliensOnScreen < totalAliens)
            {
                List<int> previousSpawnLocations = new List<int>();

                //if spawning the group of aliens will go over the limit, decrease the number of enemies being spawned at a time
                if (aliensPerSpawn > spawnPoints.Length)
                {
                    aliensPerSpawn = spawnPoints.Length - 1;
                }

                aliensPerSpawn = (aliensPerSpawn > totalAliens) ?
                    aliensPerSpawn - totalAliens : aliensPerSpawn;

                //spawns aliens at a random spawn point
                for (int i = 0; i < aliensPerSpawn; i++)
                {
                    //check if more enemies can be added
                    if (aliensOnScreen < maxAliensOnScreen)
                    {
                        //increments variables to represent alien that will soon be added
                        aliensOnScreen += 1;
                        int spawnPoint = -1;
                        //pick a random spawn point to spawn alien at
                        while (spawnPoint == -1)
                        {
                            int randomNumber = UnityEngine.Random.Range(0, spawnPoints.Length - 1);
                            if (!previousSpawnLocations.Contains(randomNumber))
                            {
                                previousSpawnLocations.Add(randomNumber);
                                spawnPoint = randomNumber;
                            }
                        }
                        //spawn alien at spawn point
                        GameObject spawnLocation = spawnPoints[spawnPoint];
                        GameObject newAlien = Instantiate(alien) as GameObject;
                        newAlien.transform.position = spawnLocation.transform.position;
                        Alien alienScript = newAlien.GetComponent<Alien>();

                        //makes alien target and turn towards player
                        alienScript.target = player.transform;
                        Vector3 targetRotation = new Vector3(player.transform.position.x,
                            newAlien.transform.position.y, player.transform.position.z);
                        newAlien.transform.LookAt(targetRotation);
                        alienScript.OnDestroy.AddListener(AlienDestroyed);
                    }
                }
            }                       
        }        
    }

    public void AlienDestroyed()
    {
        aliensOnScreen -= 1;
        totalAliens -= 1;
    }
}
