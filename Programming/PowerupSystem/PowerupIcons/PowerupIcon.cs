using UnityEngine;
using System.Collections;

public class PowerupIcon : MonoBehaviour
{
    [HideInInspector] public Powerup powerup;
    private Rigidbody rigidbody;
    public float speed;
    public Vector2 direction;
    private float speedModifier;
    public enum ColliderBounds { TOP, RIGHT, BOTTOM, LEFT}
    [HideInInspector] public float stuckPreventionTime = 5f;

    void Awake()
    {
        speed = 10f;
        speedModifier = 20f;
        rigidbody = this.GetComponent<Rigidbody>();
        RandomStartingDirection();
        powerup = this.GetComponentInParent<Powerup>();
    }

    void Update()
    {
        rigidbody.AddRelativeForce(speed * direction * speedModifier * Time.deltaTime);
    }

    private void RandomStartingDirection()
    {
        do
        {
            direction = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
        }
        while (direction.sqrMagnitude <= 0);
    }

    public void ChangeDirectionBasedOffCollider(ColliderBounds bounds)
    {
        switch (bounds)
        {
            case ColliderBounds.TOP:
                direction.y = -1;
                break;
            case ColliderBounds.BOTTOM:
                direction.y = 1;
                break;
            case ColliderBounds.LEFT:
                direction.x = -1;
                break;
            case ColliderBounds.RIGHT:
                direction.x = 1;
                break;
        }
    }
}
