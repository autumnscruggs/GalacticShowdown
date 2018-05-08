using UnityEngine;
using System.Collections;

public class MeteorStrikeSpawner : MonoBehaviour {

    public enum meteorDirection { DOWN, UP}
    public meteorDirection mDir;

    public GameObject meteorPrefab;
    public float meteorMovementSpeed = 4f;

    public void FireMeteor()
    {
        GameObject meteors;
        meteors = (GameObject)Instantiate(meteorPrefab, this.transform.position, Quaternion.identity);

        switch (mDir)
        {
            case meteorDirection.DOWN:
                meteors.GetComponent<Meteor>().direction = Vector2.down;
                break;
            case meteorDirection.UP:
                meteors.GetComponent<Meteor>().direction = Vector2.up;
                break;
        }

        meteors.GetComponent<Meteor>().speed = meteorMovementSpeed;
    }
}

