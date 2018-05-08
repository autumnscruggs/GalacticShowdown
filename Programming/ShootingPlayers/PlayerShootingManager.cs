using UnityEngine;
using System.Collections;

public class PlayerShootingManager : MonoBehaviour
{
    public Player player;
    private RateLimitedBulletManager bulletManager;
    public KeyCode shootingKey;
    public float bulletSpeed = 10f;

    private float bulletTimer = 0;
    [Tooltip("Time in between bullets to prevent rapid fire")]
    [SerializeField]
    private float bulletDelay = 3;
    [HideInInspector]
    public float originalBulletDelay;

    private bool gamepadInput = false;
    public GamepadManager.GamepadButtons gamepadShootingButton;

    void Awake()
    {
        //Referneces
        player = this.GetComponent<Player>();
        bulletManager = this.gameObject.AddComponent<RateLimitedBulletManager>();
        bulletManager.shooterIdentity = player.identifier;
        bulletManager.bulletFireRate = bulletDelay;
        originalBulletDelay = bulletDelay;
    }

    void Start()
    {
        gamepadInput = player.controller.gamepadInput;
    }

    void Update()
    {
        //Update bullet origin point
        bulletManager.bulletOriginPoint = this.transform.position;

        if (InputHandler.IsHoldingKey(shootingKey))
        {
            player.ResetBullet();
            ShootBasedOnIdentifier();
        }
        if (player.controller.Gamepad.IsConnected)
        {
            if (player.controller.Gamepad.GetButton(GamepadManager.Instance.GamepadButton(gamepadShootingButton)))
            {
                player.ResetBullet();
                ShootBasedOnIdentifier();
            }
        }
    }

    private void ShootBasedOnIdentifier()
    {
        //set bullet direction depending on identifier
        switch (player.identifier)
        {
            case Player.PlayerIdentity.PLAYER_1:
                //print("Player 1 shooting");
                bulletManager.Shoot(Vector3.right, bulletSpeed);
                break;
            case Player.PlayerIdentity.PLAYER_2:
                //print("Player 2 shooting");
                bulletManager.Shoot(Vector3.left, bulletSpeed);
                break;
        }
    }

    public void ChangeBulletDelay(float delay)
    {
        bulletDelay = delay;
        bulletManager.bulletFireRate = bulletDelay;
    }
}
