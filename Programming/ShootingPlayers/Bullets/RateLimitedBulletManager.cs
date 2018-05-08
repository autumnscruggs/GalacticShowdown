using UnityEngine;
using System.Collections;

public class RateLimitedBulletManager : BulletManager
{
    private float bulletRate; 
    public float bulletFireRate { get { return bulletRate; } set { bulletRate = value; bulletFireTimer = value; } }
    public float bulletFireTimer;
    public int maxShots;
    private AudioSimple gunSound = null;

    public RateLimitedBulletManager()
    {
        bulletRate = 5f;
        maxShots = 0;
    }

    //Returns True if ShotManager timer is under the value in limitShotRatetimer and max shots are less than the allowd value
    private bool CheckTimerToAllowShot()
    {
        if (bulletFireRate > 0)
        {
            if ((bulletFireTimer > 0) || ((this.bullets.Count >= maxShots) && maxShots != 0))
            {
                return false;
            }
        }

        return true;
    }

    public override Bullet Shoot(Vector2 direction, float speed)
    {
        if (!CheckTimerToAllowShot()) return null;
        //SOUND
        PlayGunSound();
        Bullet b = base.Shoot(direction, speed);
        bulletFireTimer = bulletRate;
        return b;
    }

    protected override Bullet Shoot()
    {
        if (!CheckTimerToAllowShot()) return null;
        //SOUND
        PlayGunSound();
        Bullet b = base.Shoot();
        bulletFireTimer = bulletRate;
        return b;
    }

    protected override void UpdateBulletManager()
    {
        base.UpdateBulletManager();

        //If shot rate is limited use timer 0 means unlimited
        if (bulletFireTimer > 0)
        { bulletFireTimer -= Time.deltaTime; }
    }

    private void GetGunSoundReference()
    {
        if (gunSound == null)
        {
            switch (shooterIdentity)
            {
                case Player.PlayerIdentity.PLAYER_1:
                    gunSound = AudioManager.Instance.FindAudioClip(AudioManager.SoundName.P1GunSound);
                    break;
                case Player.PlayerIdentity.PLAYER_2:
                    gunSound = AudioManager.Instance.FindAudioClip(AudioManager.SoundName.P2GunSound);
                    break;
                default:
                    gunSound = AudioManager.Instance.FindAudioClip(AudioManager.SoundName.P1GunSound);
                    break;
            }
        }
    }

    void Update ()
    {
        this.UpdateBulletManager();
        GetGunSoundReference();

    }

    private void PlayGunSound()
    {
        gunSound.PlayAudioClip();
    }
}
