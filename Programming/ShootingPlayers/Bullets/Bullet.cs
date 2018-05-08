using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public BulletManager manager;
    public Vector2 direction;
    private Vector3 moveTranslation;
    public float speed;

    private float destructionTimer;
    private float destructionTime;

    private Color p1;
    private Color p2;

    void Awake()
    {
        //Get material colors
        Material p1Mat = Resources.Load("Bullets/P1Bullets") as Material;
        Material p2Mat = Resources.Load("Bullets/P2Bullets") as Material;
        p1 = p1Mat.color;
        p2 = p2Mat.color;
    }

    public Bullet()
    {
        direction = Vector2.zero;
        speed = 10f;

        destructionTime = 20f;
        destructionTimer = destructionTime;
    }

    //gets reference to the bulelt manager to get player identifier
    public void Instantiate(BulletManager bulletMan)
    {
        manager = bulletMan;
        //set material depending on identifier
        switch (manager.shooterIdentity)
        {
            case Player.PlayerIdentity.PLAYER_1:
                this.GetComponent<Renderer>().material.color = p1;
                break;
            case Player.PlayerIdentity.PLAYER_2:
                this.GetComponent<Renderer>().material.color = p2;
                break;
        }
    }

    public void UpdateBullet()
    {
        BulletLife();
        moveTranslation = new Vector3(direction.x, direction.y) * Time.deltaTime * speed;
        this.transform.position += new Vector3(moveTranslation.x, moveTranslation.y);
    }

    //kills bullet over time -- prevents bullet from clogging up space if it doesn't hit anything for a while
    private void BulletLife()
    {
        destructionTimer -= Time.deltaTime;
        if (destructionTimer < 0)
        {
            AllowBulletDestruction();
        }
    }

    //unenables the bullet and hides it
    private void AllowBulletDestruction()
    {
        this.enabled = false;
        this.gameObject.SetActive(false);
    }

    //Unenable on collision
    void OnTriggerEnter(Collider collision)
    {
        //If bullet hits a player
        if (collision.gameObject.GetComponent<Player>())
        {
            //if this player is not the one who just shot the bullet
            if(collision.gameObject.GetComponent<Player>().identifier != manager.shooterIdentity)
            {
                //destroy bullet
                AllowBulletDestruction();
            }
        }
        //if it's not the player and it isn't part of the ignore list, destroy it then, too
        else if(collision.GetComponent<IgnoreBulletCollision>() == null)
        {
            AllowBulletDestruction();
        }

    }
}
