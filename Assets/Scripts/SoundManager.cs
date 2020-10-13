using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //vars for each sound effect
    public AudioClip gunFire;
    public AudioClip upgradedGunFire;
    public AudioClip hurt;
    public AudioClip alienDeath;
    public AudioClip marineDeath;
    public AudioClip vicctory;
    public AudioClip elevatorArrived;
    public AudioClip powerUpPickup;
    public AudioClip powerUpAppear;

    //var that holds SoundManager so it can be accessed in other scripts
    public static SoundManager Instance = null;
    //audio source that will play sound effects
    private AudioSource soundEffectAudio;

    // Start is called before the first frame update
    void Start()
    {
        //check if there is an existing sound manager, if there isn't, set the instance to this sound manager, 
        //otherwise destroy this sound manager to make sure there is only one
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //looks through the AudioSource components and finds an empty one to use for sound effects
        AudioSource[] sources = GetComponents<AudioSource>();
        foreach(AudioSource source in sources)
        {
            if(source.clip == null)
            {
                soundEffectAudio = source;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayOneShot(AudioClip clip)
    {
        //play sound effect once
        soundEffectAudio.PlayOneShot(clip);
    }
}
