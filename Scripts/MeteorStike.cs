using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MeteorStike : MonoBehaviour {

    public List<GameObject> meteorSpawners = new List<GameObject>();//holds all potential spawners

    public float shotTimer = 1f;//timer between meteors shot from spawners
    public int spawnScore = 0;//score used to only shot 12 meteors pure activation
    public bool iconPickedUp = false;//placeholder to start meteor Strke powerup

    private AudioSimple meteorStrikeSound;

    void Awake()
    {
        meteorStrikeSound = AudioManager.Instance.FindAudioClip(AudioManager.SoundName.MeteorStrikeAppear);
    }

    public void Update()
    {
        if (iconPickedUp)
        {
            if(spawnScore <= 11)
            {
                if (shotTimer <= 0)
                {
                    StartMeteorStrike();
                    shotTimer = 1;
                }
                else
                {
                    shotTimer -= Time.deltaTime;
                }
            }
            else
            {
                iconPickedUp = false;
                spawnScore = 0;
            }
        }
    }

    public void StartMeteorStrike()
    {
        //this method called when player picks up Meteor Strike powerup icon, starts meteor strike

        //takes all spawners in metorSpawner, random generator to decide which get chosen to fire
        //upon each firing, resets timer to 1 second
        //once 12 meteors have been shot, meteor strike ends
        meteorStrikeSound.PlayAudioClip();
        iconPickedUp = true;
        spawnScore++;
        shotTimer = 1;
        
                int spawnerPicked;

                spawnerPicked = UnityEngine.Random.Range(0, meteorSpawners.Count);
                meteorSpawners[spawnerPicked].GetComponent<MeteorStrikeSpawner>().FireMeteor();
        
    }
}
