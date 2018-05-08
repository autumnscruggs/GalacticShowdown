using UnityEngine;
using System.Collections;

public class DestructableRocks : MonoBehaviour
{
    private AudioSimple rockExplosion;

	// Use this for initialization
	void Start ()
    {
        rockExplosion = AudioManager.Instance.FindAudioClip(AudioManager.SoundName.RockExplosion);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collision)
    {
        rockExplosion.PlayAudioClip();

        //If hit by bullet
        if (collision.gameObject.GetComponent<Bullet>())
        {
            Destroy(this.gameObject);
        }
    }

}
