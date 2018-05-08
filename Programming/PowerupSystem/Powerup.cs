using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour
{
    protected PowerupManager powerupManager;
    protected PowerupManager.PowerupTypes type;
    [HideInInspector]
    public Player.PlayerIdentity playerWhoEarnedPowerup;

    [HideInInspector]
    public Transform iconSpawnpoint;
    [HideInInspector]
    public GameObject icon;
    [HideInInspector]
    public GameObject powerupPrefab;

    protected float powerupActivationLife = 5f;
    [SerializeField]
    protected float powerupActivationTimer;

    protected float powerupShelfLife = 5f;
    [SerializeField]
    protected float powerupShelfTimer;

    protected bool startActivationTimer = false;
    protected bool startShelfTimer = false;

    private AudioSimple powerupAppearSound;
    private AudioSimple powerupObtainedSound;

    private GameObject playerObject;

    public virtual void SetUpPowerup(PowerupManager.PowerupTypes pUType)
    {
        GetReferences();
        if (pUType != PowerupManager.PowerupTypes.AUTO_FIRE_BLASTER)
        { powerupActivationLife = powerupManager.pUpActivationLife; }
        else { powerupActivationLife = powerupManager.autoBlasterLife; }
        powerupActivationTimer = powerupActivationLife;
        powerupShelfLife = powerupManager.pUpShelfLife;
        powerupShelfTimer = powerupShelfLife;
        type = pUType;
        GetPowerupValues();
        SpawnIcon();
        startShelfTimer = true;
    }

    void Start()
    {
        PlayPowerupSound();
        #region Finding player object
        for (int x = 0; x < GameObject.FindObjectsOfType<Player>().Length; x++)
        {
            if (GameObject.FindObjectsOfType<Player>()[x].identifier == playerWhoEarnedPowerup)
            {
                playerObject = GameObject.FindObjectsOfType<Player>()[x].gameObject;
            }
        }
        #endregion
    }

    protected virtual void PlayPowerupSound()
    {
        powerupAppearSound = AudioManager.Instance.FindAudioClip(AudioManager.SoundName.PowerupAppear);
        powerupObtainedSound = AudioManager.Instance.FindAudioClip(AudioManager.SoundName.PowerupObtained);
        powerupAppearSound.PlayAudioClip();
    }

    void Update()
    {
        if (startShelfTimer)
        {
            PowerupTimer(ref powerupShelfTimer);
        }

        if (startActivationTimer)
        {
            PowerupTimer(ref powerupActivationTimer);
        }
    }

    private void PowerupTimer(ref float thingToCountDown)
    {
        thingToCountDown -= Time.deltaTime;
        if (thingToCountDown < 0)
        {
            DestroyPowerup();
        }
    }

    public void DestroyPowerup()
    {
        #region Stop Powerups
        if (this.GetComponent<ElectricMissleLauncher>())
        {
            this.GetComponent<ElectricMissleLauncher>().enabled = false;
            playerObject.transform.FindChild("Missile Launcher").gameObject.SetActive(false);
        }
        else if (this.GetComponent<AutoFireBlaster>())
        {
            this.GetComponent<AutoFireBlaster>().enabled = false;
            playerObject.transform.FindChild("AutoBlaster").gameObject.SetActive(false);
        }
        #endregion
        Destroy(this.gameObject);

    }

    protected void GetReferences()
    {
        powerupManager = GameObject.FindObjectOfType<PowerupManager>();
    }

    public void SetPowerupSpawn()
    {
        switch (playerWhoEarnedPowerup)
        {
            case Player.PlayerIdentity.PLAYER_1:
                iconSpawnpoint = powerupManager.p1IconSpawns[Random.Range(0, powerupManager.p1IconSpawns.Count)];
                break;
            case Player.PlayerIdentity.PLAYER_2:
                iconSpawnpoint = powerupManager.p2IconSpawns[Random.Range(0, powerupManager.p2IconSpawns.Count)];
                break;
        }
    }

    public virtual void GetPowerupValues()
    {
        SetPowerupSpawn();

        switch (type)
        {
            case PowerupManager.PowerupTypes.ELECTIC_MISSLE_LAUNCHER:
                icon = powerupManager.powerupIcons[PowerupManager.ElectricMissleLauncher];
                powerupPrefab = powerupManager.powerupPrefabs[PowerupManager.ElectricMissleLauncher];
                break;
            case PowerupManager.PowerupTypes.AUTO_FIRE_BLASTER:
                icon = powerupManager.powerupIcons[PowerupManager.AutoFireBlaster];
                powerupPrefab = powerupManager.powerupPrefabs[PowerupManager.AutoFireBlaster];
                break;
            case PowerupManager.PowerupTypes.BUBBLE_SHIELD:
                icon = powerupManager.powerupIcons[PowerupManager.BubbleShield];
                powerupPrefab = powerupManager.powerupPrefabs[PowerupManager.BubbleShield];
                break;
        }
    }
    public virtual void GrabPlayerWhoEarnedIt(Player.PlayerIdentity identity)
    {
        playerWhoEarnedPowerup = identity;
    }

    public virtual void SpawnIcon()
    {
        startShelfTimer = true;
        GameObject pIcon = Instantiate(icon, this.transform) as GameObject;
        pIcon.transform.position = iconSpawnpoint.position;
    }

    public virtual void ActivatePowerup()
    {
        //GameObject pObject = Instantiate(powerupPrefab, this.transform.position, Quaternion.identity) as GameObject;
        //pObject.transform.parent = this.transform;
        //pObject.gameObject.GetComponent<Renderer>().enabled = false;

        //Set powerup position to player who earned it position
        #region Show Powerups
        if (type == PowerupManager.PowerupTypes.ELECTIC_MISSLE_LAUNCHER)
        {
            playerObject.transform.FindChild("Missile Launcher").gameObject.SetActive(true);
        }
        else if (type == PowerupManager.PowerupTypes.AUTO_FIRE_BLASTER)
        {
            playerObject.transform.FindChild("AutoBlaster").gameObject.SetActive(true);
        }
        #endregion

        if (type != PowerupManager.PowerupTypes.ELECTIC_MISSLE_LAUNCHER)
        { startActivationTimer = true; }

        PowerupFunctionality();
    }

    protected virtual void PowerupFunctionality()
    {
        switch (type)
        {
            case PowerupManager.PowerupTypes.ELECTIC_MISSLE_LAUNCHER:
                ElectricMissleLauncher missleLauncher = this.gameObject.AddComponent<ElectricMissleLauncher>();
                missleLauncher.SetUpLauncher(playerWhoEarnedPowerup, powerupManager.emlMissleSpeed, powerupManager.maxEMLMissles, powerupManager.emlTimeUntilExplosion, this);
                break;
            case PowerupManager.PowerupTypes.AUTO_FIRE_BLASTER:
                AutoFireBlaster fireBlaster = this.gameObject.AddComponent<AutoFireBlaster>();
                fireBlaster.autoBlasterDelayAmount = powerupManager.autoBlasterDelayAmount;
                fireBlaster.RapidFire(playerWhoEarnedPowerup);
                break;
            case PowerupManager.PowerupTypes.BUBBLE_SHIELD:
                //Bubble SHield
                break;
        }
    }

    public virtual void PickUpIcon()
    {
        powerupObtainedSound.PlayAudioClip();
        startShelfTimer = false;
        Destroy(this.transform.GetChild(0).gameObject);
        ActivatePowerup();
    }

    private void AttachToPlayer(GameObject player)
    {
        if (player.GetComponentsInChildren<Powerup>().Length > 0)
        {
            Destroy(player.GetComponentInChildren<Powerup>().gameObject);
        }
        this.transform.parent = null;
        this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, (player.transform.position.z - 0.5f));
        this.transform.parent = player.transform;
    }
}
