using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    //Idenfitifier
    public enum PlayerIdentity { PLAYER_1, PLAYER_2 }
    public PlayerIdentity identifier;
    //State
    private enum PlayerState { IDLE, MOVING, RESPAWNING }
    private PlayerState state;
    private enum PoweredState { POWERED_UP, NORMAL }
    private PoweredState poweredState;
    //References
    private Rigidbody rigidbody;
    [HideInInspector] public PlayerController controller;
    //Fields
    private Vector3 moveTranslation = new Vector3(0, 0, 0);
    public Vector2 direction = new Vector2(0, 0);
    public float speed = 15;
    private float speedModifier = 100;
    [Tooltip("Decreasing this amount will increase sliding")]
    public float momentumDrag = 3;
    public bool affectedByGravity = false;
    private float respawnTimer;
    public float respawnTime = 1.5f;
    public Transform respawnLocation;
    private float originalSpeed;
    //hit by own bullet delay
    private float hitByOwnBulletDelay = 0.1f;
    private float originalBulletDelay = 0.3f;

    private AudioSimple deathSound;
    private AudioSimple hoverSound;

    void Awake()
    {
        //References
        rigidbody = this.GetComponent<Rigidbody>();
        #region Set up controller 
        controller = this.GetComponent<PlayerController>();
        switch (identifier)
        {
            case PlayerIdentity.PLAYER_1:
                controller.playerNumber = 1;
                break;
            case PlayerIdentity.PLAYER_2:
                controller.playerNumber = 2;
                break;
        }
        #endregion
        originalSpeed = speed;
        originalBulletDelay = hitByOwnBulletDelay;
    }

    void Start()
    {
        this.transform.FindChild("Missile Launcher").gameObject.SetActive(false);
        this.transform.FindChild("AutoBlaster").gameObject.SetActive(false);

        #region Get Death Sound
        switch (identifier)
        {
            case PlayerIdentity.PLAYER_1:
                deathSound = AudioManager.Instance.FindAudioClip(AudioManager.SoundName.P1ShipExplosion);
                break;
            case PlayerIdentity.PLAYER_2:
                deathSound = AudioManager.Instance.FindAudioClip(AudioManager.SoundName.P2ShipExplosion);
                break;
        }
        #endregion

        #region Get Hover Sound
        switch (identifier)
        {
            case PlayerIdentity.PLAYER_1:
                hoverSound = AudioManager.Instance.FindAudioClip(AudioManager.SoundName.P1Hovering);
                break;
            case PlayerIdentity.PLAYER_2:
                hoverSound = AudioManager.Instance.FindAudioClip(AudioManager.SoundName.P2Hovering);
                break;
        }
        #endregion

        hoverSound.LoopAudioClip();

        //Setting rigidbody values
        rigidbody.drag = momentumDrag;
        rigidbody.useGravity = affectedByGravity;
    }

    void Update()
    {
        if (hitByOwnBulletDelay > 0)
        { hitByOwnBulletDelay -= Time.deltaTime; }
        PlayerInputManager();
        ResolveState();
        //this.transform.position = CameraBoundaries.ClampedPosition(this.transform.position, sizeX, sizeY);
    }

    public void ResetBullet()
    {
        hitByOwnBulletDelay = originalBulletDelay;
    }

    private bool BulletCanHitShooter()
    {
        if(hitByOwnBulletDelay < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void PlayerInputManager()
    {
        //gets the direction from the player controller
        direction = controller.direction;
        //if we're holding any key, update the movement
        if ((InputHandler.IsHoldingAnyKey() || controller.Gamepad.ThumbsticksPressed()) && state != PlayerState.RESPAWNING)
        {
            state = PlayerState.MOVING;
            UpdateMovement();
        }
        else if (state != PlayerState.RESPAWNING)
        {
            state = PlayerState.IDLE;
        }
    }

    private void UpdateMovement()
    {
        NormalizeDiagonalSpeed();
        //With momentum
        rigidbody.AddRelativeForce(direction * speed * speedModifier * Time.deltaTime);

        //Without momentum/rigidbody
        //moveTranslation = new Vector3(direction.x, direction.y) * Time.deltaTime * speed;
        //this.transform.position += new Vector3(moveTranslation.x, moveTranslation.y);
    }

    private void NormalizeDiagonalSpeed()
    {
        //http://answers.unity3d.com/questions/22703/limit-diagonal-speed.html

        if (direction.sqrMagnitude > 1)
        {
            speed = originalSpeed * 0.7071f;
        }
        else
        {
            speed = originalSpeed;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        //If hit by bullet
        if (collision.gameObject.GetComponent<Bullet>())
        {
            //if bullet does not belong to this player
            if (collision.gameObject.GetComponent<Bullet>().manager.shooterIdentity != this.identifier)
            {
                HitByPlayer();
            }
            else if((collision.gameObject.GetComponent<Bullet>().manager.shooterIdentity == this.identifier) && BulletCanHitShooter())
            {
                HitByOwnBullet();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<RedShieldBounceBullets>())
        {
            //print("Red shield hit");
        }
    }


    private void ResolveState()
    {
        switch (state)
        {
            case PlayerState.IDLE:
                break;
            case PlayerState.MOVING:
                break;
            case PlayerState.RESPAWNING:
                Respawn();
                break;
        }
    }

    public void HitByPlayer()
    {
        StartRespawnSequence();
        IncrementPlayerScore();
    }

    public void HitByOwnBullet()
    {
        print("hit by own bullet");
        StatusText.ST.SetStatusText(identifier + " was hit by their own bullet!");

        StartRespawnSequence();
        //Increment player score
        if (identifier == PlayerIdentity.PLAYER_1)
        {
            GameManager.Instance.p1Score--;
        }
        else
        {
            GameManager.Instance.p2Score--;
        }
    }

    public void IncrementPlayerScore()
    {
        //Increment player score
        if (identifier == PlayerIdentity.PLAYER_1)
        {
            GameManager.Instance.p2Score++;
            GameManager.Instance.p1Score--;
        }
        else
        {
            GameManager.Instance.p1Score++;
            GameManager.Instance.p2Score--;
        }
    }

    public void StartRespawnSequence()
    {
        respawnTimer = respawnTime;
        state = PlayerState.RESPAWNING;
        deathSound.PlayAudioClip();
    }

    private void Respawn()
    {
        respawnTimer -= Time.deltaTime;
        if (respawnTimer > 0)
        {
            DeadState(true);
        }
        else
        {
            DeadState(false);
            this.transform.position = respawnLocation.position;
            state = PlayerState.IDLE;
        }
    }

    private void DeadState(bool dead)
    {
        //we can't unenable it because it turns off update, so we hide it instead
        this.GetComponent<Renderer>().enabled = !dead;
        this.GetComponent<Collider>().enabled = !dead;
        this.GetComponent<PlayerShootingManager>().enabled = !dead;
    }
}
