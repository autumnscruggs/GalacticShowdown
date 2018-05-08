using UnityEngine;
using System.Collections;
using System;

public class ShieldHealth : MonoBehaviour {

    public int healthPoints = 2;//how many hits it takes to deactivate shield

    public float respawnTimer;
    public float timerMax = 4;//seconds needed until shield can respawn
    bool isRecharging = false;

    public Material defaultShield;//blue/healthy shield
    public Material damagedShield;//slighlty faded tinted with yellow and visible cracks
    public Material inactiveShield;//invisible

    private AudioSimple shieldImpactSound;
    private AudioSimple shieldDestructionSound;

    // Use this for initialization
    void Start()
    {
        this.gameObject.SetActive(true);
        healthPoints = 2;

        shieldImpactSound = AudioManager.Instance.FindAudioClip(AudioManager.SoundName.BlueShieldImpact);
        shieldDestructionSound = AudioManager.Instance.FindAudioClip(AudioManager.SoundName.BlueShieldDestruction);
    }

    // Update is called once per frame
    void Update()
    {
        //print("Health: " + healthPoints);
        //print("Timer: " + respawnTimer);

        CheckHealthStatus();

        if (isRecharging)
        {
            //isRecharging = false;

            if (respawnTimer < timerMax)
            {
                respawnTimer += Time.deltaTime;
            }
            else
            {
                healthPoints = 2;
                respawnTimer = 0;
                isRecharging = false;
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        //If hit by bullet
        if (collision.gameObject.GetComponent<Bullet>())
        {
            healthPoints--;
        }
        else if (collision.gameObject.GetComponent<Missle>())
        {
            healthPoints = 0;
        }

        if(healthPoints > 0 && (collision.gameObject.GetComponent<Bullet>() || collision.gameObject.GetComponent<Missle>()))
        {
            shieldImpactSound.PlayAudioClip();
        }
        else if((collision.gameObject.GetComponent<Bullet>() || collision.gameObject.GetComponent<Missle>()))
        {
            shieldDestructionSound.PlayAudioClip();
        }
    }

    private void CheckHealthStatus()
    {
        switch (healthPoints)//health status
        {
            case 2:
                this.GetComponent<Renderer>().material = defaultShield;
                this.GetComponent<BoxCollider>().enabled = true;
                break;

            case 1:
                this.GetComponent<Renderer>().material = damagedShield;
                this.GetComponent<BoxCollider>().enabled = true;
                break;

            case 0:
                gameObject.GetComponent<Renderer>().material = inactiveShield;
                this.GetComponent<BoxCollider>().enabled = false;
                isRecharging = true;
                break;
        }
    }

}
