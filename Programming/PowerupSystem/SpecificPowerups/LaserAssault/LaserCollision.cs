using UnityEngine;
using System.Collections;

public class LaserCollision : MonoBehaviour
{
    private Laser laser;

    void OnEnable()
    {
        laser = this.GetComponentInParent<Laser>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<Player>())
        {
            if (collider.GetComponent<Player>().identifier != laser.assault.playerWhoSpawnedIt && laser != null)
            {
                collider.GetComponent<Player>().StartRespawnSequence();
            }
        }
    }

}
