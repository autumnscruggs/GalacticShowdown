using UnityEngine;
using System.Collections;

public class AutoFireBlaster : MonoBehaviour
{
    public float activationLife = 5f;
    public float autoBlasterDelayAmount = 0.3f;

    public void RapidFire(Player.PlayerIdentity identity)
    {
        for (int x = 0; x < GameObject.FindObjectsOfType<Player>().Length; x++)
        {
            if (GameObject.FindObjectsOfType<Player>()[x].identifier == identity)
            {
                GameObject.FindObjectsOfType<Player>()[x].GetComponent<PlayerShootingManager>().ChangeBulletDelay(autoBlasterDelayAmount);
            }
        }
    }

    void OnDisable()
    {
        for (int x = 0; x < GameObject.FindObjectsOfType<Player>().Length; x++)
        {
            PlayerShootingManager pSM = GameObject.FindObjectsOfType<Player>()[x].GetComponent<PlayerShootingManager>();
            pSM.ChangeBulletDelay(pSM.originalBulletDelay);
        }
    }

}
