using UnityEngine;
using System.Collections;

public class RedShieldsBounce2 : MonoBehaviour {

    public enum ShieldSide { LOWERLEFT, LOWERRIGHT, UPPERLEFT, UPPERRIGHT}
    public ShieldSide shieldDir;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collision)
    {

        //If hit by bullet
        if (collision.gameObject.GetComponent<Bullet>())
        {

            if(shieldDir == ShieldSide.LOWERLEFT)
            {
                print("lowleft");
                collision.GetComponent<Bullet>().direction.y -= collision.GetComponent<Bullet>().direction.y - .3f;
                collision.GetComponent<Bullet>().direction.x += -1.5f;
            }

            else if(shieldDir == ShieldSide.LOWERRIGHT)
            {
                print("lowright");
                collision.GetComponent<Bullet>().direction.y -= collision.GetComponent<Bullet>().direction.y - .3f;
                collision.GetComponent<Bullet>().direction.x -= collision.GetComponent<Bullet>().direction.x - 1.5f;
            }

            if (shieldDir == ShieldSide.UPPERLEFT)
            {
                print("upleft");
                collision.GetComponent<Bullet>().direction.y += .3f;
                collision.GetComponent<Bullet>().direction.x += -1.5f;
            }

            else if (shieldDir == ShieldSide.UPPERRIGHT)
            {
                print("upright");
                collision.GetComponent<Bullet>().direction.y += .3f;
                collision.GetComponent<Bullet>().direction.x -= collision.GetComponent<Bullet>().direction.x - 1.5f;
            }

            //code that will figure out which way player that shot the bullet was going (up/down) decide which way bullet will bounce off y axis
            //CODE DOESNT WORK
            //collision.GetComponent<Bullet>().direction.y += collision.GetComponent<Bullet>().manager.shooterIdentity.
            //collision.GetComponent<Bullet>().direction.x += collision.GetComponent<>
        }
    }
}