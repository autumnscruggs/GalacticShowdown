using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupManager : MonoBehaviour
{
    public const int ElectricMissleLauncher = 0;
    public const int AutoFireBlaster = 1;
    public const int BubbleShield = 2;
    public const int LaserBeamAssault = 3;
    public const int MeteorShower = 4;

    private GameObject basicPowerupPrefab;
    private GameObject basicTriggerPrefab;
    public enum PowerupTypes { ELECTIC_MISSLE_LAUNCHER, AUTO_FIRE_BLASTER, BUBBLE_SHIELD, NONE }
    public enum TriggerTypes { LASER_BEAM_ASSAULT, METEOR_SHOWER }
    public List<Transform> p1IconSpawns = new List<Transform>();
    public List<Transform> p2IconSpawns = new List<Transform>();
    public List<GameObject> powerupIcons = new List<GameObject>();
    public List<GameObject> powerupPrefabs = new List<GameObject>();

    public float pUpActivationLife = 5f;
    public float pUpShelfLife = 8f;

    [Tooltip("How many icons can float around at once")]
    public int pUpsOnFieldAllowed = 2;

    [Header("Auto Blaser")]
    public float autoBlasterDelayAmount = 0.3f;
    public float autoBlasterLife = 5f;
    [Header("Electric Missle Launcher")]
    public int maxEMLMissles = 3;
    public float emlMissleSpeed = 10f;
    public float emlTimeUntilExplosion = 0.3f;
    [Header("Laser Beam Assault")]
    public Transform[] p1LaserSpawns = new Transform[2];
    public Transform[] p2LaserSpawns = new Transform[2];
    public float laserSpeed = 5f;
    public float lbaRotationTime = 5f;
    public float lbaTopInitialRotation = -90f;
    public float lbaBottomInitialRotation = -20f;
    [Header("Meteor Strike")]
    public MeteorStike p1Strike;
    public MeteorStike p2Strike;

    [Header("Powerup Debugger")]
    public Player.PlayerIdentity debuggingPlayer;
    public bool EMLDebug = false;
    public bool AFBDebug = false;
    public bool LBADebug = false;
    public bool MSDebug = false;


    void Awake()
    {
        basicPowerupPrefab = Resources.Load("Powerups/Powerup") as GameObject;
        basicTriggerPrefab = Resources.Load("Powerups/Trigger") as GameObject;
    }

    void Update()
    {
        if (EMLDebug)
        { PowerupDebugger(PowerupTypes.ELECTIC_MISSLE_LAUNCHER, ref EMLDebug); }
        if (AFBDebug)
        { PowerupDebugger(PowerupTypes.AUTO_FIRE_BLASTER, ref AFBDebug); }
        if (LBADebug)
        { TriggerDebugger(TriggerTypes.LASER_BEAM_ASSAULT, ref LBADebug); }
        if (MSDebug)
        { TriggerDebugger(TriggerTypes.METEOR_SHOWER, ref MSDebug); }
    }

    public void PowerupDebugger(PowerupTypes type, ref bool debug)
    {
        Powerup p = SpawnPowerup();
        p.GrabPlayerWhoEarnedIt(debuggingPlayer);
        p.SetUpPowerup(type);
        debug = false;
    }
    public void TriggerDebugger(TriggerTypes type, ref bool debug)
    {
        EnvironmentalTrigger p = SpawnTrigger();
        p.GrabPlayerWhoEarnedIt(debuggingPlayer);
        p.SetUpTrigger(type);
        debug = false;
    }

    private void PreventChildrenOverflow()
    {
        if (this.transform.childCount >= pUpsOnFieldAllowed)
        {
            Destroy(this.transform.GetChild(0).gameObject);
        }
    }

    public Powerup SpawnPowerup()
    {
        PreventChildrenOverflow();
        GameObject pU = Instantiate(basicPowerupPrefab, this.transform) as GameObject;
        return pU.GetComponent<Powerup>();
    }

    public EnvironmentalTrigger SpawnTrigger()
    {
        PreventChildrenOverflow();
        GameObject pU = Instantiate(basicTriggerPrefab, this.transform) as GameObject;
        return pU.GetComponent<EnvironmentalTrigger>();
    }

}
