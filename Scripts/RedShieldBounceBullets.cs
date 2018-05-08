using UnityEngine;
using System.Collections;

public class RedShieldBounceBullets : MonoBehaviour {

    private AudioSimple redShieldImpact;

    // Use this for initialization
    void Start()
    {
        redShieldImpact = AudioManager.Instance.FindAudioClip(AudioManager.SoundName.RedShieldImpact);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {
        //print("Red hit");
        //If hit by bullet

        redShieldImpact.PlayAudioClip();

        if (collision.gameObject.GetComponent<Bullet>())
        {
            //print("Bullet Bounce");

            collision.GetComponent<Bullet>().direction.y += .3f * this.GetComponent<DestructableShields>().direction.y;
            collision.GetComponent<Bullet>().direction.x -= collision.GetComponent<Bullet>().direction.x * 1.5f;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        //print("Red hit");
        //If hit by bullet
        if (other.gameObject.GetComponent<Player>())
        {
            //print("hit player");

            other.gameObject.GetComponent<Player>().HitByPlayer();
        }
    }
}
