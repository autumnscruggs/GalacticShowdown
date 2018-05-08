using UnityEngine;
using System.Collections.Generic;

public class Missle : MonoBehaviour
{
    public ElectricMissleLauncher missleLauncher;
    public Vector2 direction;
    private Vector3 moveTranslation;
    public float speed;
    private bool canMove = true;
    public List<Collider> collidersInTriggerArea = new List<Collider>();
    private GameObject[] removeList;

    private float destructionTimer;
    private float destructionTime;

    private float timeUntilExplosion = 0.2f;

    private AudioSimple missleExplosionSound;

    public Missle()
    {
        direction = Vector2.zero;
        speed = 10f;

        destructionTime = 20f;
        destructionTimer = destructionTime;
    }

    //kills bullet over time -- prevents bullet from clogging up space if it doesn't hit anything for a while
    private void MissleLife()
    {
        destructionTimer -= Time.deltaTime;
        if (destructionTimer < 0)
        {
            Destroy(this.gameObject);
        }
    }

    //gets reference to the bulelt manager to get player identifier
    public void Instantiate(ElectricMissleLauncher eml)
    {
        missleLauncher = eml;
        timeUntilExplosion = eml.timeUntilMissleExplosion;
        missleExplosionSound = AudioManager.Instance.FindAudioClip(AudioManager.SoundName.MissleExplosion);
    }

    public void UpdateMissle()
    {
        MissleLife();
        moveTranslation = new Vector3(direction.x, direction.y) * Time.deltaTime * speed;
        this.transform.position += new Vector3(moveTranslation.x, moveTranslation.y);
    }

    private bool canDestroy = false;

    void OnTriggerEnter(Collider collider)
    {
        bool colliderisMeteor = collider.GetComponent<Meteor>();
        bool colliderIsDestructible = collider.GetComponent<DestructableRocks>() || collider.GetComponent<DestructableShields>() || collider.GetComponent<ShieldHealth>();
        bool colliderIsPlayer = collider.GetComponent<Player>() != null;
        bool colliderIsOtherPlayer = false;
        if (colliderIsPlayer)
        { colliderIsOtherPlayer = collider.GetComponent<Player>().identifier != missleLauncher.player.GetComponent<Player>().identifier; }

        if (colliderIsDestructible || colliderIsOtherPlayer || colliderisMeteor)
        {
            canMove = false;
            canDestroy = true;
        }
    }

    void OnTriggerStay(Collider collider)
    {
        bool colliderisMeteor = collider.GetComponent<Meteor>();
        bool colliderIsDestructible = collider.GetComponent<DestructableRocks>() || collider.GetComponent<DestructableShields>();
        bool colliderIsPlayer = collider.GetComponent<Player>() != null;
        bool colliderIsOtherPlayer = false;
        if (colliderIsPlayer)
        { colliderIsOtherPlayer = collider.GetComponent<Player>().identifier != missleLauncher.player.GetComponent<Player>().identifier; }

        if (colliderIsDestructible || colliderIsOtherPlayer || colliderisMeteor)
        {
            //print("collider name " + collider.name);
            if (!collidersInTriggerArea.Contains(collider))
            { collidersInTriggerArea.Add(collider); }
        }
    }

    private void StartExplosionCountdown()
    {
        timeUntilExplosion -= Time.deltaTime;
        if (timeUntilExplosion < 0)
        {
            missleExplosionSound.PlayAudioClip();
            PrepareToRemoveObjects();
            DestroyEverything();
            Destroy(this.gameObject);
        }
    }

    private void PrepareToRemoveObjects()
    {
        Collider[] collider = collidersInTriggerArea.ToArray();
        collidersInTriggerArea.Clear();
        removeList = new GameObject[collider.Length];
        for (int x = 0; x < collider.Length; x++)
        {
            if (collider[x] != null)
            { removeList[x] = collider[x].gameObject; }
            else
            {
                break;
            }
        }
    }

    private void DestroyEverything()
    {
        for (int x = 0; x < removeList.Length; x++)
        {
            if (removeList[x] != null)
            {
                if (removeList[x].GetComponent<Meteor>())
                {
                    removeList[x].SetActive(false);
                }
                else if (removeList[x].GetComponent<DestructableShields>())
                {
                    if (removeList[x].GetComponent<DestructableShields>().redShield)
                    {
                        //do nothing
                    }
                }
                else if (removeList[x].GetComponent<ShieldHealth>())
                {
                    //shield health script takes care of it
                }
                else if (removeList[x].GetComponent<Player>())
                {
                    removeList[x].GetComponent<Player>().HitByPlayer();
                }
                else
                {
                    Destroy(removeList[x].gameObject, 0.1f);
                }
            }
        }

        if (missleLauncher.outOfMissles)
        { missleLauncher.DestroyLauncher(); }

        Destroy(this.gameObject);
    }

    void Update()
    {
        if (canMove)
        { UpdateMissle(); }


        if (canDestroy)
        {
            StartExplosionCountdown();
        }
    }
}
