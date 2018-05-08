using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserBeamAssault : MonoBehaviour
{
    private GameObject laserPrefab;
    private Vector3 laserOrigin;
    public List<LaserBeamAssault> lasers = new List<LaserBeamAssault>();

    [HideInInspector] public Player.PlayerIdentity playerWhoSpawnedIt;
    private float laserSpeed;
    private float laserLife;
    private float topLaserRotation;
    private float bottomLaserRotation;

    private Transform[] p1Spawns;
    private Transform[] p2Spawns;

    public float laserChargingTimer = 2f;
    private bool chargeTimer = false;
    private bool canFire = false;

    private AudioSimple laserCharging;
    private AudioSimple laserAppear;
    private AudioSimple laserFiring;

    void Awake()
    {
        laserPrefab = Resources.Load("Powerups/MisslesAndLasers/LazerPrefab") as GameObject;
    }

    void Start()
    {

    }

    public void SetUpLaserAssault(Player.PlayerIdentity identity, float laserBeamSpeed, float laserLifespan, float topRotation, float bottomRotation)
    {
        playerWhoSpawnedIt = identity;
        laserSpeed = laserBeamSpeed;
        laserLife = laserLifespan;
        topLaserRotation = topRotation;
        bottomLaserRotation = bottomRotation;

        laserAppear = AudioManager.Instance.FindAudioClip(AudioManager.SoundName.LaserAssaultAppear);
        laserCharging = AudioManager.Instance.FindAudioClip(AudioManager.SoundName.LaserAssaultCharge);
        laserFiring = AudioManager.Instance.FindAudioClip(AudioManager.SoundName.LaserAssaultFiring);

        laserAppear.PlayAudioClip();
    }

    public void GetLaserSpawnPoints(Transform[] p1, Transform[] p2)
    {
        p1Spawns = new Transform[p1.Length];
        p1Spawns = p1;
        p2Spawns = new Transform[p2.Length];
        p2Spawns = p2;
    }


    private Laser CreateLaser(Laser.LaserPosition pos)
    { 
        #region Set Laser Spawn
        if(playerWhoSpawnedIt != Player.PlayerIdentity.PLAYER_1)
        {
            switch (pos)
            {
                case Laser.LaserPosition.TOP:
                    laserOrigin = p1Spawns[1].position;
                    break;
                case Laser.LaserPosition.BOTTOM:
                    laserOrigin = p1Spawns[0].position;
                    break;
                default:
                    laserOrigin = p1Spawns[0].position;
                    break;
            }
        }
        else
        {
            switch (pos)
            {
                case Laser.LaserPosition.TOP:
                    laserOrigin = p2Spawns[1].position;
                    break;
                case Laser.LaserPosition.BOTTOM:
                    laserOrigin = p2Spawns[0].position;
                    break;
                default:
                    laserOrigin = p2Spawns[0].position;
                    break;
            }
        }
    #endregion

        GameObject laser = Instantiate(laserPrefab, laserOrigin, laserPrefab.transform.rotation) as GameObject;
        Laser l = laser.GetComponent<Laser>();

        Laser.LaserDirection direction;
        if(playerWhoSpawnedIt == Player.PlayerIdentity.PLAYER_1) { direction = Laser.LaserDirection.LEFT; }
        else { direction = Laser.LaserDirection.RIGHT; }

        l.Initialize(this, laserSpeed, direction, pos, laserLife, topLaserRotation, bottomLaserRotation);
        
        return l;
    }

    void Update()
    {
        if (chargeTimer)
        {
            laserChargingTimer -= Time.deltaTime;
            if (laserChargingTimer < 0)
            {
                chargeTimer = false;
                canFire = true;
            }
        }

        if (canFire)
        {
            FireLasers();
        }
    }

    public void SpawnLasers()
    {
        laserCharging.PlayAudioClip();
        SpawnLaserGuns(true);
        chargeTimer = true;
    }

    private void FireLasers()
    {
        canFire = false;
        laserFiring.PlayAudioClip();
        CreateLaser(Laser.LaserPosition.TOP);
        CreateLaser(Laser.LaserPosition.BOTTOM);
    }

    private void SpawnLaserGuns(bool show)
    {
        if(playerWhoSpawnedIt == Player.PlayerIdentity.PLAYER_1)
        {
            foreach (Transform p in p2Spawns)
            {
                p.GetChild(0).gameObject.SetActive(show);
            }
        }
        else
        {
            foreach (Transform p in p1Spawns)
            {
                p.GetChild(0).gameObject.SetActive(show);
            }

        }
    }

    void OnDisable()
    {
        laserFiring.StopAudio();
        SpawnLaserGuns(false);
    }

}
