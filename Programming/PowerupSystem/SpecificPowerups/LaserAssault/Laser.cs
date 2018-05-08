using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour
{
    [HideInInspector] public LaserBeamAssault assault;
    public enum LaserPosition { TOP, BOTTOM }
    public LaserPosition laserPosition;
    public enum LaserDirection { RIGHT, LEFT}
    public LaserDirection laserDirection;
    public float speed = 10f;

    [HideInInspector] public float topLaserInitialRotation = -90;
    [HideInInspector] public float bottomLaserInitialRotation = -20;
    [HideInInspector] public float destructionTimer = 90f;

    public void Initialize(LaserBeamAssault lba, float laserSpeed, LaserDirection dir, LaserPosition pos, float lifespan, float topRotation, float bottomRotation)
    {
        destructionTimer = lifespan;
        assault = lba;
        speed = laserSpeed;
        laserDirection = dir;
        laserPosition = pos;
        topLaserInitialRotation = topRotation;
        bottomLaserInitialRotation = bottomRotation;

        if(dir == LaserDirection.RIGHT)
        {
            if (laserPosition == LaserPosition.TOP)
            { this.transform.rotation = Quaternion.Euler(0, 0, topLaserInitialRotation); }
            else
            { this.transform.rotation = Quaternion.Euler(0, 0, bottomLaserInitialRotation); }
        }
        else
        {
            if (laserPosition == LaserPosition.TOP)
            { this.transform.rotation = Quaternion.Euler(0, 0, -topLaserInitialRotation); }
            else
            { this.transform.rotation = Quaternion.Euler(0, 0, -bottomLaserInitialRotation); }
        }
    }

    void Update()
    {
        LaserLife();
        RotateLaser();
    }

    private void RotateLaser()
    {
        Vector3 rotationDir;

        if (laserDirection == LaserDirection.LEFT)
        { rotationDir = new Vector3(0, 0, 1); }
        else
        { rotationDir = new Vector3(0, 0, -1); }

        this.transform.Rotate(rotationDir * Time.deltaTime * speed);
    }

    private void LaserLife()
    {
        destructionTimer -= Time.deltaTime;
        if (destructionTimer < 0)
        {
            Destroy(this.gameObject);
            if(assault != null)
            Destroy(assault.gameObject);
        }
    }
}
