using UnityEngine;
using System.Collections;

public class MeteorStrikeMeteor : Meteor
{
    protected override void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<Player>())
        {
            collider.gameObject.GetComponent<Player>().HitByPlayer();
            this.meteorSound.PlayAudioClip();
        }
    }
}
