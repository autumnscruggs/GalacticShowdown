using UnityEngine;
using System.Collections;

public class PowerupColliderBounds : MonoBehaviour
{
    public PowerupIcon.ColliderBounds bound;
    private PowerupIcon icon;
    public float stuckPreventionTimer = 0f;

    void Awake()
    {
        icon = this.GetComponentInParent<PowerupIcon>();
    }

    void Update()
    {
        StuckPrevention();
    }

    void OnTriggerEnter(Collider collider)
    {
       // print("Collision - " + bound + " with " + collider.name);
        if (collider.GetComponent<Player>())
        {
            icon.powerup.PickUpIcon();
        }
        else if (collider.GetComponent<Bullet>())
        {
            if(collider.GetComponent<Bullet>().manager.shooterIdentity == icon.powerup.playerWhoEarnedPowerup)
            {
                icon.powerup.PickUpIcon();
            }
        }
        else
        {
            stuckPreventionTimer = 0f;
            icon.ChangeDirectionBasedOffCollider(bound);
        }
    }

    private void StuckPrevention()
    {
        stuckPreventionTimer += Time.deltaTime;
        if(stuckPreventionTimer > icon.stuckPreventionTime)
        {
            icon.ChangeDirectionBasedOffCollider(bound);
            stuckPreventionTimer = 0f;
        }
    }
}
