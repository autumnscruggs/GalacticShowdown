using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DestructableShields : MonoBehaviour {
    public List<GameObject> moveLocs = new List<GameObject>();
    int currentMoveLoc;//The moveLoc's number the shield is moving towards

    public float speed = 2f;

    public Vector3 direction;
    private Vector3 moveTranslation;

    public bool redShield = false;

    // Use this for initialization
    void Start()
    {
        direction = new Vector3(0, 0, 1);
        currentMoveLoc = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //MoveShield();
        ChangeTargetMoveLocation();
        ChangeShieldDirection();
        UpdateShieldMovement();
    }

    //private void MoveShield()
    //{
    //    if (moveLocScore > moveLocs.Count)
    //    {
    //        targetMoveLoc.transform.position = moveLocs[moveLocScore].transform.position;
    //    }
    //    else
    //    {
    //        moveLocScore = 0;
    //    }
    //}

    private void ChangeTargetMoveLocation()
    {
        float offset = 0.1f;
        bool inTargetMoveLocXPosRange = this.transform.position.x > (moveLocs[currentMoveLoc].transform.position.x - offset)
            && this.transform.position.x < (moveLocs[currentMoveLoc].transform.position.x + offset);
        bool inTargetMoveLocYPosRange = this.transform.position.y > (moveLocs[currentMoveLoc].transform.position.y - offset)
            && this.transform.position.y < (moveLocs[currentMoveLoc].transform.position.y + offset);

        //print("moveLocXRange = " + (moveLocs[currentMoveLoc].transform.position.x - offset) + " to " + (moveLocs[currentMoveLoc].transform.position.x + offset));
        //print("moveLocYRange = " + (moveLocs[currentMoveLoc].transform.position.y - offset) + " to " + (moveLocs[currentMoveLoc].transform.position.y + offset));

        if (inTargetMoveLocXPosRange && inTargetMoveLocYPosRange)
        {
            if (currentMoveLoc < (moveLocs.Count - 1))
            {
                currentMoveLoc++;
            }
            else
            {
                currentMoveLoc = 0;
            }
        }
    }

    private void ChangeShieldDirection()
    {
        Vector3 chaseDir = moveLocs[currentMoveLoc].transform.position - this.transform.position;
        //direction = chaseDir;
        direction = Vector3.Normalize(chaseDir);
    }

    protected void UpdateShieldMovement()
    {
        moveTranslation = new Vector3(direction.x, direction.y, direction.z) * Time.deltaTime * speed;
        this.transform.position += new Vector3(moveTranslation.x, moveTranslation.y, moveTranslation.z);
    }
}
