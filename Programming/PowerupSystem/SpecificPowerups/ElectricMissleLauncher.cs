using UnityEngine;
using System.Collections;

public class ElectricMissleLauncher : MonoBehaviour
{
    private Powerup powerup;
    private GameObject misslePrefab;
    [HideInInspector]
    public GameObject player;
    private PlayerShootingManager playerShooter;
    public int missleAmount = 3;
    public float timeUntilMissleExplosion = 0.3f;
    private Vector3 missleOrigin;
    private float missleSpeed = 10f;
    [SerializeField]
    private int missleCount = 0;
    [HideInInspector]
    public bool outOfMissles = false;

    private float destroyDelay = 2f;
    private bool canDestroy = false;

    private AudioSimple missleFireSound;

    public void SetUpLauncher(Player.PlayerIdentity identity, float speed, int missles, float explosionTime, Powerup pu)
    {
        missleFireSound = AudioManager.Instance.FindAudioClip(AudioManager.SoundName.MissleFiring);
        misslePrefab = Resources.Load("Powerups/MisslesAndLasers/Missle") as GameObject;
        missleSpeed = speed;
        missleAmount = missles;
        powerup = pu;
        timeUntilMissleExplosion = explosionTime;

        for (int x = 0; x < GameObject.FindObjectsOfType<Player>().Length; x++)
        {
            if (GameObject.FindObjectsOfType<Player>()[x].identifier == identity)
            {
                player = GameObject.FindObjectsOfType<Player>()[x].gameObject;
                playerShooter = player.GetComponent<PlayerShootingManager>();
                playerShooter.enabled = false;
            }
        }
    }

    void Update()
    {
        //Update bullet origin point
        missleOrigin = player.transform.position;

        if (missleCount < missleAmount)
        {
            outOfMissles = false;

            if (InputHandler.IsKeyDown(playerShooter.shootingKey))
            {
                ShootBasedOnIdentifier();
            }
        }
        else
        {
            if(this.transform.childCount > 0)
            {
                this.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            }
            outOfMissles = true;
        }

        if (canDestroy)
        {
            destroyDelay -= Time.deltaTime;
            if (destroyDelay < 0)
            {
                powerup.DestroyPowerup();
            }
        }
    }

    public void DestroyLauncher()
    {
        canDestroy = true;
    }

    public Missle CreateMissle()
    {
        GameObject missle = Instantiate(misslePrefab, missleOrigin, misslePrefab.transform.rotation) as GameObject;
        missle.AddComponent<Missle>();

        Missle m = missle.GetComponent<Missle>();
        m.Instantiate(this);
        missleCount++;

        return m;
    }


    public virtual Missle Shoot(Vector2 direction, float speed)
    {
        missleFireSound.PlayAudioClip();
        Missle b = CreateMissle();
        b.direction = direction;
        b.speed = speed;
        return b;
    }

    public void ShootBasedOnIdentifier()
    {
        //set bullet direction depending on identifier
        switch (player.GetComponent<Player>().identifier)
        {
            case Player.PlayerIdentity.PLAYER_1:
                //print("Player 1 shooting");
                Shoot(Vector3.right, missleSpeed);
                break;
            case Player.PlayerIdentity.PLAYER_2:
                //print("Player 2 shooting");
                Shoot(Vector3.left, missleSpeed);
                break;
        }
    }

    void OnDisable()
    {
        playerShooter.enabled = true;
    }


}
