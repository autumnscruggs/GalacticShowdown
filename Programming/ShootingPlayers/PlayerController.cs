using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public int playerNumber = 1;
    public KeyCode up = KeyCode.W;
    public KeyCode left = KeyCode.A;
    public KeyCode down = KeyCode.S;
    public KeyCode right = KeyCode.D;

    public bool gamepadInput = true;

    //for rotating upper ship wings based on movement direction
    public GameObject RightWing, LeftWing;//holds rotatable wing objects

    private float defaultXAngle = -23.86f;//turns back to this angle in idol
    private float minXAngle = -57.444f;
    private float maxXAngle = 47.063f;


    //direction vectors for movement
    [HideInInspector] public Vector2 direction = new Vector2(0, 0);
    [HideInInspector] public Vector2 keyDirection = new Vector2(0, 0);

    public x360_Gamepad Gamepad; 

    void Start()
    {
        Gamepad = GamepadManager.Instance.GetGamepad(playerNumber);
    }

    void Update()
    {
        //resets the wing positions for idol state
        RightWing.transform.localRotation = Quaternion.Euler(defaultXAngle, 0, 0);
        LeftWing.transform.localRotation = Quaternion.Euler(defaultXAngle, 0, 0);
        
        //Resets the directions each frame
        ResetDirection();
        //Sets direction based on input
        RightAndLeft();
        UpAndDown();
        //sets the direction based on the keyDirection and normalizes it
        direction += keyDirection;
        //direction = ForcedNormalization(direction);
    }

    private void ResetDirection()
    {
        //Sets them both back to 0
        keyDirection = new Vector2(0, 0);
        direction = new Vector2(0, 0);
    }

    public virtual void RightAndLeft()
    {
        #region Keyboard Input
        if (InputHandler.IsHoldingKey(right))
        {
            keyDirection.x += 1;

            if (this.gameObject.GetComponent<Player>().identifier == Player.PlayerIdentity.PLAYER_1)
            {
                {
                    RightWing.transform.localRotation = Quaternion.Euler(maxXAngle, 0, 0);
                    LeftWing.transform.localRotation = Quaternion.Euler(maxXAngle, 0, 0);
                }
            }
            else
            {
                {
                    RightWing.transform.localRotation = Quaternion.Euler(minXAngle, 0, 0);
                    LeftWing.transform.localRotation = Quaternion.Euler(minXAngle, 0, 0);
                }
            }

        }
        if (InputHandler.IsHoldingKey(left))
        {
            keyDirection.x += -1;

            if (this.gameObject.GetComponent<Player>().identifier == Player.PlayerIdentity.PLAYER_1)
            {
                {
                    RightWing.transform.localRotation = Quaternion.Euler(minXAngle, 0, 0);
                    LeftWing.transform.localRotation = Quaternion.Euler(minXAngle, 0, 0);
                }
            }
            else
            {
                {
                    RightWing.transform.localRotation = Quaternion.Euler(maxXAngle, 0, 0);
                    LeftWing.transform.localRotation = Quaternion.Euler(maxXAngle, 0, 0);
                }
            }

        }
        #endregion
        #region Gamepad Input
        if (gamepadInput)
        {
            if (Gamepad.GetStick_L().X < 0) //GAMEPAD LEFT
            {
                keyDirection.x += -1;

                if (this.gameObject.GetComponent<Player>().identifier == Player.PlayerIdentity.PLAYER_1)
                {
                    {
                        RightWing.transform.localRotation = Quaternion.Euler(minXAngle, 0, 0);
                        LeftWing.transform.localRotation = Quaternion.Euler(minXAngle, 0, 0);
                    }
                }
                else
                {
                    {
                        RightWing.transform.localRotation = Quaternion.Euler(maxXAngle, 0, 0);
                        LeftWing.transform.localRotation = Quaternion.Euler(maxXAngle, 0, 0);
                    }
                }

            }
            if (Gamepad.GetStick_L().X > 0) //GAMEPAD RIGHT
            {
                keyDirection.x += 1;

                if (this.gameObject.GetComponent<Player>().identifier == Player.PlayerIdentity.PLAYER_1)
                {
                    {
                        RightWing.transform.localRotation = Quaternion.Euler(maxXAngle, 0, 0);
                        LeftWing.transform.localRotation = Quaternion.Euler(maxXAngle, 0, 0);
                    }
                }
                else
                {
                    {
                        RightWing.transform.localRotation = Quaternion.Euler(minXAngle, 0, 0);
                        LeftWing.transform.localRotation = Quaternion.Euler(minXAngle, 0, 0);
                    }
                }

            }
        }
        #endregion
    }

    public virtual void UpAndDown()
    {
        #region Keyboard Input
        if (InputHandler.IsHoldingKey(up))
        {
            keyDirection.y += 1;
        }
        if (InputHandler.IsHoldingKey(down))
        {
            keyDirection.y= -1;
        }
        #endregion
        #region Gamepad Input
        if (gamepadInput)
        {
            if (Gamepad.GetStick_L().Y < 0) //GAMEPAD DOWN
            {
                keyDirection.y += -1;
            }
            if (Gamepad.GetStick_L().Y > 0) //GAMEPAD UP
            {
                keyDirection.y += 1;
            }
        }
        #endregion
    }

    //Making sure the vector can ONLY BE 1 or -1
    public Vector3 ForcedNormalization(Vector3 vec)
    {
        if (vec.x != 0)
        {
            if (vec.x > 0)
            {
                vec.x = 1;
            }
            else
            {
                vec.x = -1;
            }
        }
        if (vec.y != 0)
        {
            if (vec.y > 0)
            {
                vec.y = 1;
            }
            else
            {
                vec.y = -1;
            }

        }

        return vec;

    }

}
