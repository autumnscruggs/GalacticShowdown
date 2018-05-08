using UnityEngine;
using System.Collections;

public class CameraBoundaries : MonoBehaviour
{
    //Taken from Jeff Meyer's Util

    //Fields
    public Camera mainCamera; //the camera we are going to be keeping things inside of 
    private static Vector3 bottomLeft; //the bottom left corner of the camera
    private static Vector3 topRight; //the top right corner of the camera
    private static Rect cameraRect; //the rectangle of the camera's view
    private static float cameraTopOffset = 0.4f;  //in case you need to adjust some data

    void Start()
    {
        //Sets camera boundaries
        bottomLeft = mainCamera.ScreenToWorldPoint(Vector3.zero);
        topRight = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight));
        cameraRect = new Rect(
            bottomLeft.x,
            bottomLeft.y,
            topRight.x - bottomLeft.x,
            topRight.y - bottomLeft.y);
    }

    public static void BounceOffWalls(Vector3 position, float width, float height, ref Vector2 direction)
    {
        //if(cameraRect.xMin == cameraRect.x) throw new UnityException("No instance of Util in Scene");
        //if(!cameraRect.Contains(position))
        //{
        //keep on screen
        if (((position.x + (width)) - 0.3f) <= cameraRect.xMin)
        {
            //Debug.Log(string.Format("{0} xMin {1} {2} {3} direction {4}", cameraRect.xMin, position, width, height, direction));
            //direction.x *= -1;
            direction.x = 1;
        }
        if (((position.x - (width)) + 0.3f) >= cameraRect.xMax)
        {
            //Debug.Log(string.Format("{0} xMax {1} {2} {3} direction {4}", cameraRect.xMax, position, width, height, direction));
            //direction.x *= -1;
            direction.x = -1;
        }
        if (position.y - (height) < cameraRect.yMin)
        {
            //Debug.Log(string.Format("{0} yMin {1} {2} {3} direction {4}", cameraRect.yMin, position, width, height, direction));
            //direction.y *= -1;
            direction.y = 1;
        }
        if (position.y + (height) > (cameraRect.yMax - cameraTopOffset))
        {
            //Debug.Log(string.Format("{0} yMax {1} {2} {3} direction {4}", cameraRect.yMax, position, width, height, direction));
            //direction.y *= -1;
            direction.y = -1;
        }
    }

    public static Vector2 ClampedPosition(Vector3 position, float width, float height)
    {
        position = new Vector3(
            Mathf.Clamp(position.x, (cameraRect.xMin + width), (cameraRect.xMax - width)),
            Mathf.Clamp(position.y, (cameraRect.yMin + height), (cameraRect.yMax - height)), position.z);

        return position;
    }
    public static Vector2 ClampedPosition(Vector3 position, float width, float height, float zPos)
    {
        position = new Vector3(
            Mathf.Clamp(position.x, (cameraRect.xMin + width), (cameraRect.xMax - width)),
            Mathf.Clamp(position.y, (cameraRect.yMin + height), (cameraRect.yMax - height)), zPos);

        return position;
    }

    public static bool HitBottomOfWindow(Vector3 position, float height)
    {
        if (position.y - (height) <= cameraRect.yMin)
        {
            return true;
        }
        else
        {
            //Debug.Log(string.Format("yMin = {0}, position.y = {1}, height = {2}", cameraRect.yMin, position.y, height));
            return false;
        }
    }
}

