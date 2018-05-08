using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletManager : MonoBehaviour
{
    public Player.PlayerIdentity shooterIdentity;
    public Vector3 bulletOriginPoint;
    public List<Bullet> bullets;
    protected List<Bullet> bulletsToRemove; //list of shots to be removed on Update is a shot is not enabled
    //Bullet reference
    private GameObject bulletPrefab;

    void Awake()
    {
        bulletPrefab = Resources.Load("Bullets/Bullet") as GameObject;
    }

    public BulletManager()
    {
        bulletOriginPoint = new Vector3(0, 0, 0);
        bullets = new List<Bullet>();
        bulletsToRemove = new List<Bullet>();
    }

    protected virtual void UpdateBulletManager()
    {
        bulletsToRemove.Clear(); //clear old shots to be removed

        //Update each shot in the Shots Collection
        foreach (var b in bullets)
        {
            if (b.enabled)
            {
                b.UpdateBullet();
            }
            else //If the shot is not enabled 
            {
                bulletsToRemove.Add(b);
            }
        }

        //Remove shots that are not enalbled anymore
        foreach (Bullet b in bulletsToRemove)
        {
            this.RemoveBullet(b);
        }
    }

    public Bullet CreateBullet()
    {
        GameObject bul = Instantiate(bulletPrefab, bulletOriginPoint, bulletPrefab.transform.rotation) as GameObject;
        bul.AddComponent<Bullet>();

        Bullet b = bul.GetComponent<Bullet>();
        b.Instantiate(this);
        AddBullet(b);

        return b;
    }

    protected virtual Bullet Shoot()
    {
        return CreateBullet();
    }

    public virtual Bullet Shoot(Vector2 direction, float speed)
    {
        Bullet b = CreateBullet();
        b.direction = direction;
        b.speed = speed;
        return b;
    }

    protected virtual void AddBullet(Bullet b)
    {
        bullets.Add(b);
    }

    protected virtual void RemoveBullet(Bullet b)
    {
        bullets.Remove(b);
        Destroy(b.gameObject);
    }

    //Actually run the above function
    void Update()
    {
        UpdateBulletManager();
    }
}
