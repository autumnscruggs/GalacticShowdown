using UnityEngine;
using System.Collections;

public class MeteorSpawner : TimedSpawner 
{
    private MeteorTypeManager meteorManager;

    public bool randomSpawnRate = true;
    public int[] timeBetweenSpawnsRandomRange;

    public Transform beginningSpawnRange;
    public Transform endSpawnRange;
    [HideInInspector] public Vector2 direction;
    public int meteorMovementSpeed;
    public int spawnerMovementSpeed;
    private Vector3 moveTranslation;
    protected bool reachedEnd;
    public enum MeteorDirection { UP, DOWN }
    public MeteorDirection meteorDirection;

    private bool powerupMeteor = false;

    private MeteorSpawner()
    {  timeBetweenSpawnsRandomRange = new int[2]; }

    void Awake()
    {
        meteorManager = GameObject.FindObjectOfType<MeteorTypeManager>();
        RandomStartingPosition();
    }

    void Start()
    {
        if(randomSpawnRate)
        SetRandomTimeSpawnRange();
    }

    private void SetRandomTimeSpawnRange()
    {
        int random = Random.Range(timeBetweenSpawnsRandomRange[0], timeBetweenSpawnsRandomRange[1]);
        this.timeBetweenSpawns = random;
    }

    private void DecideIfMeteorHasPowerup()
    {
        int random = Random.Range(0, 6);
        //2/5 chances it's a powerup
        if(random == 0 || random == 1)
        {
            powerupMeteor = true;
            DecideWhatPowerupMeteorHas();
        }
        else
        {
            powerupMeteor = false;
            spawnObject = meteorManager.meteorPrefabs[0];
        }
    }

    private void DecideWhatPowerupMeteorHas()
    {
        int random = Random.Range(0, 4);
        switch (random)
        {
            case 0: //Electic Missle Launcher
                spawnObject = meteorManager.meteorPrefabs[1];
                break;
            case 1: //Auto-Fire-Blaster
                spawnObject = meteorManager.meteorPrefabs[2];
                break;
            //case 2: //Bubble Shield
            //    spawnObject = meteorManager.meteorPrefabs[3];
                break;
            case 2: //Laser Beam Assault
                spawnObject = meteorManager.meteorPrefabs[3];
                break;
            case 3: //Meteor Shower
                spawnObject = meteorManager.meteorPrefabs[4];
                break;
        }
    }

    private void RandomStartingPosition()
    {
        //Random Starting Position
        int random = Random.Range(0, 2);
        switch (random)
        {
            case 0:
                direction = Vector2.right;
                break;
            case 1:
                direction = Vector2.left;
                break;
        }
    }

	public void Update ()
    {
        ClampPosition();
        MoveSpawner();
        DecideIfMeteorHasPowerup();
        base.SpawnOverTime();
        base.Update();
    }  

    private void ClampPosition()
    {
        if(this.transform.position.x > endSpawnRange.position.x)
        {
            direction.x = -1;
        }
        else if(this.transform.position.x < beginningSpawnRange.position.x)
        {
            direction.x = 1;
        }
    }
    private void MoveSpawner()
    {
        moveTranslation = new Vector3(direction.x, direction.y) * Time.deltaTime * spawnerMovementSpeed;
        this.transform.position += new Vector3(moveTranslation.x, moveTranslation.y);
    }
    protected override void SetupSpawnObject(GameObject go)
    {
        base.SetupSpawnObject(go);

        //change the time at which the next one is going to spawn
        if (randomSpawnRate)
            SetRandomTimeSpawnRange();

        switch (meteorDirection)
        {
            case MeteorDirection.DOWN:
                go.GetComponent<Meteor>().direction = Vector2.down;
                break;
            case MeteorDirection.UP:
                go.GetComponent<Meteor>().direction = Vector2.up;
                break;
        }

        go.GetComponent<Meteor>().speed = meteorMovementSpeed;
    }

}
