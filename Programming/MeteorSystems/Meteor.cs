using UnityEngine;
using System.Collections;

public class Meteor : MonoBehaviour
{
    private Vector3 moveTranslation;
    [HideInInspector]
    public Vector2 direction;
    [HideInInspector]
    public float speed = 5;

    private float destructionTimer;
    private float destructionTime;

    public enum PowerupState { NONE, ELECTRIC_MISSLE_LAUNCHER, AUTO_FIRE_BLASTER, BUBBLE_SHIELD, LASER_BEAM_ASSAULT, METEOR_SHOWER }
    public PowerupState powerupState;

    private Player.PlayerIdentity shooterIdentity;
    private PowerupManager powerupManager;

    protected AudioSimple meteorSound;

    void Awake()
    {
        powerupManager = GameObject.FindObjectOfType<PowerupManager>();
        destructionTime = 20;
        destructionTimer = destructionTime;
    }

    void Start()
    {
        meteorSound = AudioManager.Instance.FindAudioClip(AudioManager.SoundName.MeteorExplosion);
    }

    public void Update()
    {
        UpdateMeteor();
        MeteorLife();
    }

    protected void UpdateMeteor()
    {
        moveTranslation = new Vector3(direction.x, direction.y) * Time.deltaTime * speed;
        this.transform.position += new Vector3(moveTranslation.x, moveTranslation.y);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            collision.gameObject.GetComponent<Player>().StartRespawnSequence();
            meteorSound.PlayAudioClip();
        }

        this.gameObject.SetActive(false);
    }

    protected virtual void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<Bullet>() && powerupState != PowerupState.NONE)
        {
            shooterIdentity = collider.GetComponent<Bullet>().manager.shooterIdentity;
            DropPowerupOnDeath();
            this.gameObject.SetActive(false);
            meteorSound.PlayAudioClip();
        }
        else if (collider.GetComponent<Bullet>() && powerupState == PowerupState.NONE)
        {
            shooterIdentity = collider.GetComponent<Bullet>().manager.shooterIdentity;
            this.gameObject.SetActive(false);
            meteorSound.PlayAudioClip();
        }
        if (collider.GetComponent<Missle>())
        {
            shooterIdentity = collider.GetComponent<Missle>().missleLauncher.player.GetComponent<Player>().identifier;
            DropPowerupOnDeath();
            meteorSound.PlayAudioClip();
        }
    }

    //kills bullet over time -- prevents bullet from clogging up space if it doesn't hit anything for a while
    private void MeteorLife()
    {
        destructionTimer -= Time.deltaTime;
        if (destructionTimer < 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void DropPowerupOnDeath()
    {
        SpawnPowerupOnField(powerupState);
    }

    private void SpawnPowerupOnField(PowerupState state)
    {
        Powerup pU;
        EnvironmentalTrigger eT;

        if (state == PowerupState.ELECTRIC_MISSLE_LAUNCHER || state == PowerupState.AUTO_FIRE_BLASTER || state == PowerupState.BUBBLE_SHIELD)
        {
            pU = powerupManager.SpawnPowerup();
            pU.GrabPlayerWhoEarnedIt(shooterIdentity);
            eT = null;
        }
        else
        {
            pU = null;
            eT = powerupManager.SpawnTrigger();
            eT.GrabPlayerWhoEarnedIt(shooterIdentity);
        }

        switch (state)
        {
            case PowerupState.NONE:
                //do nothing
                break;
            case PowerupState.ELECTRIC_MISSLE_LAUNCHER:
                //spawn electic missle launcher
                pU.SetUpPowerup(PowerupManager.PowerupTypes.ELECTIC_MISSLE_LAUNCHER);
                break;
            case PowerupState.AUTO_FIRE_BLASTER:
                //spawn auto fire blaster
                pU.SetUpPowerup(PowerupManager.PowerupTypes.AUTO_FIRE_BLASTER);
                break;
            case PowerupState.BUBBLE_SHIELD:
                //spawn bubble shield
                pU.SetUpPowerup(PowerupManager.PowerupTypes.BUBBLE_SHIELD);
                break;
            case PowerupState.LASER_BEAM_ASSAULT:
                //spawn laser beam assault
                eT.SetUpTrigger(PowerupManager.TriggerTypes.LASER_BEAM_ASSAULT);
                break;
            case PowerupState.METEOR_SHOWER:
                //spawn meteor shower
                eT.SetUpTrigger(PowerupManager.TriggerTypes.METEOR_SHOWER);
                break;
        }
    }

}
